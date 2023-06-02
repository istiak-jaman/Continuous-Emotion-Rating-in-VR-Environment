
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic functions to help recording
/// </summary>
public static class RecordingUtils
{
    public static string ToS(Vector3 v, int trunc)
    {
        string line = "";
        line += string.Format("{0:f" + trunc + "}", v.x) + ",";
        line += string.Format("{0:f" + trunc + "}", v.y) + ",";
        line += string.Format("{0:f" + trunc + "}", v.z);
        return line;
    }

    public static string ToS(Vector3 v)
    {
        return v.x + "," + v.y + "," + v.z;
    }
}
