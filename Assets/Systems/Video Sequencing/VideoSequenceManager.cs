using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Manages the video sequence by parsing video sequence files in real time.
/// </summary>
public class VideoSequenceManager : MonoBehaviour, IReceiveQuizUpdates
{
    [SerializeField] private string rootNodeName;
    [SerializeField] private VideoPlayer vidPlayer;
    [SerializeField] private VideoClip vid;
    [SerializeField] private List<float> pauseTimes;
    [SerializeField] private float pauseTimer;
    [SerializeField] private int quizIndex;
    [SerializeField] private bool paused = false;
    [SerializeField] private string nextNode;

    // On start open up the root node and parse it
    void Start()
    {
        QuizManager.instance.SubscribeToQuizUpdates(this);
        ParseNode("root");
    }

    private void Update() {
         if (quizIndex < pauseTimes.Count && !vidPlayer.isPaused) {
            pauseTimer += Time.deltaTime;

            if (pauseTimer >= pauseTimes[quizIndex]) {
                vidPlayer.Pause();
                quizIndex++;
                QuizManager.instance.ActivateCurrQuiz();
            }
        }
    }

    /// <summary>
    /// Every node offers a single video and a sequence of quizzes.
    /// </summary>
    public void ParseNode(string name) {
        QuizManager.instance.ResetQuizzes();
        pauseTimer = 0;
        quizIndex = 0;
        pauseTimes.Clear();

        string node = Resources.Load<TextAsset>("Sequences/" + name).text;
        string[] lines = System.Text.RegularExpressions.Regex.Split(node, "\n|\r|\r\n");
        
        foreach (string line in lines) {
            // Parse each line
            string[] tokens = line.Split('|');
            switch (tokens[0]) {
                case "node":
                    // currently no use for node title
                    break;
                case "video":
                    // Video is simple, second arg is video name
                    vid = Resources.Load<VideoClip>("Videos/" + tokens[1]);
                    vidPlayer.clip = vid;
                    if (vid == null)
                        Debug.LogWarning("Video not found, did you download the vids from my OneDrive?");
                    break;
                case "quiz":
                    // Quiz is more complex. Arg 1: how far in for the video does the quiz pause.
                    // Arg 2: Quiz title
                    // Arg 3: Number of answers
                    // Rest: sets of 3: 4-correct or not, 5-answer text, 6-video to go to (and quiz message)
                    string title = tokens[2]; 
                    int numAnswers = int.Parse(tokens[3]);
                    List<string> answers = new List<string>();
                    List<int> correct = new List<int>();
                    List<string> messages = new List<string>();

                    for (int i = 0; i < numAnswers*3; i+=3) {
                        answers.Add(tokens[5 + i]);
                        messages.Add(tokens[6 + i]);
                        int cor = int.Parse(tokens[4 + i]);
                        if (cor == 1) correct.Add(i / 3);
                    }

                    QuizManager.instance.AddQuiz(title, answers, messages, correct, 1);
                    Debug.Log(tokens[1]);
                    // Two options for time in video to pause, in seconds or in percent
                    float pauseTime = 100;
                    if (tokens[1].EndsWith("%")) {
                        float per = float.Parse(tokens[1].Replace("%", ""));
                        pauseTime = Mathf.Lerp(0, (float)vid.length, per / 100);
                    } 
                    else if (tokens[1].EndsWith("s")) {
                        pauseTime = float.Parse(tokens[1].Replace("s", ""));
                        if (pauseTime >= vid.length) pauseTime = (float)vidPlayer.length;
                    }
                    Debug.Log(pauseTime);
                    pauseTimes.Add(pauseTime);
                    break;
            }
        }

        vidPlayer.Play();
    }

    public void ReceiveQuizAnswer(string question, string answer, string message, bool correct) {
       print("Received " + message);
        nextNode = message;


       
    }

    public void ReceiveQuizFinished() {
        // If the quiz is finished but it's not the last one, unpause the video
        vidPlayer.Play();
    }


    public void ReceiveQuizSequenceFinished() {
        // When the last quiz is finished, we reset and set up the next node.
        vidPlayer.Pause();
        ParseNode(nextNode);
    }
}
