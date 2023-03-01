using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public static RestartButton instance;
    private SpriteRenderer sprite;
    private bool repeat;

    // Start is called before the first frame update
    void Start()
    {
        repeat = false;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        this.transform.position = new Vector2(1.83f, 0);
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if ((Global.currentHealth <= 0) && (repeat == false))
        {
            sprite.enabled = true;
            
            StartCoroutine(BITCHES());
            repeat = true;

        }
        
    }

    private IEnumerator BITCHES()
    {

        yield return new WaitForSeconds(2f);
        sprite.color = new Color(1f, 1f, 1f, 0.1f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1f, 1f, 1f, 0.2f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1f, 1f, 1f, 0.7f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1f, 1f, 1f, 0.8f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1f, 1f, 1f, 0.9f);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color(1f, 1f, 1f, 1f);
        GetComponent<Collider2D>().enabled = true;

    }

    public void PauseRestartShow()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.transform.position = new Vector2(1.83f, 2);
        sprite.enabled = true;
        sprite.color = new Color(1f, 1f, 1f, 1f);
        GetComponent<Collider2D>().enabled = true;
    }

    public void PauseRestartHide()
    {
        if (instance == null)
        {
            instance = this;
        }
        sprite.enabled = false;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        GetComponent<Collider2D>().enabled = false;
        this.transform.position = new Vector2(1.83f, 0);
    }

    private void OnMouseDown()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("PlayingGame 1");
    }

}
