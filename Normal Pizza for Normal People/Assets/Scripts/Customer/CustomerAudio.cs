using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAudio : MonoBehaviour
{
    private AudioSource audioSource = null;

    public enum CustomerAudioStates {Walking, GoodOrder, BadOrder, OrderEndingSoon, AtCounter, Stop}
    public CustomerAudioStates CurrentCustomerAudioState { get; private set; }
    
    [SerializeField] private List<AudioClip> customerAudioClips = new List<AudioClip>();
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Changes the audio state of the customer 
    /// </summary>
    /// <param name="state">The new audio state to change to</param>
    public void ChangeCustomerAudio(CustomerAudioStates state)
    {
        CurrentCustomerAudioState = state;
        
        if (CurrentCustomerAudioState == CustomerAudioStates.Stop)
            audioSource.Stop();
        else
            PlayCustomerAudio();
    }
    
    private void PlayCustomerAudio()
    {
        audioSource.clip = customerAudioClips[(int) CurrentCustomerAudioState];
        audioSource.loop = CurrentCustomerAudioState == CustomerAudioStates.Walking;
        audioSource.Play();
    }
}
