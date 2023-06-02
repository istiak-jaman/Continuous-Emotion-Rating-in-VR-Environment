using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A single quiz answer, part of a larger quiz.
/// </summary>
public class QuizAnswer : MonoBehaviour
{
    private string answer;
    private int answerNumber;
    [SerializeField] private UnityEngine.UI.Button button;
    [SerializeField] private TMPro.TextMeshProUGUI answerText;

    /// <summary>
    /// Sets up this answer with a title and a link to the parent quiz so it can 
    /// inform it if it's clicked.
    /// </summary>
    /// <param name="parentQuiz"></param>
    public void SetUpAnswer(Quiz parentQuiz, string ans, int ansNum) {
        answer = ans;
        answerText.text = ans;
        answerNumber = ansNum;
        // When clicked, should report to the parent quiz.
        button.onClick.AddListener(delegate { parentQuiz.ReportFromAnswer(ansNum); });
    }
}
