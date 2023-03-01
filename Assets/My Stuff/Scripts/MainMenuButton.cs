using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{
    public static MainMenuButton instance;
    private SpriteRenderer sprite;
    private bool repeat;

    // Start is called before the first frame update
    void Start()
    {
        repeat = false;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        this.transform.position = new Vector2(1.79f, -1.38f);
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if ((Global.currentHealth <= 0) && (repeat == false))
        {
            sprite.enabled = true;

            StartCoroutine(BITCHMADE());
            repeat = true;

        }

    }

    private IEnumerator BITCHMADE()
    {

        yield return new WaitForSeconds(2.3f);
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

    public void MainMenuRestartShow()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.transform.position = new Vector2(1.83f, 0.5f);
        sprite.enabled = true;
        sprite.color = new Color(1f, 1f, 1f, 1f);
        GetComponent<Collider2D>().enabled = true;
    }

    public void MainMenuRestartHide()
    {
        if (instance == null)
        {
            instance = this;
        }
        sprite.enabled = false;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        GetComponent<Collider2D>().enabled = false;
        this.transform.position = new Vector2(1.79f, -1.38f);
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

}
