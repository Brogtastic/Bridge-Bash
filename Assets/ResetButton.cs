using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class ResetPressed
{
    public static bool state;
}

public class ResetButton : MonoBehaviour
{
    public void onResetProgress()
    {
        PlayerPrefs.SetInt("Coins", 0);
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.SetFloat("MusicVolume", -11);
        PlayerPrefs.SetFloat("SFXVolume", 1);
        //ResetPressed.state = true;
    }
}
