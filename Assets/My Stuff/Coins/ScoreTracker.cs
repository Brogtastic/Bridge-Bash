using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTracker : MonoBehaviour
{

    public static ScoreTracker instance;
    public TextMeshProUGUI text;
    int score;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }

        score = PlayerPrefs.GetInt("Coins", 0);
        text.text = "X" + score.ToString();

    }

    private void Update()
    {
        PlayerPrefs.SetInt("Coins", score);
    }

    public void ChangeScore(int coinValue)
    {
        
        StartCoroutine(PlusAnimation(coinValue));
        
    }

    private IEnumerator PlusAnimation(int coinValue)
    {
        //float framerate = 0.01333f;
        float framerate = 0.04f;
        framerate = framerate / coinValue;

        for (int i = 1; i < coinValue + 1; i++)
        {
            score += 1;
            text.text = "X" + score.ToString();
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.05f);
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.1f);
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.15f);
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.05f);
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.1f);
            yield return new WaitForSeconds(framerate);
            this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.15f);

        }

    }


}
