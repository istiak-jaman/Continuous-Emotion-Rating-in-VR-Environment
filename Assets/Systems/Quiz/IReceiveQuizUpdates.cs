using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic interface for something that wants to receive udpates when a quiz question
///  is answerd or a quiz is finished.
/// Examples include the networked quiz stuff, an answer recorder, or some greater sequence manager.
/// </summary>
public interface IReceiveQuizUpdates
{
    void ReceiveQuizAnswer(string question, string answer, string message, bool correct);
    void ReceiveQuizFinished();
    void ReceiveQuizSequenceFinished();
}
