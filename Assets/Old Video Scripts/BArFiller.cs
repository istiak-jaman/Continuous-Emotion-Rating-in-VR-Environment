using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class BArFiller : MonoBehaviour
{

    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    [SerializeField] private Image bar;
    private Vector2 axis;
    private float x = 0f;
    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }

    // Update is called once per frame
    void Update()
    {
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        //simply fill bar with value of the x axis value.
        x = axis.x;
        bar.fillAmount = x;
    }
}
