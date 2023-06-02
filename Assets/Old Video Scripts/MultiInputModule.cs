using UnityEngine;
using UnityEngine.EventSystems;

using System.Collections.Generic;

public class MultiInputModule : PointerInputModule
{
    private List<PointerSource> sources = new List<PointerSource>();

    // Holds a controller and associated event data
    private class PointerSource
    {
        public Camera camera; // If based on an existing camera
        public ControllerInputSource controller; // If based on a VR controller

        public PointerEventData eventData;
        public int targetLayers;
        // Pressed UI element
        public GameObject pressObject;
    }

    // Single camera that is set to each controller's position
    private Camera cam;

    public static MultiInputModule main;

    public void Awake()
    {
        main = this;
    }

    public override void ActivateModule()
    {
        GameObject camObj = new GameObject("UI Raycast Camera");
        cam = camObj.AddComponent<Camera>();
        camObj.AddComponent<PhysicsRaycaster>();

        cam.clearFlags = CameraClearFlags.Nothing;
        cam.enabled = false;
        cam.stereoTargetEye = StereoTargetEyeMask.None;
    }

    bool init = false;
    void ActivateDelayed()
    {
        Canvas[] canvases = Resources.FindObjectsOfTypeAll<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
                canvas.worldCamera = cam;
        }
        init = true;
    }

    public void Deselect()
    {
        eventSystem.SetSelectedGameObject(null);
    }

    GameObject prevSelected = null;

    private void SelectObject(GameObject obj)
    {
        print("select");
        // Only use with a single input source
        Deselect();
        if (obj && ExecuteEvents.GetEventHandler<ISelectHandler>(obj))
            eventSystem.SetSelectedGameObject(obj);
        prevSelected = obj;
    }

    RaycastResult FindFirstRaycastWorld(List<RaycastResult> candidates, int layer)
    {
        for (var i = 0; i < candidates.Count; ++i)
        {
            if (candidates[i].gameObject == null)
                continue;
            if (((1 << candidates[i].gameObject.layer) & layer) == 0)
                continue;
            return candidates[i];
        }
        return new RaycastResult();
    }

    public void AddSource(ControllerInputSource controller, int layer)
    {
        // Add new controller-based source
        PointerSource source = new PointerSource();
        source.controller = controller;
        source.eventData = new PointerEventData(eventSystem);
        source.pressObject = null;
        source.targetLayers = (1 << layer);
        sources.Add(source);
    }

    public void AddCamera(Camera camera, int layer)
    {
        // Add new camera-based source
        PointerSource source = new PointerSource();
        source.camera = camera;
        source.controller = null;
        source.eventData = new PointerEventData(eventSystem);
        source.pressObject = null;
        source.targetLayers = (1 << layer);
        sources.Add(source);
    }

    public override void Process()
    {
        if (!init)
            ActivateDelayed();

        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);

        foreach (PointerSource source in sources)
        {
            if (source.controller != null)
            {
                cam.transform.position = source.controller.transform.position;
                cam.transform.forward = source.controller.transform.forward;
                cam.fieldOfView = 5;
                cam.nearClipPlane = 0.01f;
                cam.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

                // Position of raycast should be the center of the camera
                source.eventData.position = new Vector2(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f);
            }
            else if (source.camera)
            {
                cam.transform.position = source.camera.transform.position;
                cam.transform.forward = source.camera.transform.forward;
                cam.fieldOfView = source.camera.fieldOfView;
                cam.nearClipPlane = source.camera.nearClipPlane;
                cam.rect = source.camera.rect;

                // Position of raycast should be the mouse position
                source.eventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            source.eventData.delta = Vector2.zero;
            source.eventData.scrollDelta = Vector2.zero;

            // Trigger raycast
            eventSystem.RaycastAll(source.eventData, m_RaycastResultCache);
            source.eventData.pointerCurrentRaycast = FindFirstRaycastWorld(m_RaycastResultCache, source.targetLayers);
            m_RaycastResultCache.Clear();

            GameObject hitObject = source.eventData.pointerCurrentRaycast.gameObject;
            float distance = source.eventData.pointerCurrentRaycast.distance;
            base.HandlePointerExitAndEnter(source.eventData, hitObject);
            if (hitObject)
            {
                if (source.controller)
                {
                    source.controller.NotifyUIHit(true, distance, hitObject, source.eventData.position);
                }
                if (ButtonDown(source))
                {
                    print("Clicked " + hitObject.name);
                    source.eventData.button = PointerEventData.InputButton.Left;
                    source.eventData.pressPosition = source.eventData.position;
                    source.eventData.pointerPressRaycast = source.eventData.pointerCurrentRaycast;
                    source.eventData.pointerPress = null;
                    source.pressObject = hitObject;

                    GameObject newObject = ExecuteEvents.ExecuteHierarchy(source.pressObject, source.eventData, ExecuteEvents.pointerDownHandler);
                    if (newObject)
                    {
                        source.pressObject = newObject;
                        ExecuteEvents.Execute(source.pressObject, source.eventData, ExecuteEvents.pointerClickHandler);
                    }
                    else
                    {
                        newObject = ExecuteEvents.ExecuteHierarchy(source.pressObject, source.eventData, ExecuteEvents.pointerClickHandler);
                        if (newObject)
                            source.pressObject = newObject;
                    }
                    source.eventData.pointerPress = source.pressObject;
                    Deselect();
                    ExecuteEvents.Execute(source.pressObject, source.eventData, ExecuteEvents.beginDragHandler);
                    source.eventData.pointerDrag = source.pressObject;
                }
            }
            else
            {
                if (source.controller)
                    source.controller.NotifyUIHit(false, 0, null, Vector2.zero);
            }
            if (ButtonUp(source))
            {
                if (source.pressObject)
                {
                    ExecuteEvents.Execute(source.pressObject, source.eventData, ExecuteEvents.endDragHandler);
                    if (hitObject)
                    {
                        ExecuteEvents.ExecuteHierarchy(hitObject, source.eventData, ExecuteEvents.dropHandler);
                    }
                    ExecuteEvents.Execute(source.pressObject, source.eventData, ExecuteEvents.pointerUpHandler);
                    source.eventData.pointerDrag = null;
                    source.eventData.pointerPress = null;
                    source.pressObject = null;
                }
            }
            if (source.pressObject)
            {
                ExecuteEvents.Execute(source.pressObject, source.eventData, ExecuteEvents.dragHandler);
            }
        }
    }

    bool ButtonDown(PointerSource source)
    {
        if (source.controller != null)
        {
            return source.controller.PrimaryButtonDown();
        }
        return Input.GetMouseButtonDown(0);
    }
    bool ButtonUp(PointerSource source)
    {
        if (source.controller != null)
            return source.controller.PrimaryButtonUp();
        return Input.GetMouseButtonUp(0);
    }

    public override void DeactivateModule()
    {
        base.DeactivateModule();
        ClearSelection();
    }
}
