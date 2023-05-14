using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeButton : MonoBehaviour
{
    public static ResumeButton instance;
    private SpriteRenderer sprite;
    private bool repeat;

    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        repeat = false;
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        this.transform.position = new Vector2(1.79f, -1.38f);
        GetComponent<Collider2D>().enabled = false;
    }

    public void ResumeShow()
    {
        if (instance == null)
        {
            instance = this;
        }
        this.transform.position = new Vector2(1.83f, -1.0f);
        sprite.enabled = true;
        sprite.color = new Color(1f, 1f, 1f, 1f);
        GetComponent<Collider2D>().enabled = true;
    }

    public void ResumeHide()
    {
        if (instance == null)
        {
            instance = this;
        }
        sprite.enabled = false;
        sprite.color = new Color(1f, 1f, 1f, 0f);
        GetComponent<Collider2D>().enabled = false;
        this.transform.position = new Vector2(26.25f, 3.81f);
    }

    private void OnMouseDown()
    {
        playerMovement.resumeGame();
    }
}
