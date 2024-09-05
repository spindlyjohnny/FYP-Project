
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour {

    [SerializeField] AudioMixer audioMixer;
    public Slider SFXSlider, musicSlider;

    private void Awake() {
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }
    void Start() {
        SFXSlider.value = PlayerPrefs.GetFloat("SFX Volume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("Music Volume", .7f);
        SFXSlider.maxValue = 1f;
        musicSlider.maxValue = .7f;
        //volumeSlider.value = volume;
    }
    private void OnDisable() {
        PlayerPrefs.SetFloat("SFX Volume", SFXSlider.value);
        PlayerPrefs.SetFloat("Music Volume", musicSlider.value);
    }
    public void SetSFXVolume(float volume) {
        // Set the AudioMixer volume to the slider value
        audioMixer.SetFloat("SFX Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX Volume", SFXSlider.value);
        // Store the slider value in PlayerPrefs

    }
    public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("BGM Volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("Music Volume", musicSlider.value);
    }
}
