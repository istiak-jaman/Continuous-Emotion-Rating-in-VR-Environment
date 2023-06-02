using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DotSize : MonoBehaviour
{

    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    [SerializeField] GameObject circle;
    [SerializeField] Material mat;
    Vector2 axis;
    private float alpha;
    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }

    void Update()
    {
        //When we are touching the touch pad, we want to calculate distance from circle, then multiply by a flaot decimal for VR scaling. Otherwise we want a small dot.
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        if(axis.x != 0 || axis.y != 0)
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
}
