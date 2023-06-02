using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInputSource : MonoBehaviour
{
    void Start()
    {
        // Tell the input module about this camera
        MultiInputModule.main.AddCamera(GetComponent<Camera>(), 5); // Layer 5 is the default Unity UI layer; change if needed
    }
}