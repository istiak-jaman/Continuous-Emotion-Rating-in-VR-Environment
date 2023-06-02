using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.IO;
using System;

public class CurvingSmile : MonoBehaviour
{
    [SerializeField] private XRNode node;
    private InputDevice controller;
    [SerializeField] private Transform upPos, downPos;
    Vector2 touchPos;

    [SerializeField] private Transform endL, endR, midL, midR;

    private ContinuousRatingSystem ratings;

    // Start is called before the first frame update
    void Start()
    {
        controller = InputDevices.GetDeviceAtXRNode(node);
        ratings = ContinuousRatingSystem.instance;
    }

    // Update is called once per frame
    void Update()
    {

        controller.TryGetFeatureValue(CommonUsages.primary2DAxis, out touchPos);

        float inputYPos = (touchPos.y + 1) / 2; // Normalize y to 0-1
        float midYPos = Mathf.Lerp(upPos.localPosition.y, downPos.localPosition.y, inputYPos);
        float endYPos = Mathf.Lerp(downPos.localPosition.y, upPos.localPosition.y, inputYPos);

        endL.localPosition = new Vector3(endL.localPosition.x, endYPos, endL.localPosition.z);
        endR.localPosition = new Vector3(endR.localPosition.x, endYPos, endR.localPosition.z);
        midL.localPosition = new Vector3(midL.localPosition.x, midYPos, midL.localPosition.z);
        midR.localPosition = new Vector3(midR.localPosition.x, midYPos, midR.localPosition.z);

       /* if (ratings.valence != 0)
        {
            *//*using (var fs = new FileStream("data recordings/Valence__CurvingSmile.csv", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.valence.ToString());

            }*//*
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/Curving Smile/ Valence__CurvingSmile {0}.csv", timestamp);

            using (var fs = new FileStream(filename, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.valence.ToString());
            }
        }

        if (ratings.arousal != 0)
        {
            *//*using (var fs = new FileStream("data recordings/Arousal__CurvingSmile.csv", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.arousal.ToString());

            }*//*
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/Curving Smile/ Valence__CurvingSmile {0}.csv", timestamp);

            using (var fs = new FileStream(filename, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.arousal.ToString());
            }

        }*/

        if (ratings.arousal !=0 || ratings.valence != 0)
        {
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/Curving Smile/CurvingSmile {0}.csv", timestamp);

            using (var fs = new FileStream(filename, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                if (fs.Length == 0) // Check if the file is empty
                {
                    sw.WriteLine("Valence, Arousal"); // Write the column names
                }
                sw.WriteLine($"{ratings.valence.ToString()}, {ratings.arousal.ToString()}"); // Write the values
            }
        }


    }
}
