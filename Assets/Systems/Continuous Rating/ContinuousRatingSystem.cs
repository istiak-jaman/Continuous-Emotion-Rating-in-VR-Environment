using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Abstracts the rating concepts on the controller into arousal and valence.
/// Allows the rating interfaces themselves to just ask this component for valence/arousal scores.
/// </summary>
public class ContinuousRatingSystem : MonoBehaviour
{
    public enum Hand { LEFT, RIGHT }
    public enum Axis { X, Y }

    [Header("Options")]
    [SerializeField] private Hand valenceHand;
    [SerializeField] private Hand arousalHand;
    [SerializeField] private Axis valenceAxis, arousalAxis;
    [SerializeField] private bool mapToSquare;

    private InputDevice leftHand, rightHand;

    [Header("Values")]
    public float valence;
    public float arousal;
    public bool help;

    public static ContinuousRatingSystem instance;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    // Start is called before the first frame update
    void Start() {
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHand = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update() {
        Vector2 arousalVec;
        if (arousalHand == Hand.LEFT) leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out arousalVec);
        else rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out arousalVec);

        arousal = (arousalAxis == Axis.X) ? arousalVec.x : arousalVec.y;

        Vector2 valenceVec;
        if (valenceHand == Hand.LEFT) leftHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out valenceVec);
        else rightHand.TryGetFeatureValue(CommonUsages.primary2DAxis, out valenceVec);

        valence = (valenceAxis == Axis.X) ? valenceVec.x : valenceVec.y;

        // These A/V are just coordinates on the circle, if we want it to look better on a grid, map it to square
        // This only makes sense if both are on a single hand and opposite axes
        if (mapToSquare) {
            Vector2 xy = new Vector2();
            if (valenceAxis == Axis.X && arousalAxis == Axis.Y) {
                xy = DiscToSquare(valence, arousal);
                valence = xy.x; arousal = xy.y;
            } else if (valenceAxis == Axis.Y && arousalAxis == Axis.X) {
                xy = DiscToSquare(arousal, valence);
                valence = xy.y; arousal = xy.x;
            }
        }

        bool lhelp, rhelp;
        leftHand.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out lhelp);
        rightHand.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out rhelp);
        help = lhelp || rhelp;
    }

    private float twosqrt = 2.82842712475f;
    Vector2 DiscToSquare(float u, float v) {
        //u = Mathf.Clamp(u, -.7071f, .7071f);
        //v = Mathf.Clamp(v, -.7071f, .7071f);

        float u2 = u * u;
        float v2 = v * v;

        float xinner = (twosqrt * u) + u2 - v2;
        float yinner = (twosqrt * v) - u2 + v2;

        float x = (.5f * Mathf.Sqrt(2 + (twosqrt * u) + u2 - v2)) - (.5f * Mathf.Sqrt(2 - (twosqrt * u) + u2 - v2));
        float y = (.5f * Mathf.Sqrt(2 + (twosqrt * v) - u2 + v2)) - (.5f * Mathf.Sqrt(2 - (twosqrt * v) - u2 + v2));

        return new Vector2(x, y);
    }

    public void SetOptions(Hand vhand, Hand ahand, Axis vaxis, Axis aaxis, bool mts) {
        valenceHand = vhand;
        arousalHand = ahand;
        valenceAxis = vaxis;
        arousalAxis = aaxis;
        mapToSquare = mts;
    }
}
