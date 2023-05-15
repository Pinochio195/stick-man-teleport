using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip

        Click,
        Done,
        Tap1,
        Tap2,
        ShowPopup,
        MusicWin,
        Fall;

    public AudioClip[] SoundMove;
    public int indexSoundMove = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        AS_SOUND.volume = PlayerPrefs.GetInt("Music", 1);
        AS_MUSIC.volume = PlayerPrefs.GetInt("Music", 1);
    }

    public AudioSource AS_SOUND, AS_MUSIC;

    public AudioClip SoundMovePuzzle()
    {
        AudioClip clip = SoundMove[indexSoundMove];
        indexSoundMove++;
        if (indexSoundMove >= SoundMove.Length) indexSoundMove = 0;
        return clip;
    }

    public void PlaySound(AudioClip clip, float volume = 1)
    {
        AS_SOUND.volume = PlayerPrefs.GetInt("Music", 1);
        AS_SOUND.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip, float volume = 1)
    {
        AS_MUSIC.Stop();
        AS_MUSIC.volume = PlayerPrefs.GetInt("Music", 1);
        AS_MUSIC.clip = clip;
        AS_MUSIC.Play();
        AS_MUSIC.loop = true;
    }
}