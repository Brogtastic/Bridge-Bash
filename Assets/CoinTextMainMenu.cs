using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinTextMainMenu : MonoBehaviour
{
    public TextMeshProUGUI text;
    public int textLength;

    // Start is called before the first frame update
    void Start()
    {
        int coins = PlayerPrefs.GetInt("Coins", 0);
        text.text = coins.ToString();
        CoinPositionInstructor.pos = coins.ToString().Length - 1;
    }
}
