using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuizAssets> assets;
    public GameObject[] options;
    public int currentQuestionIndex;

    public TMP_Text QuestionText;
    public TMP_Text ScoreText;

    public int score;
    int totalQuestions = 0;  // this is going to get decreased 
    int totalQuestionsFixed = 0;  // this is for the game over text
     
    public GameObject quizPromptUI;
    public GameObject quizUI;
    public GameObject quizOverUI;

    public bool quizIsActive = false;

    CameraController controller;

    void Start()
    {
        totalQuestions = 2;  // set number of questions for the quiz
        totalQuestionsFixed = totalQuestions;
        GenerateQuestion();
    }

    void Update()  // review later
    {
        if (Input.GetKeyUp(KeyCode.Q) && !FindObjectOfType<DialogueManager>().isDialogueShown)
        {
            InitiateQuiz();
            
        }
    }

    public void Correct()
    {
        score += 1;
        assets.RemoveAt(currentQuestionIndex);  // dont repeat question
        GenerateQuestion();
    }

    public void Wrong()
    {
        assets.RemoveAt(currentQuestionIndex);  // dont repeat question
        GenerateQuestion();
        
    }

    void SetAnswers()
    {
        // get text component from button object and set the text to the answer from the answers list
        for (int i = 0; i < options.Length; i++) 
        {
            options[i].GetComponent<QuizAnswers>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = assets[currentQuestionIndex].Answers[i];

            if (assets[currentQuestionIndex].CorrentAnswerIndex == i+1) 
            {
                options[i].GetComponent<QuizAnswers>().isCorrect = true;    
            }
        }
    }

    void GenerateQuestion()
    {
        if (assets.Count > 0 && totalQuestions > 0)
        {
            currentQuestionIndex = Random.Range(0, assets.Count);

            QuestionText.text = assets[currentQuestionIndex].Question;
            SetAnswers();

            totalQuestions--;
        }
        else
        {
            Debug.Log("Out of questions");
            QuizOver();
        }

    }

    public void InitiateQuiz()  // quiz minigame prompt
    {
        Cursor.lockState = CursorLockMode.None;

        controller = FindObjectOfType<CameraController>();
        controller.DisableCameraMovement();
        quizPromptUI.SetActive(true);
        quizIsActive = true;

    }

    public void StartQuiz()  // quiz minigame start
    {
        
        quizPromptUI.SetActive(false);
        quizUI.SetActive(true);
        
    }

    public void QuizOver()
    {
        quizUI.SetActive(false);
        quizOverUI.SetActive(true);
        ScoreText.text = "Your quiz score was " + score + "/" + totalQuestionsFixed;
        
    }

    public void ExitQuiz()  // quiz minigame end 
    {
        Cursor.lockState = CursorLockMode.Locked;
        quizPromptUI.SetActive(false);
        quizUI.SetActive(false);
        quizOverUI.SetActive(false);
        quizIsActive = false;
        controller.ToggleMovement();
    }
}
