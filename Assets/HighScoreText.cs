using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreText : MonoBehaviour
{

    public TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        Global.highScore = PlayerPrefs.GetInt("HighScore", 0);
        text.text = Global.highScore.ToString();
        if (text.text.Length < 8)
        {
            for (int j = 0; j < 8 - Global.highScore.ToString().Length; j++)
            {
                text.text = text.text.Insert(0, "0");
            }
        }
        text.text = text.text.Insert(0, "Hi-Score: ");
        PlayerPrefs.Save();
    }
}
