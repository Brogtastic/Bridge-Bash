using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class CoinScript : MonoBehaviour
{
    public int coinValue;
    public int pointValue;
    public float coinKB;
    public float coinKBUp;
    public float stayingPower;
    private float stayCounter;
    private Rigidbody2D rb;
    private static readonly Random getrandom = new Random();
    private int rand;
    private bool grabbed;

    private Animator anim;
    private SpriteRenderer sprite;
    private enum MovementState { spin, grab }

    [SerializeField] public AudioClip Grab;
    private AudioSource audioSource;
    private AudioManager audio;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        audio = FindObjectOfType<AudioManager>();

        grabbed = false;
        stayCounter = stayingPower;

        rand = GetRandomNumber(0, 2);
        int KBrand = GetRandomNumber(1, 5);
        int KBUprand = GetRandomNumber(1, 5);
        if (rand == 0)
        {
            rb.velocity = new Vector2(-coinKB, coinKBUp + KBUprand);
        }
        if (rand == 1)
        {
            rb.velocity = new Vector2(coinKB, coinKBUp + KBUprand);
        }
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        UpdateAnimationState();

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("CoinGrab"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .9f)
            {
                Destroy(gameObject);
            }
        }

        if (stayCounter > 0)
        {
            stayCounter -= Time.deltaTime;
        }
        if((stayCounter <= 0) && (stayCounter > -5))
        {
            StartCoroutine(FlashThenDisappear());
            stayCounter = -6f;
        }
        if(grabbed == false)
        {
            GetComponent<Collider2D>().enabled = true;
        }
    }

    void UpdateAnimationState()
    {
        MovementState state;

        state = MovementState.spin;
        if (grabbed == true)
        {
            rb.velocity = Vector3.zero;
            state = MovementState.grab;
        }
        anim.SetInteger("state", (int)state);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            StartCoroutine(CoinSound());
            GetComponent<Collider2D>().enabled = false;
            grabbed = true;
            ScoreTracker.instance.ChangeScore(coinValue);
            PointScore.instance.ChangeScore(pointValue);
        }
    }

    public static int GetRandomNumber(int min, int max)
    {
        lock (getrandom) // synchronize
        {
            return getrandom.Next(min, max);
        }
    }

    private IEnumerator FlashThenDisappear()
    {
        float flashDelayInvisible = 0.1f;
        float flashDelayVisible = 0.13f;

        for (int i = 0; i < 5; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(flashDelayInvisible);
            sprite.enabled = true;
            yield return new WaitForSeconds(flashDelayVisible);
        }
        for (int i = 0; i < 3; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(flashDelayInvisible / 2f);
            sprite.enabled = true;
            yield return new WaitForSeconds(flashDelayVisible / 2f);
        }
        for (int i = 0; i < 5; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(flashDelayInvisible / 3f);
            sprite.enabled = true;
            yield return new WaitForSeconds(flashDelayVisible / 3f);
        }
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);

    }
    private IEnumerator CoinSound()
    {
        float framerate = 0.03f / (coinValue / 2);
        for (int i = 0; i < coinValue; i++)
        {
            audio.Play("CoinGrab");
            yield return new WaitForSeconds(framerate);
        }
    }
}
