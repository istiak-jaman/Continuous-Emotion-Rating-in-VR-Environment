using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using UnityEngine.UI;

public class PauseQuiz : MonoBehaviour
{

    [Header("Video Stuff")]
    public VideoPlayer vidPlayer;
    
    [SerializeField] private GameObject canvas;
    public Transform cam;
    public long frame;
    public GameObject empty;
    public long currFrame;
    public bool oneTime = false;

    


    void Start()
    {
        vidPlayer = GetComponent <VideoPlayer>();
        vidPlayer.url = GetComponent<QuizParse>().URL;
        vidPlayer.loopPointReached += Pause;
    }


    void Update()
    {
        
        //When we get to the specified frame, pause and load quiz.
        currFrame = vidPlayer.frame;
        //if(!oneTime && currFrame >= (long)vidPlayer.frameCount)
        //if(!oneTime && vidPlayer.)
        //{
        //    oneTime = true;
        //    vidPlayer.Pause();
        //    GetComponent<QuizParse>().MakeQuiz();

            

        //}

    }

    public void Pause(VideoPlayer source) {
        oneTime = true;
        vidPlayer.Pause();
        GetComponent<QuizParse>().MakeQuiz();
    }


}
