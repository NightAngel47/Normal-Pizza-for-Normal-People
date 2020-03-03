using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Old_Scripts
{
    public class TimeLeftUI : MonoBehaviour
    {
        private GameManager gm;
    
        [SerializeField, Tooltip("Time Fill Bar UI")]
        private Image fillBar;
        /// <summary>
        /// Total time left on the order
        /// </summary>
        [HideInInspector]
        public float totalOrderTime;
        /// <summary>
        /// Current time left on the order
        /// </summary>
        [HideInInspector]
        public float currentTimeLeft;

        private AudioSource audioSource;
        [SerializeField]
        private AudioClip[] audioClips;

        void Start()
        {
            gm = FindObjectOfType<GameManager>();
            fillBar.fillAmount = 1;
            audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (gm.isPaused) return;
            currentTimeLeft -= Time.deltaTime;
            fillBar.fillAmount = currentTimeLeft / totalOrderTime;

            if(Math.Abs(currentTimeLeft / totalOrderTime - 0.25f) < 0.01f) // just to work with floating point
            {
                audioSource.Stop();
                audioSource.clip = audioClips[0];
                audioSource.loop = false;
                audioSource.Play();
            }

            if(Math.Abs(currentTimeLeft / totalOrderTime - 0.01f) < 0.01f)
            {
                audioSource.Stop();
                audioSource.clip = audioClips[1];
                audioSource.loop = false;
                audioSource.Play();
            }
        }

        /// <summary>
        /// Sets total order time and starts timer
        /// </summary>
        /// <param name="orderTime">The total time for the order in seconds</param>
        public void SetTime(float orderTime)
        {
            currentTimeLeft = totalOrderTime = orderTime;
        }
    }
}
