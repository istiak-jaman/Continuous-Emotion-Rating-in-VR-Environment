using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class ContinuousRating : MonoBehaviour
{
    [SerializeField] private Material mat;
    [SerializeField] private XRNode node;
    [SerializeField] private InputDevice controller;
    Vector2 axis;
    [SerializeField] private bool saturation = false;
    [SerializeField] private bool opacity = false;

    public Color col;

    private void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);

    }
    //We are working in four quadrants, Color and intensity are related to position on touchpad.
    void Update()
    {

        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out axis);
        if(opacity == true)
        {
            Opacity();
        }
        if(saturation == true)
        {
            Saturation();
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
    public void Saturation()
    {
        float H;
        float s;
        float v;
        Color.RGBToHSV(Color.red, out H, out s, out v);

        s = .5f;
        Color newCol = Color.HSVToRGB(H, s, v);

        Debug.Log(H + "" + s + "" + v);

        
  
    }
}

