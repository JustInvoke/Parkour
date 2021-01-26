using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;

    public void PlaySound(AudioClip audioClip) {
        audioSource.PlayOneShot(audioClip);
    }

}
