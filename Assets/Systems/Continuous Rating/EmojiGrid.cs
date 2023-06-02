using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class EmojiGrid : MonoBehaviour
{
    [SerializeField] private Transform ratingCircle;

    private ContinuousRatingSystem ratings;

    // Start is called before the first frame update
    void Start()
    {
        ratings = ContinuousRatingSystem.instance;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (ratings.valence != 0)
        {
            *//*using (var fs = new FileStream("data recordings/Valence_EmojiGrid.csv", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.valence.ToString());

            }*//*

            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/EmojiGrid/ Valence__CurvingSmile {0}.csv", timestamp);

            using (var fs = new FileStream(filename, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.valence.ToString());
            }
        }
        if (ratings.arousal != 0)
        {
            *//*using (var fs = new FileStream("data recordings/Arousal__EmojiGrid.csv", FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(ratings.arousal.ToString());

            }*//*

            DateTime now = DateTime.Now;
            string timestamp = string.Format("{0}-{1} {2}-{3}", now.Month, now.Day, now.Hour, now.Minute);
            string filename = string.Format("data recordings/EmojiGrid/ Valence__CurvingSmile {0}.csv", timestamp);

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
            string filename = string.Format("data recordings/EmojiGrid/EmojiGrid {0}.csv", timestamp);

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







        ratingCircle.localPosition = new Vector2(ratings.valence, ratings.arousal) * .0585f;
    }
}
