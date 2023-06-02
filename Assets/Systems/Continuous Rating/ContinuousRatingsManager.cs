using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuousRatingsManager : MonoBehaviour
{
    [SerializeField] private bool useCEAPColor = false;
    [SerializeField] private bool useEmojiGrid;
    [SerializeField] private bool useSAM;
    [SerializeField] private bool useCurvingSmile;
    [SerializeField] private GameObject CEAPColor;
    [SerializeField] private GameObject emojiGrid;
    [SerializeField] private GameObject SAM;
    [SerializeField] private GameObject curvingSmile;

    //This is just to toggle the rating interfaces, allows you to add on to list of them.
    void Start()
    {
        
        
    }

    void Update()
    {
        //if true, sets active when play is hit, if false, sets inactive when play is hit. Allows multiple to be used at once if designer chooses so and uses both hands.
        CEAPColor.SetActive(useCEAPColor);
        emojiGrid.SetActive(useEmojiGrid);
        SAM.SetActive(useSAM);
        curvingSmile.SetActive(useCurvingSmile);
    }
}
