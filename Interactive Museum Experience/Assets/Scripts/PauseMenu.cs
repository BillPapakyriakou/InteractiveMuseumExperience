using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public  bool gameIsPaused = false;
    public static bool welcomeUIActive = false;

    public GameObject pauseMenuUI;
    public GameObject welcomeMenuUI;

    MusicManager musicManager;

    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();

        if (SceneManager.GetActiveScene().name.Equals("Game"))
        {
            musicManager.Initialize();
            Invoke("WelcomeMessage", .2f);
            
        }
        
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !FindObjectOfType<DialogueManager>().isDialogueShown)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        
        gameIsPaused = false;
        
        if (welcomeUIActive)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
        
        
        
        //AudioManager.instance.SetVolume(AudioManager.instance.masterVolumePercent * 2f, AudioManager.AudioChannel.Master);
        // above line initiates menu theme too and changes both volumes, may fix later
    }

    public void ExitWelcome()
    {
        welcomeUIActive = false;
        welcomeMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Cursor.lockState = CursorLockMode.None;
        //AudioManager.instance.SetVolume(AudioManager.instance.masterVolumePercent * 0.5f, AudioManager.AudioChannel.Master);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();   // works only on final build
    }

    public void WelcomeMessage()
    {
        welcomeUIActive = true;
        welcomeMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;

    }

}