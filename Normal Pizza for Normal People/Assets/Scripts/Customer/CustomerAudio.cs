using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerAudio : MonoBehaviour
{
    private AudioSource audioSource = null;

    public enum CustomerAudioStates {Stop, Walking, GoodOrder, BadOrder, OrderEndingSoon, AtCounter}
    public CustomerAudioStates currentCustomerAudioState { get; private set; }
    
    [SerializeField] private List<AudioClip> customerAudioClips = new List<AudioClip>();
    
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ChangeCustomerAudio(CustomerAudioStates state)
    {
        currentCustomerAudioState = state;
        
        if (currentCustomerAudioState == CustomerAudioStates.Stop)
            audioSource.Stop();
        else
            PlayCustomerAudio();
    }
    
    private void PlayCustomerAudio()
    {
        audioSource.clip = customerAudioClips[(int) currentCustomerAudioState];
        audioSource.loop = currentCustomerAudioState == CustomerAudioStates.Walking;
        audioSource.Play();
    }
}
