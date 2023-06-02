using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;
public class ValenceArousal : MonoBehaviour
{
    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    private Vector2 axis;
    [SerializeField] private Image smallOrb;


    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }

    // Update is called once per frame
    void Update()
    {
        //we only want to move in x direction for sam box, so clamp y to 0. Have to multiply by scalar of 3 in order to reach both ends of the SAM box.
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        axis.y = 0;
        smallOrb.transform.localPosition = axis * 3f;
        
    }
}
