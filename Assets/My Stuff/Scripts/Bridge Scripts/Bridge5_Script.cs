using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge5_Script : MonoBehaviour
{

    private SpriteRenderer sprite;
    private Animator anim;

    public int health = 100;
    private int currentHealth;
    private bool invincibility;
    public float invincibilityTime;
    private float invincibilityCounter;

    private bool bridgeBDamage;
    private bool bridgeCDamage;
    private bool bridgeDDamage;
    private bool bridgeEDamage;
    private bool bridgeGDamage;

    private CameraShake shake;

    private enum MovementState { bridgeA, bridgeB, bridgeC, bridgeD, bridgeE, bridgeGone }

    // Start is called before the first frame update
    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<CameraShake>();

        currentHealth = health;
        bridgeBDamage = false;
        bridgeCDamage = false;
        bridgeDDamage = false;
        bridgeEDamage = false;
        bridgeGDamage = false;
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimationState();

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("BridgeGone"))
        {
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .15f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
            {
                if (GetComponent<Collider2D>().enabled == true)
                {
                    FindObjectOfType<AudioManager>().Play("BridgeBreak");
                    FindObjectOfType<AudioManager>().Play("BridgeBreak8");
                }
                GetComponent<Collider2D>().enabled = false;
                GlobalEnemyCheck.bridge5dead = true;
                shake.camShake2();
            }
        }

        if (GlobalEnemyCheck.isEnemyFifth == false)
        {
            invincibility = true;
            invincibilityCounter = invincibilityTime;
        }

        if (currentHealth <= 0)
        {
            invincibility = true;
        }

        if (invincibility == true)
        {
            invincibilityCounter -= Time.deltaTime;
        }
        if (invincibilityCounter <= 0)
        {
            invincibility = false;
        }
    }


    public void TakeDamage(int damage)
    {
        if (invincibility == false)
        {
            currentHealth -= damage;
            invincibilityCounter = invincibilityTime;
            invincibility = true;
            Debug.Log("This Bridge Health is: " + currentHealth);

        }



        if (currentHealth <= health * .8f)
        {
            if ((bridgeBDamage == false) && (currentHealth >= health * .6f))
            {
                FindObjectOfType<AudioManager>().Play("BridgeDamage");
            }
            bridgeBDamage = true;

            bridgeCDamage = false;
            bridgeDDamage = false;
            bridgeEDamage = false;
            bridgeGDamage = false;
        }
        if (currentHealth <= health * .6f)
        {
            bridgeCDamage = true;

            bridgeBDamage = false;
            bridgeDDamage = false;
            bridgeEDamage = false;
            bridgeGDamage = false;
        }
        if (currentHealth <= health * .4f)
        {
            bridgeDDamage = true;

            bridgeBDamage = false;
            bridgeCDamage = false;
            bridgeEDamage = false;
            bridgeGDamage = false;
        }
        if (currentHealth <= health * .2f)
        {
            bridgeEDamage = true;

            bridgeBDamage = false;
            bridgeCDamage = false;
            bridgeDDamage = false;
            bridgeGDamage = false;
        }
        if (currentHealth <= 0)
        {
            bridgeGDamage = true;

            bridgeBDamage = false;
            bridgeCDamage = false;
            bridgeDDamage = false;
            bridgeEDamage = false;

        }

    }

    void UpdateAnimationState()
    {
        MovementState state;


        if (bridgeBDamage == true)
        {
            state = MovementState.bridgeB;
        }
        else if (bridgeCDamage == true)
        {
            state = MovementState.bridgeC;
        }
        else if (bridgeDDamage == true)
        {
            state = MovementState.bridgeD;
        }
        else if (bridgeEDamage == true)
        {
            state = MovementState.bridgeE;
        }
        else if (bridgeGDamage == true)
        {
            state = MovementState.bridgeGone;
        }
        else
        {
            state = MovementState.bridgeA;
        }

        anim.SetInteger("state", (int)state);
    }

}
