using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class CEAPCircle : MonoBehaviour
{
    [SerializeField] private Material mat;

    [Header("Options")]
    [Tooltip("Should intensity (distance from center) be represented by object opacity")]
    [SerializeField] private bool opacityIntensity;
    [Tooltip("Should intensity (distance from center) be represented by object size")]
    [SerializeField] private bool sizeIntensity;

    [Header("Color Options")]
    [SerializeField] private Color stressedColor;
    [SerializeField] private Color excitedColor, sadColor, relaxedColor, neutralColor;

    [Header("Size Options")]
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;

    private ContinuousRatingSystem ratings;

    //new code

    //List<float> listValence = new ArrayList<float>();
    //List<float> listArousal = new ArrayList<float>();

    //float meanValence, meanArousal;

    // Start is called before the first frame update
    void Start()
    {
        ratings = ContinuousRatingSystem.instance;

    }
        // Update is called once per frame
    void Update()
    {

        //read valence and arousal here
        //listValence.Add(ratings.valence);
        //listArousal.Add(ratings.arousal);

        /*if (ratings.valence != 0)
        {
            *//*using (var fs = new FileStream("data recordings/Valence_CEAP_Color.csv", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.valence.ToString());

            }*//*
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/CEAP Color/ Valence__CurvingSmile {0}.csv", timestamp);

            using (var fs = new FileStream(filename, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.valence.ToString());
            }
        }
        if (ratings.arousal != 0)
        {
            *//*using (var fs = new FileStream("data recordings/Arousal__CEAP_color.csv", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.arousal.ToString());

            }*//*
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/CEAP Color/ Valence__CurvingSmile {0}.csv", timestamp);

            using (var fs = new FileStream(filename, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.arousal.ToString());
            }
        }*/
        if (ratings.arousal != 0 || ratings.valence != 0)
        {
            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/CEAP Color/CEAP_Color {0}.csv", timestamp);

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





        Color col = GetQuadrantColor(ratings.valence, ratings.arousal);
        if (opacityIntensity) {
            float opacity = GetOpacity(ratings.valence, ratings.arousal);
            col.a = opacity;
        }
        mat.color = col;

        if (sizeIntensity) {
            float s = GetSize(ratings.valence, ratings.arousal);
            transform.localScale = new Vector3(s, float.Epsilon, s);
        }
    }

    /// <summary>
    /// Get the standard, non-gradiated color for the given A/V quadrant
    /// </summary>
    /// <param name="valence"></param>
    /// <param name="arousal"></param>
    /// <returns></returns>
    public Color GetQuadrantColor(float valence, float arousal) {

        
       
        if (valence > 0 && arousal > 0) return excitedColor;
        else if (valence < 0 && arousal > 0) return stressedColor;
        else if (valence < 0 && arousal < 0) return sadColor;
        else if (valence > 0 && arousal < 0) return relaxedColor;
        else return neutralColor;
    }

    // Opacity is distance from center, so return that.
    private float GetOpacity(float valence, float arousal) {
        return Vector2.Distance(new Vector2(valence, arousal), Vector2.zero);
    }

    private float GetSize(float valence, float arousal) {
        float t = Vector2.Distance(new Vector2(valence, arousal), Vector2.zero);
        return Mathf.Lerp(minSize, maxSize, t);
    }




}
