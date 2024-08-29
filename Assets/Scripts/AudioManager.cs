using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip levelComplete, rotationSound, levelFail;

    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayRotationSound()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(rotationSound);
    }
    public void PlayLevelComplete()
    {
        audioSource.PlayOneShot(levelComplete);
    }
    public void PlayLevelFail()
    {
        audioSource.PlayOneShot(levelFail);
    }
}
