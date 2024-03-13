using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, Sfx, Music};

    public float masterVolumePercent { get; private set; } = .2f;
    public float sfxVolumePercent { get; private set; } = 1f;
    public float musicVolumePercent { get; private set; } = .2f;

    AudioSource[] musicSources;

    int activeMusicSourceIndex;

    public static AudioManager instance;

    void Awake()
    {

        if (instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source" + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;  // parent newMusicSource to AudioManager game object

            }
            masterVolumePercent = PlayerPrefs.GetFloat("master vol", masterVolumePercent);  // remember settings
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", musicVolumePercent);  // remember settings
        }
        
    }

    public void SetVolume(float volumePercent, AudioChannel channel)
    {
        
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        // Debug.Log("That works");

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        musicSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);  // remember settings
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);  // remember settings
        PlayerPrefs.Save();
    }


    public void PlayMusic(AudioClip clip, float fadeDuration = 1)
    {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;  // new index = 1 - old index : follows 0-1-0-1 pattern
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play();


        StartCoroutine(AnimateMusicCrossfade(fadeDuration));

    }

    public void PlaySound(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
        }
    }

    IEnumerator AnimateMusicCrossfade(float duration)
    {
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * (1 / duration);   // percent = deltaTime * crossFade speed
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);  // for active music cource
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0,  percent);  // for non active music source
            yield return null;
        }
    }
}
