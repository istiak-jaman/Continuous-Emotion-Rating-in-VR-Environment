using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// A singular quiz with a number of answers.
/// Any number of answers can be correct.
/// Place on a Quiz Canvas. It'll generate answers and such automatically.
/// </summary>
public class Quiz : MonoBehaviour
{
    // How the quiz responds to a correct/incorrect answer
    // Only determines how this object responds, excludes teacher resopnse.
    public enum QuizResponse { NONE, BACKGROUND }

    public bool trackHead = true;

    [Header("Fields")]
    [SerializeField] private string title;
    [SerializeField] private List<string> answers;
    [SerializeField] private List<string> messages; // If a message is associated with the quiz answers, it's placed here. Basically allows shorthand.
    [SerializeField] private List<int> correctAnswers;

    [Header("Links")]
    [SerializeField] private TextMeshProUGUI quizTitle;
    [SerializeField] private Transform quizAnswersParent;
    [SerializeField] private GameObject quizAnswerPrefab;

    [Header("Response Params")]
    [SerializeField] private QuizResponse responseType;
    [SerializeField] private float waitTime;
    [SerializeField] private Image background;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color wrongColor;

    private bool correct;

    // Start is called before the first frame update
    void Start()
    {
        SetUpQuiz();
    }

    // Update is called once per frame
    void Update()
    {
        // DEBUG answering method
        if (Input.GetKeyDown(KeyCode.Alpha1)) ReportFromAnswer(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ReportFromAnswer(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ReportFromAnswer(2);

        if (trackHead) {
            transform.position = Camera.main.transform.position + Camera.main.transform.forward * .5f;
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
        }
    }

    public void SetInfo(string t, List<string> a, List<string> m, List<int> c, QuizResponse r) {
        title = t; answers = a; correctAnswers = c; messages = m; responseType = r;
    }

    /// <summary>
    /// Sets title, adds all the buttons, and sets them up to respond with their answers.
    /// </summary>
    public void SetUpQuiz() {
        quizTitle.text = title;
        for (int i = 0; i < answers.Count; i++) {
            var ans = Instantiate(quizAnswerPrefab, quizAnswersParent);
            ans.GetComponent<QuizAnswer>().SetUpAnswer(this, answers[i], i);
        }
        // Can't have a response if there's no right answer.
        if (correctAnswers.Count == 0) responseType = QuizResponse.NONE;
    }

    public void ReportFromAnswer(int answerNum) {
        correct = correctAnswers.Contains(answerNum);
        QuizManager.instance.ReportQuizAnswer(title, answers[answerNum], messages[answerNum], correct);
        switch(responseType) {
            case QuizResponse.NONE:
                QuizManager.instance.ReportQuizFinished();
                break;
            case QuizResponse.BACKGROUND:
                StartCoroutine(HandleBackgroundResponse());
                break;
        }
    }

    // Change the background color based on the answer, wait a bit and tell manager
    IEnumerator HandleBackgroundResponse() {
        background.color = (correct) ? correctColor : wrongColor;
        yield return new WaitForSeconds(waitTime);
        QuizManager.instance.ReportQuizFinished();
    }
}
