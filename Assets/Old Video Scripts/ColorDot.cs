using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class ColorDot : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    [SerializeField] GameObject circle;
    Vector2 axis;
    private float alpha;

    private void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }
    //We are working in four quadrants, Color and intensity are related to position on touchpad.
    void Update()
    {

        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        Opacity();
        if (axis.x != 0 || axis.y != 0)
        {
            alpha = Mathf.Sqrt((axis.x * axis.x) + (axis.y * axis.y));
            alpha *= .5f;
            circle.transform.localScale = new Vector3(alpha, 0, alpha);
        }
        else
        {
            circle.transform.localScale = new Vector3(.1f, 0, .1f);

        }

    }
    public void Opacity()
    {

        //First if for quadrant color.
        if (axis.x > 0.01 && axis.y > 0.01)
        {
            float alpha = (axis.x + axis.y) / 1.8f;
            //Second ifs for intensity(change the alpha).
            if (axis.x > 0.1 && axis.y > 0.1)
            {
                mat.color = new Color(0f, 1f, 0f, alpha);
            }
            else
            {
                mat.color = new Color(0f, 1f, 0f, .25f);
            }

        }
        else if (axis.x < -0.01 && axis.y > 0.01)
        {
            float alpha = (-axis.x + axis.y) / 2;
            if (axis.x < -0.1 && axis.y > 0.1)
            {
                mat.color = new Color(0f, 0f, 1f, alpha);
            }

            else
            {
                mat.color = new Color(0f, 0f, 1f, .25f);
            }
        }
        else if (axis.x > 0.01 && axis.y < -0.01)
        {
            float alpha = (axis.x - axis.y) / 2;
            if (axis.x > 0.1 && axis.y < -0.1)
            {
                mat.color = new Color(1f, .92f, .016f, alpha);
            }

            else
            {
                mat.color = new Color(1f, .92f, .016f, .25f);
            }
        }
        else if (axis.x < -0.01 && axis.y < -0.01)
        {
            float alpha = -(axis.x + axis.y) / 2;
            if (axis.x < -0.1 && axis.y < -0.1)
            {
                mat.color = new Color(1f, 0f, 0f, alpha);
            }

            else
            {
                mat.color = new Color(1f, 0f, 0f, .25f);
            }
        }
        else
        {
            mat.color = new Color(1f, 1f, 1f, 0.25f);
        }
    }
}