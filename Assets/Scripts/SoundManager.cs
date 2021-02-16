﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour, IOnEventCallback
{
    public AudioSource LobbyTheme;
    public AudioSource GameTheme;
    public AudioSource CPUTheme;
    [SerializeField] AudioSource PickUp;
    [SerializeField] AudioSource Jump;
    [SerializeField] AudioSource PointGot;
    [SerializeField] AudioSource Walking;
    [SerializeField] AudioSource Running;

    

    [SerializeField] bool ignoreEvents = false;
    [SerializeField] AudioMixerGroup outputGroup;
    public bool isRemotePlayer;
    AudioMixerGroup sfx;
    AudioMixerGroup music;
    public AudioSource SceneTheme; // default music supposed to playing in a scene


    private void Awake()
    {
        AudioMixer mix = Resources.Load("PlayerSounds") as AudioMixer;
        if (isRemotePlayer)
        {
            outputGroup = mix.FindMatchingGroups("RemotePlayer")[0];

        } else
        {
            outputGroup = mix.FindMatchingGroups("LocalPlayer")[0];
        }

        sfx = mix.FindMatchingGroups($"{outputGroup.name}/SFX")[0];
        music = mix.FindMatchingGroups($"{outputGroup.name}/Music")[0];

        LobbyTheme.outputAudioMixerGroup = music;
        GameTheme.outputAudioMixerGroup = music;
        this.SceneTheme = LobbyTheme;

        Jump.outputAudioMixerGroup = sfx;
        PointGot.outputAudioMixerGroup = sfx;
        PickUp.outputAudioMixerGroup = sfx;
        PlayLobbyTheme();
        
    }


    #region PLAY_FUNCTIONS

    public void PlayLobbyTheme()
    {
        this.SceneTheme = LobbyTheme;
        this.SceneTheme.loop = true;
        this.SceneTheme.Play();
    }

    public void PlayGameTheme()
    {
        this.SceneTheme = GameTheme;
        this.SceneTheme.loop = true;
        this.SceneTheme.Play();
    }

    public void PlayJump()
    {
        if (Jump.isPlaying) return;
        Jump.Play();
    }

    public void PlayPickUp()
    {
        if (PickUp.isPlaying) return;
        PickUp.Play();
    }

    public void PlayPointGot()
    {
        if (PointGot.isPlaying) return;
        PointGot.Play();
    }

    public void PlayRunning()
    {
        return;
    }

    public void StopRunningSFX()
    {
        return;
    }

    public void PlayWalking()
    {
        return;
    }

    #endregion


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
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

    public void resumeOriginalTrack()
    {
        throw new NotImplementedException();
    }

    public void switchAudioTracks()
    {
        throw new NotImplementedException();
    }

    public void OnEvent(EventData photonEvent)
    {
    
        byte eventCode = photonEvent.Code;
        Debug.Log("Recieved Event code: " + NetworkCodes.getNameForCode(eventCode));
        if (eventCode == NetworkCodes.ChangeToGameMusicEvent)
        {
            this.SceneTheme.Stop();
            this.PlayGameTheme();
        }
        
    }
}

