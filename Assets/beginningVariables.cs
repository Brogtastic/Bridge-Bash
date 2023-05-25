using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class beginningVariables : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SFXVolume.sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        SFXVolume.musicVolume = PlayerPrefs.GetFloat("MusicVolume", -11f);
        //Global.highScore = PlayerPrefs.GetInt("HighScore", 0);

    }
}
