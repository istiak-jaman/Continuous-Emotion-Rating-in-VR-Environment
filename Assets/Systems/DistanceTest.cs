using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Place this on something attached to the camera. It'll move it back and forth with the right hand.
/// This can be used to test how far away something appears in the videos.
/// </summary>
public class DistanceTest : MonoBehaviour
{
    [SerializeField] private bool scaleWithDistance;
    [SerializeField] private TMPro.TextMeshPro text;

    private InputDevice controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 axis;
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);

        Vector3 newDist = new Vector3(0, 0, transform.localPosition.z + axis.y * Time.deltaTime);
        transform.localPosition = newDist;
        text.text = newDist.z.ToString();
    }
}
