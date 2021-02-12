using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class SoundtrackManager : MonoBehaviour, IOnEventCallback, IPunObservable
{
    [SerializeField] AudioSource soundtrack;
    [SerializeField] AudioSource GameTheme;

    [SerializeField] bool ignoreEvents = false;

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

    public void OnEvent(EventData photonEvent)
    {
        if (ignoreEvents)
        {
            byte eventCode = photonEvent.Code;
            if (eventCode == NetworkCodes.ChangeToGameMusicEvent)
            {
                this.GameTheme.Play();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsReading)
        {
            this.soundtrack.time = (float)stream.ReceiveNext();
            this.GameTheme.time = (float)stream.ReceiveNext();

        } else
        {
            stream.SendNext(this.soundtrack.time);
            stream.SendNext(this.GameTheme.time);
        }
    }
}
