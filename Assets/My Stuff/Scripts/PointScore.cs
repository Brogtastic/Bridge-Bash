using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = System.Random;

public class PointScore : MonoBehaviour
{
    public static PointScore instance;
    public TextMeshProUGUI text;
    int score;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
        }
    }

    public void ChangeScore(int pointValue)
    {

        StartCoroutine(PlusAnimation(pointValue));

    }

    private void Update()
    {
        if(Global.currentHealth <= 0)
        {
            audioSource.Stop();
        }

        if(score > Global.highScore)
        {
            Global.highScore = score;
            PlayerPrefs.SetInt("HighScore", Global.highScore);
        }
    }

    public void PauseTheMusic()
    {
        audioSource.Pause();
        Debug.Log("Music Pause");
    }
    public void UnpauseTheMusic()
    {
        audioSource.UnPause();
        Debug.Log("Music UnPause");
    }

    private IEnumerator PlusAnimation(int pointValue)
    {

        float framerate = 0.00001f;
        float intensity = 0.05f;

        Random rnd = new Random();
        int factor = rnd.Next(17, 27);

        framerate = .0000000001f;
        for (int i = 0; i < (pointValue / factor); i++)
        {
            score += factor;
            text.text = score.ToString();
            if (text.text.Length < 8)
            {
                for (int j = 0; j < 8 - score.ToString().Length; j++)
                {
                    text.text = text.text.Insert(0, "0");
                }
            }
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + intensity);
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - intensity);
        }
        score += pointValue % factor;
        text.text = score.ToString();
        if (text.text.Length < 8)
        {
            for (int j = 0; j < 8 - score.ToString().Length; j++)
            {
                text.text = text.text.Insert(0, "0");
            }
        }


    }
}
