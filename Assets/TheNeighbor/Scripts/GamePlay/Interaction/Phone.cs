using System;
using System.Collections;
using System.Collections.Generic;
using Trellcko.Gameplay.QuestLogic;
using UnityEngine;

namespace Trellcko.Gameplay.Interactable
{
    public class Phone : MonoBehaviour    {
        
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _ringClip;
        [SerializeField] private AudioClip _hangUpPhoneClip;
        [SerializeField] private AudioClip _hangDownPhoneClip;
        

        public void PlayRing()
        {
            _audioSource.clip = _ringClip;
            _audioSource.Play();
            _audioSource.loop = true;
        }

        public void PlayVoice(AudioClip clip)
        {
            _audioSource.clip = _hangUpPhoneClip;
            _audioSource.Play();
            _audioSource.loop = false;
            StartCoroutine(PlaySound(clip));
        }

        private IEnumerator PlaySound(AudioClip clip)
        {
            yield return new WaitForSeconds(1f);
            _audioSource.clip = clip;
            _audioSource.Play();
            yield return new WaitForSeconds(clip.length + 0.5f);
            _audioSource.clip = _hangDownPhoneClip;
            _audioSource.Play();
        }
    }
}