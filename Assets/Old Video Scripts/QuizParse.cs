using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class QuizParse : MonoBehaviour
{

    [Header("File Stuff")]
    private string path = "recs";


    [Header("Quiz Stuff")]
    [SerializeField] private TMPro.TextMeshProUGUI answer;
    [SerializeField] private TMPro.TextMeshProUGUI answer2;
    [SerializeField] private TMPro.TextMeshProUGUI answer3;
    [SerializeField] private TMPro.TextMeshProUGUI title;
    [SerializeField] private GameObject canvasPrefab;


    List<string> quizAnswers = new List<string>();
    List<string> correctAnswers = new List<string>();
    List<string> nextSeqFile = new List<string>();

    private string quizTitle = "";
    public string URL = "";
    private GameObject c;


    void Start()
    {

        
    }

    void Update()
    {

    }
    void Awake()
    {
        List<string> seqString = GetFiles();

        ParseSeq(seqString[0]);
    }


    //<summary>
    //
    //Parse through each file and take all file info.
    //Look for initial indications in each line to gather separate data such as title, quiz info, and next video clip.
    //
    //<summary>
    public void ParseSeq(string fileName)
    {
        nextSeqFile.Clear(); //Clears the list so that each answer contains the correct file information.
        quizAnswers.Clear(); //Clears the list so that each answer is displayed correctly on the quiz.

        List<string> fileLines = new List<string>();
        //Debug.Log(fileName);
        using (StreamReader reader = new StreamReader(File.Open("recs/" + fileName + ".seq", FileMode.Open)))
        {
            while (!reader.EndOfStream)
                fileLines.Add(reader.ReadLine());

        }

        foreach (string line in fileLines)
        {

            string[] tokens = line.Split('|');

            //for (int i = 0; i < tokens.Length; i++)
            //{
            //    Debug.Log(tokens[i] + "-------" + "value of i: " + i);
            //}
            string nodeTitle = "";
            int numAnswers;

            switch (tokens[0])
            {
                case "node":
                    nodeTitle = tokens[1];
                    //Debug.Log(nodeTitle);
                    break;
                case "quiz":
                    //Format: quiz|location|branchOrNot|title|numOfAnswers|CorrectOrNot|QuizAnswer|nextSeqFile|etc...|
                    quizTitle = tokens[1];
                    numAnswers = int.Parse(tokens[2]);

                    for (int i = 3; i <= numAnswers * 3; i += 3)
                    {
                        int correct = int.Parse(tokens[i]);
                        if (correct == 1)
                        {
                            correctAnswers.Add(tokens[i + 1]);
                        }
                        quizAnswers.Add(tokens[i + 1]);
                        nextSeqFile.Add(tokens[i + 2]);
                        //print(tokens[i]);
                        //print(tokens[i + 1]);
                        //print(tokens[i + 2]);
                    }

                    break;

                case "video":
                    URL = tokens[1];


                    break;
            }
        }
    }

    public void MakeQuiz()
    {
        answer.text = quizAnswers[0];
        answer2.text = quizAnswers[1];
        answer3.text = quizAnswers[2];
        title.text = quizTitle;
        c = Instantiate(canvasPrefab, GetComponent<PauseQuiz>().empty.transform);
        c.SetActive(true);
        

    }


    //<summary>
    //
    //Will read in all files that end with .seq and store them for use.
    //
    //<summary>
    public List<string> GetFiles()
    {
        List<string> seqs = new List<string>();
        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            if (file.EndsWith(".seq"))
            {
                seqs.Add(Path.GetFileNameWithoutExtension(file));
            }
        }
        return seqs;
    }

    public void ChangeC()
    {
        //Load next file on selection and play the video contained within.
        ParseSeq(nextSeqFile[0]);
        c.SetActive(false);
        GetComponent<PauseQuiz>().vidPlayer.url = URL;
        GetComponent<PauseQuiz>().vidPlayer.Play();
        GetComponent<PauseQuiz>().oneTime = false;
    }
    public void ChangeW()
    {
        //Load next file on selection and play the video contained within.
        ParseSeq(nextSeqFile[1]);
        c.SetActive(false);
        GetComponent<PauseQuiz>().vidPlayer.url = URL;
        GetComponent<PauseQuiz>().vidPlayer.Play();
        GetComponent<PauseQuiz>().oneTime = false;
    }
    public void ChangeM()
    {
        //Load next file on selection and play the video contained within.
        ParseSeq(nextSeqFile[2]);
        c.SetActive(false);
        GetComponent<PauseQuiz>().vidPlayer.url = URL;
        GetComponent<PauseQuiz>().vidPlayer.Play();
        GetComponent<PauseQuiz>().oneTime = false;
    }
}
