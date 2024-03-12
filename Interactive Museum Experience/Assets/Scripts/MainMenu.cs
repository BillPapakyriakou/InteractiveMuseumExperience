using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Slider[] volumeSliders;

    void Start()
    {
        //volumeSliders = new Slider[1];

        volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
        volumeSliders[1].value = AudioManager.instance.musicVolumePercent;

    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   // load next scene in queue
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();   // works only on final build
    }
    public void SetMasterVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }
}
