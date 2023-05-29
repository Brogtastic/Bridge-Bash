using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverText : MonoBehaviour
{

    public static GameOverText instance;
    public TextMeshProUGUI gameOverText;
    private Canvas renderCanvas;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool repeat;

    private float myCounter;


    // Start is called before the first frame update
    void Start()
    {
        myCounter = 3.0f;
        repeat = false;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        if (instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if((Global.currentHealth <= 0) && (repeat == false))
        {

            StartCoroutine(BITCH());
            repeat = true;

        }

    }

    private IEnumerator BITCH()
    {
        gameOverText.canvasRenderer.SetAlpha(0.01f);
        this.transform.position = new Vector2(0, 0);

        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector2(0.2f, 0.2f);
        gameOverText.CrossFadeAlpha(1.0f, 0.5f, false);
    }
}
