using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using ViveSR.anipal.Eye;
using System.Runtime.InteropServices;
using System.Linq;

/// <summary>
/// A logger that wants to log everything at 120 Hz
/// </summary>
public class EyeRecorder : MonoBehaviour
{
    public static EyeRecorder instance;

    //public static ANTLogger ant;

    // Eye track stuff
    public static EyeData_v2 eyeData = new EyeData_v2();
    public bool eye_callback_registered = false;



    [Header("File Info - logPath/activity/user-time-dataType.csv")]
    public string logPath = "logs";
    public string activity;
    public string username ="Omi";
    public string dataType;

    public string logPath1 = "eye position";
    //public 

    [Header("Write Info")]
    [SerializeField]
    protected float writeInterval;
    private float writeTimer = 0;
    [SerializeField]
    [Tooltip("If true, calls LogLine every FixedUpdate. If false, call it yourself.")]
    protected bool writeConstant = true;

    [SerializeField]
    private static bool isRecording;
    private static float logStartTime;
    private static float time;
    public static List<string> lines = new List<string>();
    public static List<string> lines1 = new List<string>();
    private string filePath;
    private string filePath1;
    //private string filePathTimeStamp;

    //private string line_timestamp;

    //time stamp

    //private bool isTimestampLogged = false;

    private static long time_logstart;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }


    void start()
    {
        /*using (var fs = new FileStream("logs/timestamp.csv", FileMode.Create))
        using (var sw = new StreamWriter(fs))
        {
            sw.Write("User, Video/Task, Timestamp, Start or Stop");
            sw.WriteLine("\n");
        }*/

        //StartLogging();
        
    }


    // Update is called once per frame
    void Update()
    {
        // Register callback
        if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING && SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

        if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
        {
            SRanipal_Eye_v2.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = true;
            StartLogging();
        }

        // Write lines
        if (isRecording)
        {
            writeTimer += Time.deltaTime;
            if (writeTimer > writeInterval)
            {
                WriteLines();
                writeTimer = 0;
            }
        }
    }

    private void FixedUpdate()
    {
        time = Time.time;
    }


    protected void WriteHeader(StreamWriter file)
    {
        // Assume eye stuff will always be used
        file.Write("Unix Time, Vive Time,Left Open,Right Open,Left Pupil,Right Pupil,Left Pos,Right Pos,Eye Position,Left Dir.x,Left Dir.y,Left Dir.z,Right Dir.x,Right Dir.y,Right Dir.z,Left Frown,Right Frown,Left Squeeze,Right Squeeze,Left Wide,Right Wide");
        /*if (LobbyManager.instance.useANT)
            file.Write(",HR");
        if (LobbyManager.instance.useEmoBit)
            file.Write(",EDA,PPG:RED,PPG:IR,PPG:GREEN,HR,SCR:AMP,SCR:FREQ,SCR:RIS,THERM,TEMP1");*/
        // No current way to do poses.
        file.Write("\n");
    }
    protected void WriteHeader1(StreamWriter file)
    {
        // Assume eye stuff will always be used
        file.Write("Unix Time, Vive Time,Left Pos x, Left Pos y, Right Pos x, Right Pos y, Eye Position");
        /*if (LobbyManager.instance.useANT)
            file.Write(",HR");
        if (LobbyManager.instance.useEmoBit)
            file.Write(",EDA,PPG:RED,PPG:IR,PPG:GREEN,HR,SCR:AMP,SCR:FREQ,SCR:RIS,THERM,TEMP1");*/
        // No current way to do poses.
        file.Write("\n");
    }

    // ------ Logging Funcs ----------
    /*    public void StartLogging(string activityName)
        {
            activity = activityName;
            StartLogging();
        }*/

    /// <summary>
    /// Start logging with the activity name as already set
    /// </summary>
    public void StartLogging()
    {

        isRecording = true;
        logStartTime = Time.time;

        // Make sure the directory exists
        System.IO.Directory.CreateDirectory(logPath + @"\" + activity + @"\");

        System.IO.Directory.CreateDirectory(logPath1 + @"\" + activity + @"\");


        // Write and clear the file
        string time = DateTime.Now.Month.ToString() + "-" + DateTime.Now.Day.ToString() +
            " " + DateTime.Now.Hour.ToString() + "-" + DateTime.Now.Minute.ToString();
        filePath = logPath + @"\" + activity + @"\" + username + " " + time + " " +
            dataType + ".csv";

        filePath1 = logPath1 + @"\" + activity + @"\" + username + " " + time + " " +
            dataType + ".csv";

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, false))
        {
            WriteHeader(file);
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath1, false))
        {
            WriteHeader1(file);
        }

        //log timestamp for each task/video

        //isTimestampLogged = true;

        //// Make sure the directory exists
        //System.IO.Directory.CreateDirectory(logPath + @"\" + "Timestamp" + @"\");
        //filePathTimeStamp = logPath + @"\" + "Timestamp" + @"\" + "task_video_start_time_stamp"  + ".csv";

        //using (System.IO.StreamWriter fileTimestamp = new System.IO.StreamWriter(filePathTimeStamp, false))
        //{
        //    WriteHeaderTimestamp(fileTimestamp);
        //}

        /*      long time_microsecond = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds * 1000.0);

              time_logstart = time_microsecond;

              Debug.Log("StartLogging from: " + activity + " For User " + username + "  Time: " + time_microsecond.ToString());

              using (var fs = new FileStream("logs/timestamp.csv", FileMode.Append))
              using (var sw = new StreamWriter(fs))
              {
                  string line = username + "," + activity + "," + time_microsecond.ToString() + "," + "start";
                  sw.WriteLine(line);

                  // sw.Close();
              }*/

    }

  /*  protected void WriteHeaderTimestamp(StreamWriter file)
    {
        file.Write("User, TaskVideo, Start Timestamp");
        file.Write("\n");
    }*/

    public void StopLogging()
    {
        isRecording = false;
        WriteLines();
        writeTimer = 0;

        //isTimestampLogged = false;

       /* var time_microsecond = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds * 1000.0);

        Debug.Log("StopLogging from: " + activity + " For User " + username + "  Time: " + time_microsecond.ToString());

        using (var fs = new FileStream("logs/timestamp.csv", FileMode.Append))
        using (var sw = new StreamWriter(fs))
        {
            string line = username + "," + activity + "," + time_microsecond.ToString() + "," + "stop";
            sw.WriteLine(line);
            //sw.Close();
        }*/
    }

   /* public void SetActivity(string a)
    {
        activity = a;
    }*/

    public bool GetIsRecording()
    {
        return isRecording;
    }

    private void WriteLines()
    {
        // Make sure there are lines in the first place
        if (lines.Count == 0) return;

        // Write all lines to file
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
        {
            foreach (string line in lines.ToList())
                file.WriteLine(line);
            lines.Clear();
            file.Close();
        }


        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath1, true))
        {
            foreach (string line in lines1.ToList())
                file.WriteLine(line);
            lines1.Clear();
            file.Close();
        }

        //for timestamp
        //if (isTimestampLogged == true)
        //{
        //    var time_nanosecond = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds * 1000000.0);

        //    line_timestamp = username + "," + activity + "," + time_nanosecond;


        //    using (System.IO.StreamWriter fileTimestamp = new System.IO.StreamWriter(filePathTimeStamp, true))
        //    {

        //        fileTimestamp.WriteLine(line_timestamp);

        //        line_timestamp = "";

        //        fileTimestamp.Close();
        //    }

        //    isTimestampLogged = false;
        //}

    }

    // This is where all the logging happens
    private void EyeCallback(ref EyeData_v2 eye_data)
    {

        if (isRecording)
        {
            EyeData_v2 ed = eye_data;
            string eyePosition = "OK";

            //var time_nanosecond =  (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds * 1000000.0);

            long time_microsecond = (long)((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds * 1000.0);

            Debug.Log("Time offset from log start to record: " + (time_microsecond - time_logstart).ToString());

            //string line = (time - logStartTime) + "," +

            if ((ed.verbose_data.left.pupil_position_in_sensor_area.x >= .43 && ed.verbose_data.left.pupil_position_in_sensor_area.y <= .65)
                    || (ed.verbose_data.right.pupil_position_in_sensor_area.x >= .45 && ed.verbose_data.right.pupil_position_in_sensor_area.y <= .70))
            {
                    eyePosition = "Rating Interface";
            }
           


            string line = time_microsecond.ToString() + "," +
                ed.timestamp + "," +
                ed.verbose_data.left.eye_openness.ToString() + "," +
                ed.verbose_data.right.eye_openness.ToString() + "," +
                ed.verbose_data.left.pupil_diameter_mm.ToString() + "," +
                ed.verbose_data.right.pupil_diameter_mm.ToString() + "," +
                ed.verbose_data.left.pupil_position_in_sensor_area.x.ToString() + "|" + ed.verbose_data.left.pupil_position_in_sensor_area.y.ToString() + "," +
                ed.verbose_data.right.pupil_position_in_sensor_area.x.ToString() + "|" + ed.verbose_data.right.pupil_position_in_sensor_area.y.ToString() + "," +
                eyePosition + "," +
                RecordingUtils.ToS(ed.verbose_data.left.gaze_direction_normalized) + "," +
                RecordingUtils.ToS(ed.verbose_data.right.gaze_direction_normalized) + "," +
                ed.expression_data.left.eye_frown.ToString() + "," +
                ed.expression_data.right.eye_frown.ToString() + "," +
                ed.expression_data.left.eye_squeeze.ToString() + "," +
                ed.expression_data.right.eye_squeeze.ToString() + "," +
                ed.expression_data.left.eye_wide.ToString() + "," +
                ed.expression_data.right.eye_wide.ToString();



            string line1 = time_microsecond.ToString() + "," +
                ed.timestamp + "," +
                ed.verbose_data.left.pupil_position_in_sensor_area.x + "," +
                ed.verbose_data.left.pupil_position_in_sensor_area.y + "," +
                ed.verbose_data.right.pupil_position_in_sensor_area.x + "," +
                ed.verbose_data.right.pupil_position_in_sensor_area.y + "," +
                eyePosition;

            //print(ed.verbose_data.left.eye_openness.ToString());
            //Console.WriteLine(ed.verbose_data.left.eye_openness);

            /*f (LobbyManager.instance.useANT)
                line += "," + ant.currHR;

            if (LobbyManager.instance.useEmoBit)
                line += "," + EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.EDA] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.PPGR] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.PPGIR] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.PPGG] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.HR] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.SCRA] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.SCRF] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.SCRR] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.THERM] + "," +
                EmotiBitRelay.instance._currValues[EmotiBitRelay.EBDataTypes.TEMP];
*/
            lines.Add(line);
            lines1.Add(line1);
        }
    }

    private void Release()
    {
        if (eye_callback_registered == true)
        {
            SRanipal_Eye_v2.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye_v2.CallbackBasic)EyeCallback));
            eye_callback_registered = false;
        }
    }
}
