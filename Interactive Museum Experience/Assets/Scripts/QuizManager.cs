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

    private List<int> usedQuestionIndices = new List<int>();  //Track used question indices

    void Start()
    {
        totalQuestions = 5;  // set number of questions for the quiz
        totalQuestionsFixed = totalQuestions;        
    }

    void Update()  // review later
    {
        if (Input.GetKeyUp(KeyCode.Q) && !FindObjectOfType<DialogueManager>().isDialogueShown && !FindObjectOfType<Puzzle>().puzzleIsActive && !quizIsActive)
        {
            InitiateQuiz();
            
            
        }
        if (Input.GetKeyUp(KeyCode.Escape) && quizIsActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
            QuizReset();
            ExitQuiz();
        }
    }

    public void Correct()
    {
        score += 1;
        
        RemoveCurrentQuestion();
    }

    public void Wrong()
    {
        RemoveCurrentQuestion();
    }

    void RemoveCurrentQuestion()
    {
        usedQuestionIndices.Add(currentQuestionIndex); // Track used question index
        if (assets.Count > 0)
        {
            GenerateQuestion();
        }
        else
        {
            QuizOver();  // No more questions, end quiz
        }
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
            // Find a random question index that hasn't been used
            do
            {
                //Debug.Log("pick random");
                currentQuestionIndex = Random.Range(0, assets.Count);
            } 
            while (usedQuestionIndices.Contains(currentQuestionIndex));


            QuestionText.text = assets[currentQuestionIndex].Question;
            SetAnswers();

            totalQuestions--;
        }
        else
        {
            //Debug.Log("Out of questions");
            QuizOver();
        }

    }

    public void QuizReset()
    {
        totalQuestions = totalQuestionsFixed;
        score = 0;
        usedQuestionIndices.Clear(); // Clear used question indices
        
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
        GenerateQuestion();

    }

    public void QuizOver()
    {
        quizUI.SetActive(false);
        quizOverUI.SetActive(true);
        ScoreText.text = "Your quiz score was " + score + "/" + totalQuestionsFixed;
        QuizReset();
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
