using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;


public static class SFXVolume
{
    public static float sfxVolume;
    public static float musicVolume;
}

public class OptionsMenu : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    private AudioManager audio;


    public AudioMixer audioMixer;
    Resolution[] resolutions;

    [SerializeField] public TMP_Dropdown resolutionDropdown;


    private void Start()
    {
        musicSlider.value = SFXVolume.musicVolume;
        sfxSlider.value = SFXVolume.sfxVolume;

        audio = FindObjectOfType<AudioManager>();

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for(int i = 0; i<resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            audio.newSoundSet();
            audio.Play("SmallCall2");
        }
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        SFXVolume.musicVolume = volume;
    }

    public void SetSFX(float volume)
    {
        SFXVolume.sfxVolume = volume;
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}