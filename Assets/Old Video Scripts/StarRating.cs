using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class StarRating : MonoBehaviour
{

    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    [SerializeField] private Texture noCircle;
    [SerializeField] private Texture oneCircle;
    [SerializeField] private Texture twoCircle;
    [SerializeField] private Texture threeCircle;
    [SerializeField] private Texture fourCircle;
    [SerializeField] private Texture fiveCircle;
    [SerializeField] private RawImage circle;

    private Vector2 axis;

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);
    }

    // Update is called once per frame
    void Update()
    {
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        //left to right, as we get larger in value, the more stars we accumulate.
        if(axis.x <= -.75)
        {
            circle.texture = noCircle;
        }
        else if(axis.x >= -.75 && axis.x <= -.45)
        {
            circle.texture = oneCircle;
        }
        else if (axis.x >= -.45 && axis.x <= -.15)
        {
            circle.texture = twoCircle;
        }
        else if (axis.x >= -.15 && axis.x <= .15)
        {
            circle.texture = threeCircle;
        }
        else if (axis.x >= .15 && axis.x <= .45)
        {
            circle.texture = fourCircle;
        }
        else if (axis.x >= .45 && axis.x <= .75)
        {
            circle.texture = fiveCircle;
        }
        
    }
}
