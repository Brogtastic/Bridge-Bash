using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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


        if(pointValue <= 99)
        {
            framerate = .0000000001f;
            for (int i = 0; i < (pointValue); i++)
            {
                score += 1;
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
            score += pointValue % 1;
            text.text = score.ToString();
            if (text.text.Length < 8)
            {
                for (int j = 0; j < 8 - score.ToString().Length; j++)
                {
                    text.text = text.text.Insert(0, "0");
                }
            }
        }
        if (pointValue == 100)
        {
            framerate = .0000000001f;
            for (int i = 0; i < (pointValue / 3); i++)
            {
                score += 3;
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
            score += pointValue % 3;
            text.text = score.ToString();
            if (text.text.Length < 8)
            {
                for (int j = 0; j < 8 - score.ToString().Length; j++)
                {
                    text.text = text.text.Insert(0, "0");
                }
            }
        }
        if (pointValue == 200)
        {
            framerate = .0000000001f;
            for (int i = 0; i < (pointValue / 7); i++)
            {
                score += 7;
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
            score += pointValue % 7;
            text.text = score.ToString();
            if (text.text.Length < 8)
            {
                for (int j = 0; j < 8 - score.ToString().Length; j++)
                {
                    text.text = text.text.Insert(0, "0");
                }
            }
        }
        if (pointValue == 500)
        {
            framerate = .0000000001f;
            for (int i = 0; i < (pointValue / 14); i++)
            {
                score += 14;
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
            score += pointValue % 14;
            text.text = score.ToString();
            if (text.text.Length < 8)
            {
                for (int j = 0; j < 8 - score.ToString().Length; j++)
                {
                    text.text = text.text.Insert(0, "0");
                }
            }
        }
        if (pointValue == 750)
        {
            framerate = .0000000001f;
            for (int i = 0; i < (pointValue / 14); i++)
            {
                score += 14;
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
            score += pointValue % 14;
            text.text = score.ToString();
            if (text.text.Length < 8)
            {
                for (int j = 0; j < 8 - score.ToString().Length; j++)
                {
                    text.text = text.text.Insert(0, "0");
                }
            }
        }
        if (pointValue == 1000)
        {
            framerate = .0000000001f;
            for (int i = 0; i < (pointValue / 20); i++)
            {
                score += 20;
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
            score += pointValue % 20;
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
}
