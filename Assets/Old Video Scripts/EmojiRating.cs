using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;

public class EmojiRating : MonoBehaviour
{
    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    private Vector2 axis;

    [SerializeField] private Texture smile;
    [SerializeField] private Texture frown;
    [SerializeField] private Texture mid;
    [SerializeField] private Texture worst;
    [SerializeField] RawImage baseFace;

    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }

    // Update is called once per frame
    void Update()
    {
        //From the left most side of touch pad to right most side, we set texture of image dependent on the value.
        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        if(axis.x <= -.5)
        {
            baseFace.texture = worst;
        }
        if(axis.x >= -.5 && axis.x <= 0)
        {
            baseFace.texture = frown;
        }
        if (axis.x >= 0 && axis.x <= .5)
        {
            baseFace.texture = mid;
        }
        if (axis.x >= .5)
        {
            baseFace.texture = smile;
        }


    }
}
