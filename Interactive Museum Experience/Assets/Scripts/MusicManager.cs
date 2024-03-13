using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static AudioManager;

public class MusicManager : MonoBehaviour
{
    public AudioClip mainTheme;
    public AudioClip menuTheme;

    string sceneName;

    void Start()
    {
  
        sceneName = SceneManager.GetActiveScene().name;

        SceneManager.sceneLoaded += OnSceneLoaded;

        Invoke("PlayMusic", .2f);  // music starts with crossfade

    }

    public void Initialize()
    {
        Start();
    }


    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = scene.name;

        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", 0.2f);   // AudioManager dublicate is destroyed
                                         // this method would be called before obj destruction
                                         // so playmusic would be called twice if not for the Invoke .2 delay 
                                         // fade in would not work
        }
    }


    void PlayMusic()
    {
        AudioClip clipToPlay = null;

        if (sceneName == "Menu")
        {
            clipToPlay = menuTheme;
        }
        else if (sceneName == "Game")
        {
             clipToPlay = mainTheme;
        }


        if (clipToPlay != null)
        {
            AudioManager.instance.PlayMusic(clipToPlay, 2);  // gets static instance of AudioManager

            Invoke("PlayMusic", clipToPlay.length);  // replay music clip for when clip is finished
        }
    }

}
