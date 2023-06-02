using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to a continuous rating system to have it set defaults for how A/V
/// is set by input on start. This won't really work with multiple interfaces at a time.
/// </summary>
public class SetInputDefaults : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private ContinuousRatingSystem.Hand valenceHand;
    [SerializeField] private ContinuousRatingSystem.Hand arousalHand;
    [SerializeField] private ContinuousRatingSystem.Axis valenceAxis, arousalAxis;
    [SerializeField] private bool mapToSquare;

    // Start is called before the first frame update
    void OnEnable()
    {
        ContinuousRatingSystem.instance.SetOptions(
            valenceHand, arousalHand, valenceAxis, arousalAxis, mapToSquare);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
