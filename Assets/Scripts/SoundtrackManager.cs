using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackManager : MonoBehaviour
{
    [SerializeField] AudioSource soundtrack;
    
    public void switchAudioTracks()
    {
        if(soundtrack == null)
        {
            Debug.LogWarning("No secondary soundtrack provided");
        }

        GameObject camera = GameObject.Find("Main Camera");
        AudioSource original = camera.GetComponentInChildren<AudioSource>();
        original.Pause();
        this.soundtrack.Play();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Destroy(this.gameObject);
        }
    }


    public void resumeOriginalTrack()
    {

        this.soundtrack.Pause();
        GameObject camera = GameObject.Find("Main Camera");
        AudioSource original = camera.GetComponentInChildren<AudioSource>();
        original.Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            switchAudioTracks();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            resumeOriginalTrack();
        }
    }
}
