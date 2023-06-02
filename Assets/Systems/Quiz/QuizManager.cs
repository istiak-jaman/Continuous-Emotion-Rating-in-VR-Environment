using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Manages quiz sequences.
/// Quiz sequence is set automatically by the order of children.
/// Quiz objects report to this when an answer is selected and when they finish.
/// </summary>
public class QuizManager : MonoBehaviour
{
    [SerializeField] private GameObject quizPrefab;
    [Tooltip("Where we're at in the sequence.")]
    [SerializeField] private int sequenceIndex;
    [SerializeField] private List<Quiz> quizSequence;
    [Tooltip("Demo mode doesn't wait for another object to activate quizzes")]
    [SerializeField] private bool demoMode;

    [SerializeField] List<IReceiveQuizUpdates> quizObservers = new List<IReceiveQuizUpdates>();

    public static QuizManager instance;

    private void Awake() {
        if (instance) Destroy(this);
        else instance = this;
        quizObservers = new List<IReceiveQuizUpdates>();
    }

    // Start is called before the first frame update
    void Start()
    {
        sequenceIndex = 0;
        quizSequence = new List<Quiz>(GetComponentsInChildren<Quiz>());
        //quizSequence = new List<Quiz>();
        foreach (var quiz in quizSequence) quiz.gameObject.SetActive(false);

        if (demoMode) ActivateCurrQuiz();
    }

    /// <summary>
    /// Turns on the quiz at the current point in the sequence.
    /// </summary>
    public void ActivateCurrQuiz() {
        if (sequenceIndex < quizSequence.Count)
            quizSequence[sequenceIndex].gameObject.SetActive(true);
    }

    bool hidden = false;
    public void HideQuiz() {
        if (sequenceIndex < quizSequence.Count && quizSequence[sequenceIndex].gameObject.activeSelf && !hidden) {
            quizSequence[sequenceIndex].gameObject.SetActive(false);
            hidden = true;
        }
    }
    public void ShowQuiz() {
        if (sequenceIndex < quizSequence.Count && !quizSequence[sequenceIndex].gameObject.activeSelf && hidden) {
            quizSequence[sequenceIndex].gameObject.SetActive(true);
            hidden = false;
        }
    }

    /// <summary>
    /// Turns the quiz on or off. Useful for if you need to see something behind it.
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleCurrQuiz(bool toggle) {
        quizSequence[sequenceIndex].gameObject.SetActive(toggle);
    }

    /// <summary>
    /// Report that the current quiz just gave an answer.
    /// Used to record answers and respond however desired.
    /// </summary>
    /// <param name="answer"></param>
    /// <param name="correct"></param>
    public void ReportQuizAnswer(string question, string answer, string message, bool correct) {
        foreach (var observer in quizObservers) {
           
            observer.ReceiveQuizAnswer(question, answer, message, correct);
        }
    }

    /// <summary>
    /// Signals to the manager that the quiz is finished and ready to be turned off.
    /// Happens after any correct/incorrect response thing.
    /// Also advances sequence.
    /// </summary>
    public void ReportQuizFinished() {
        quizSequence[sequenceIndex].gameObject.SetActive(false);
        sequenceIndex++;
        if (demoMode) ActivateCurrQuiz();

        foreach (var observer in quizObservers) observer.ReceiveQuizFinished();
        if (sequenceIndex >= quizSequence.Count)
            foreach (var observer in quizObservers) observer.ReceiveQuizSequenceFinished();
    }

    public void SubscribeToQuizUpdates(IReceiveQuizUpdates observer) {
        quizObservers.Add(observer);
    }

    /// <summary>
    /// Adds a quiz to the manager. Spawns the quiz object and sets it to inactive for now too.
    /// </summary>
    /// <param name="title"></param>
    /// <param name="answers"></param>
    /// <param name="messages">A message the quiz gives back when the answer is selected.</param>
    /// <param name="correct"></param>
    /// <param name="response"></param>
    public void AddQuiz(string title, List<string> answers, List<string> messages, List<int> correct, int response) {
        Quiz quiz = Instantiate(quizPrefab, transform).GetComponent<Quiz>();
        quiz.SetInfo(title, answers, messages, correct, (Quiz.QuizResponse)response);
        quizSequence.Add(quiz);
        quiz.gameObject.SetActive(false);
    }

    public void ResetQuizzes() {
        foreach (Quiz q in quizSequence) {
            Destroy(q.gameObject);
        }
        sequenceIndex = 0;
        quizSequence.Clear();
    }
}
