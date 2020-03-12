using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource audioSource;
    [SerializeField]
    private List<AudioClip> musicTracks = new List<AudioClip>(3);
    public enum MusicTrackName { MainMenuMusic, WorkDayMusic, BetweenDaysMusic }
    

    private void Awake()
    {
        if (gameObject.TryGetComponent(out MusicManager musicManager))
        {
            instance = musicManager;
        }
        else
        {
            gameObject.AddComponent<MusicManager>();
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            ChangeMusic(MusicTrackName.MainMenuMusic);
        }
    }

    public void ChangeMusic(MusicTrackName musicTrack)
    {
        audioSource.Stop();
        switch (musicTrack)
        {
            case MusicTrackName.MainMenuMusic: 
                audioSource.clip = musicTracks[(int) MusicTrackName.MainMenuMusic];
                break;
            case MusicTrackName.WorkDayMusic:
                audioSource.clip = musicTracks[(int) MusicTrackName.WorkDayMusic];
                break;
            case MusicTrackName.BetweenDaysMusic:
                audioSource.clip = musicTracks[(int) MusicTrackName.BetweenDaysMusic];
                break;
        }
        
        print("Music track input: " + musicTrack + " Music track output: " + audioSource.clip);
        
        audioSource.Play();
    }
}
