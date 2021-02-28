using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.Audio;
using System;

namespace com.pebblz.finderskeepers
{
    public class SoundManager : MonoBehaviour
    {

        /*=========================Description==================*/
        /*The purpose of the sound manager script is to play all sounds. It helps us by keeping the sound effects
         in one spot. Each player has its own sound Manager Currently, the Sound Manager uses two main Audio Mixer 
        groups, one for the local player, and the other for the remote player. 
        Each These has a sub group for sound effects and music. By default the remote
        player group is muted, so we don't hear duplicate sounds. */

        /*=========Adding Sounds===========*/
        /*  1) Create an empty game object as a child of the prefab
            2) Name the Object after the sound effect or music
            3) Add an Field For the AudioSource, then drag and drop the child in
            4) In the awake method tell which mixer the sound has to go to
            5) Add a play Method in the Play_Functions Regions

        Notes: Adding a an empty game object to the sound manager is not necessary, but it makes it easier to understand
               The play_functions region also contains things for stoping sound effects like running/walking*/

        public AudioSource LobbyTheme;
        public AudioSource GameTheme;
        public AudioSource GameIntro;
        public AudioSource CPUTheme;
        [SerializeField] AudioSource PickUp;
        [SerializeField] AudioSource Jump;
        [SerializeField] AudioSource PointGot;
        [SerializeField] AudioSource Walking;
        [SerializeField] AudioSource Running;



        [SerializeField] AudioMixerGroup outputGroup;
        public bool isRemotePlayer;
        AudioMixerGroup sfx;
        AudioMixerGroup music;
        public AudioSource SceneTheme; // default music supposed to playing in a scene


        private void Awake()
        {
            // when a player is loaded through the game manager it will determine
            // where audio for that player is routed by default, all remote players are muted
            // this can be expanded upon later on to utilize unity's 3d audio for player player sfx
            // when two players are close to eachother. However, there will need to be More Audio Mixer groups
            AudioMixer mix = Resources.Load("PlayerSounds") as AudioMixer;
            if (isRemotePlayer)
            {
                //returns an array of 1
                outputGroup = mix.FindMatchingGroups("RemotePlayer")[0];

            }
            else
            {
                //returns an array of 1
                outputGroup = mix.FindMatchingGroups("LocalPlayer")[0];
            }

            //will allways return an array so we have to access the 0th element
            sfx = mix.FindMatchingGroups($"{outputGroup.name}/SFX")[0];
            music = mix.FindMatchingGroups($"{outputGroup.name}/Music")[0];

            LobbyTheme.outputAudioMixerGroup = music;
            GameTheme.outputAudioMixerGroup = music;
            GameIntro.outputAudioMixerGroup = music;
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
            //stop lobby theme
            this.SceneTheme.Stop();

            this.SceneTheme = GameTheme;
            this.SceneTheme.loop = true;

            this.GameIntro.Play();
            this.SceneTheme.PlayDelayed(GameIntro.clip.length);

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


    }
}
