using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizAnswers : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct answer");
            quizManager.Correct();
        }
        else
        {
            Debug.Log("Wrong answer");
            quizManager.Wrong();
        }
    }
}
