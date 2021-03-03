﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace com.pebblz.finderskeepers { 
    public class ChangeVolume : MonoBehaviour
    {
        public enum TargetMixer
        {
            SFX = 1,
            MUSIC
        }

        Slider slider;
        SoundManager manager;
        [SerializeField]
        TargetMixer mixer;
        private void Start()
        {
            manager = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<SoundManager>();
            slider = this.GetComponent<Slider>();
            switch (mixer)
            {
                case TargetMixer.MUSIC:
                    slider.value = manager.getMusicVol();
                    break;
                case TargetMixer.SFX:
                    slider.value = manager.getSFXVol();
                    break;
                default:
                    Debug.LogWarning("No mixer selected for volume control");
                    break;
            }
        }
        public void SetLevel(float level)
        {
            switch (mixer)
            {
                case TargetMixer.MUSIC:
                    manager.setMusicVol(level);
                    break;
                case TargetMixer.SFX:
                    manager.setSFXVol(level);
                    break;
                default:
                    Debug.LogWarning("No mixer selected for volume control");
                    break;
            }
        }
    }
}