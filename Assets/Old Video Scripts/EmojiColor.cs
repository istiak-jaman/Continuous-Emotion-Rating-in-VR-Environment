using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class EmojiColor : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image Orb;
    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    private Vector2 axis;
    private bool trigg;
    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }

    void Update()
    {
        //Get the Vector2 for the touchpad and use that to change position of the orb based on that Vector2.
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        controller.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out trigg);
        if (trigg == true)
        {
            canvas.transform.localPosition = new Vector3(0, 0.02f, 0.32f);
        }
        if (!trigg)
        {
            canvas.transform.localPosition = new Vector3(0.17f, -0.2f, 0.55f);
        }
        Orb.transform.localPosition = axis * .07f;
        //Have to scale down to fit size of VR


        //First if for quadrant color.
        if (axis.x > 0.01 && axis.y > 0.01)
        {
            float alpha = (axis.x + axis.y) / 1.8f;
            //Second ifs for intensity(change the alpha).
            if (axis.x > 0.1 && axis.y > 0.1)
            {
                Orb.color = new Color(0f, 1f, 0f, alpha);
            }
            else
            {
                Orb.color = new Color(0f, 1f, 0f, .25f);
            }

        }
        else if (axis.x < -0.01 && axis.y > 0.01)
        {
            float alpha = (-axis.x + axis.y) / 2;
            if (axis.x < -0.1 && axis.y > 0.1)
            {
                Orb.color = new Color(0f, 0f, 1f, alpha);
            }

            else
            {
                Orb.color = new Color(0f, 0f, 1f, .25f);
            }
        }
        else if (axis.x > 0.01 && axis.y < -0.01)
        {
            float alpha = (axis.x - axis.y) / 2;
            if (axis.x > 0.1 && axis.y < -0.1)
            {
                Orb.color = new Color(1f, .92f, .016f, alpha);
            }

            else
            {
                Orb.color = new Color(1f, .92f, .016f, .25f);
            }
        }
        else if (axis.x < -0.01 && axis.y < -0.01)
        {
            float alpha = -(axis.x + axis.y) / 2;
            if (axis.x < -0.1 && axis.y < -0.1)
            {
                Orb.color = new Color(1f, 0f, 0f, alpha);
            }

            else
            {
                Orb.color = new Color(1f, 0f, 0f, .25f);
            }
        }
        else
        {
            Orb.color = new Color(.5f, .5f, .5f, 0.35f);
        }
    }
}
