using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenAudio : MonoBehaviour
{
    public AudioClip intro;
    public AudioClip loop;
    private AudioSource audsource;

    private void Awake()
    {
        audsource = GetComponent<AudioSource>();
    }
    void Start()
    {
        StartCoroutine(playSoundinSequence());
    }
    private IEnumerator playSoundinSequence()
    {

        audsource.clip = intro;
        audsource.Play();
        yield return new WaitForSeconds(audsource.clip.length);
        audsource.loop = true;
        audsource.clip = loop;
        audsource.Play();
    }
    
}
