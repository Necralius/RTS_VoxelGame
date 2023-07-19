using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace NekraliusDevelopmentStudio
{
    public class AudioManager : MonoBehaviour
    {
        //Code made by Victor Paulo Melo da Silva - Game Developer - GitHub - https://github.com/Necralius
        //CompleteCodeName - (Code Version)
        //Code State - (Needs Refactoring, Needs Coments, Needs Improvement)
        //This code represents (Code functionality or code meaning)

        #region - Singleton Pattern
        public static AudioManager Instance;
        private void Awake() => Instance = this;
        #endregion


        [Header("Audio Volume Management")]
        [SerializeField] public Slider masterSlider;
        [SerializeField] public Slider musicSlider;
        [SerializeField] public Slider effectsSlider;

        [Header("Audio Main Mixer")]
        [SerializeField] private AudioMixer mainMixer;

        [Header("AudioSource")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource effectsSource;

        [Header("Game SoundTrack")]
        int previousIndex = 0;
        [SerializeField] private List<AudioClip> GameMusics;

        public AudioDatabaseModel audioDatabase;

        public void PlayClip(AudioClip clipToPlay, AudioType mixerGroupIndentifier)
        {
            switch(mixerGroupIndentifier)
            {
                case AudioType.Music:
                    musicSource.PlayOneShot(clipToPlay);
                    break;
                case AudioType.SoundEffect:
                    effectsSource.PlayOneShot(clipToPlay);
                    break;
                default:
                    effectsSource.PlayOneShot(clipToPlay); 
                    break;
            }
        }

        public void UI_MenuButtonSelect() => PlayClip(audioDatabase.GetClip("ButtonSelect"), AudioType.SoundEffect);
        public void UI_MenuButtonClick() => PlayClip(audioDatabase.GetClip("ButtonClick"), AudioType.SoundEffect);

        #region - Music Management -
        public void ManageMusic()
        {
            if (musicSource.isPlaying || GameMusics.Count <= 0) return;
            else
            {
                int newIndex = Random.Range(0, GameMusics.Count - 1);

                if (newIndex < 0 || newIndex > GameMusics.Count) return;

                if (previousIndex == newIndex) return;
                else
                {
                    previousIndex = newIndex;
                    musicSource.PlayOneShot(GameMusics[previousIndex]);
                }
            }
        }

        private void Update()
        {
            ManageMusic();
        }
        #endregion

        #region - Volume Management -
        public void ChangeMasterVolume() => ChangeValue(masterSlider, AudioType.Master);
        public void ChangeMusicVolume() => ChangeValue(musicSlider, AudioType.Music);
        public void ChangeEffectVolume() => ChangeValue(effectsSlider, AudioType.SoundEffect);

        private void ChangeValue(Slider slider, AudioType type)
        {
            string valueString = type switch
            {
                AudioType.Master => "MasterVolume",
                AudioType.Music => "MusicVolume",
                AudioType.SoundEffect => "SoundEffectsVolume",
                _ => throw new System.NotImplementedException(),
            };

            switch (type)
            {
                case AudioType.Music:
                    valueString = "MusicVolume";
                    break;
                case AudioType.SoundEffect:
                    valueString = "SoundEffectsVolume";
                    break;
                case AudioType.Master:
                    valueString = "MasterVolume";
                    break;
            }

            switch((int)slider.value)
            {
                case 0:
                    mainMixer.SetFloat(valueString, -88);
                    break;
                case 1:
                    mainMixer.SetFloat(valueString, -40);
                    break;
                case 2:
                    mainMixer.SetFloat(valueString, -20);
                    break;
                case 3:
                    mainMixer.SetFloat(valueString, -10);
                    break;
                case 4: 
                    mainMixer.SetFloat(valueString, 0);
                    break;
            }
        }
        #endregion
    }
}