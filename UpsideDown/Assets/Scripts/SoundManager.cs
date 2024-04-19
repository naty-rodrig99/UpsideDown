using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip jumpSound;
    public AudioClip landSound;
    public AudioClip collectPointSound;
    public AudioClip shootSound;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayLandingSound()
    {
        audioSource.PlayOneShot(landSound);
    }

    public void PlayCollectPointSound()
    {
        audioSource.PlayOneShot(collectPointSound);
    }

    public void PlayShootSound()
    {
        audioSource.PlayOneShot(shootSound);
    }
}
