using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

static class GlobalBlip
{
    public static string recentColor;
    public static int recentPoints;
}

public class ScoreBlip : MonoBehaviour
{
    public static ScoreBlip instance;
    public TextMeshProUGUI text;
    public float speed;
    private Canvas renderCanvas;
    private float recenty;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private float initialx;
    int thisscore;
    private string color;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

    }

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        transform.SetParent(GameObject.FindGameObjectWithTag("OverParent").transform, false);
        initialx = this.transform.position.x;
        recenty = this.transform.position.y;

        color = GlobalBlip.recentColor;
        int points = GlobalBlip.recentPoints;

        if (color == "red") {
            text.text = "+100";
            thisscore = 100;
            text.color = new Color(255, 0, 7, 255);
            text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32(192, 0, 18, 188));
        }
        else if (color == "green")
        {
            text.text = "+200";
            thisscore = 200;
            text.color = new Color(12, 197, 0, 255);
            text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32(43, 125, 0, 188));
        }
        else if (color == "blue")
        {
            text.text = "+500";
            thisscore = 500;
            text.color = new Color(0, 105, 255, 255);
            text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32(75, 144, 251, 188));
        }
        else if (color == "white")
        {
            text.text = "+1000";
            thisscore = 1000;
            text.color = new Color(0, 253, 255, 255);
            text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32(109, 35, 255, 188));
        }
        else if (color == "orange")
        {
            text.text = "+750";
            thisscore = 750;
            text.color = new Color(0, 253, 255, 255);
            text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32(109, 35, 255, 188));
        }
        else if (color == "hitPoint")
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            text.text = "+" + GlobalBlip.recentPoints.ToString();
            thisscore = GlobalBlip.recentPoints;
            text.color = new Color(255, 255, 255, 255);
            text.fontSharedMaterial.SetColor(ShaderUtilities.ID_GlowColor, new Color32(0, 0, 0, 200));
        }

        StartCoroutine(Animate());
    }


    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        rb.velocity = new Vector2(0, speed);
        SetTransformX(initialx);

        if (transform.position.y < recenty)
        {
            SetTransformY(recenty);
        }
        recenty = transform.position.y;

    }

    void SetTransformX(float n)
    {
        transform.position = new Vector3(n, transform.position.y, transform.position.z);
    }

    void SetTransformY(float n)
    {
        transform.position = new Vector3(transform.position.x, n, transform.position.z);
    }

    private IEnumerator Animate()
    {
        Vector2 objectScale = transform.localScale;
        float framerateanimate;
        float frameratefade;

        if (color == "hitPoint")
        {
            framerateanimate = 0.01f;
            frameratefade = 0.18f;
        }
        else
        {
            framerateanimate = 0.05f;
            frameratefade = 0.3f;
        }

        transform.localScale = new Vector2(0.6f, objectScale.y + 0.5f);
        yield return new WaitForSeconds(framerateanimate);
        transform.localScale = new Vector2(0.8f, objectScale.y + 0.2f);
        yield return new WaitForSeconds(framerateanimate);
        transform.localScale = new Vector2(objectScale.x + 0.5f, 0.6f);
        yield return new WaitForSeconds(framerateanimate);
        transform.localScale = new Vector2(objectScale.x + 0.2f, 0.8f);
        yield return new WaitForSeconds(framerateanimate);
        transform.localScale = new Vector2(objectScale.x - 0.5f, 1.5f);
        yield return new WaitForSeconds(framerateanimate);
        transform.localScale = new Vector2(objectScale.x, objectScale.y);
        yield return new WaitForSeconds(frameratefade);

        text.CrossFadeAlpha(0f, frameratefade, true);
        yield return new WaitForSeconds(frameratefade);
        Destroy(gameObject);

    }

}