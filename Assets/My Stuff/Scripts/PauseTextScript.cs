using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseTextScript : MonoBehaviour
{
    public static PauseTextScript instance;
    public TextMeshProUGUI text;
    private Canvas renderCanvas;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        this.transform.position = new Vector2(2.5f, 20);

    }

    public void PauseThisWhore()
    {
        if (instance == null)
        {
            instance = this;
        }
        transform.localScale = new Vector2(0.2f, 0.2f);
        this.transform.position = new Vector2(2.5f, 5);
    }
    public void UnpauseThisWhore()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.transform.position = new Vector2(2.5f, 20);
    }
}
