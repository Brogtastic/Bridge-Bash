using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverText : MonoBehaviour
{

    public static GameOverText instance;
    public TextMeshProUGUI text;
    private Canvas renderCanvas;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool repeat;

    private float myCounter;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

    }



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
        text.canvasRenderer.SetAlpha(0.01f);
        this.transform.position = new Vector2(0, 2);

        yield return new WaitForSeconds(1f);
        transform.localScale = new Vector2(0.2f, 0.2f);
        text.CrossFadeAlpha(1.0f, 0.5f, false);
    }
}
