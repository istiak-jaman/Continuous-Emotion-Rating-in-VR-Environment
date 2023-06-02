using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControllerInputSource : MonoBehaviour
{
    public XRNode node; // VR node to track (is this the left hand or the right hand?)
    InputDevice device; // VR controller device

    bool click = false;
    bool lastClick = false;

    void Start()
    {
        // Tell the input module about this controller
        MultiInputModule.main.AddSource(this, 5); // Layer 5 is the default Unity UI layer; change if needed
    }

    void Update()
    {
        // "lastClick" stores the pressed status of the trigger on the previous frame.
        // This is stored because the Unity VR APIs don't report transitions between
        // pressed and unpressed, so we need to keep track of this manually.
        lastClick = click;

        if (device.isValid)
        {
            // It's possible that the VR controller couldn't be initialized (or isn't plugged in?)
            // so only execute this code if the controller is actually active

            // This function returns the instantaneous status of the controller's trigger button.
            bool v = device.TryGetFeatureValue(CommonUsages.triggerButton, out click);
            //print("Click " + click + ", Last Click" + lastClick + " Worked: " + v);

            Vector2 joystick;
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystick);
            //print("Joy: " + joystick);
        }
        else
        {
            // This line tries to find the VR control0ler corresponding to the hand set in "node"
            device = InputDevices.GetDeviceAtXRNode(node);
        }
    }

    public bool PrimaryButtonDown()
    {
        // If the trigger is pressed, and it wasn't pressed on the previous frame:
        // the user must have *just* pressed the trigger.
        return click && !lastClick;
    }
    public bool PrimaryButtonUp()
    {
        // If the trigger is not pressed, and it was pressed on the previous frame:
        // the user must have *just* let go of the trigger.
        return lastClick && !click;
    }

    public void NotifyUIHit(bool hit, float distance, GameObject target, Vector2 position)
    {
        // This function is responsible for setting up the ray that comes from the controller
        // Feel free to remove / change if needed
        GetComponent<LineRenderer>().enabled = hit;
        if (hit)
        {
            GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, distance));
        }
    }
}
