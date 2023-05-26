using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class HoverEnemyScript : MonoBehaviour
{
    private static readonly Random getrandom = new Random();

    public LayerMask playerLayer;
    public float attackRange = 8f;
    public int attackDamage = 10;
    public string color;

    public int maxHealth;
    int currentHealth;

    public Transform target;
    public Transform targetLeft;
    public Transform targetRight;
    public Transform leftWall;
    public Transform rightWall;

    public int speed = 5;
    public int pointValue;
    public float rotateSpeed = 200f;
    private float dirX = 0f;

    public float hoverSpeed;
    public float hoverRotateSpeed;

    private Rigidbody2D rb;

    private bool startedTrueRight;
    private bool leftPoint;

    private int whereEnemy = 0;
    private bool isChasing = false;
    private bool isHovering = false;
    private float hoverCounter;
    public float hoverTime;
    private float hoverWindUpCounter;
    public float hoverWindUpMax;

    private float hoverGetThereCounter;

    private int hoverPhase;
    public bool takeItFromHere;
    public float attackSpeed;

    private float KBCounter;
    private float KBAttackCounter;
    public float KBForce;
    public float KBAttackForce;
    public float KBForceUp;
    public float KBTotalTime;
    public float KBAttackTotalTime;

    private bool invincibility;
    public float invincibilityTime;
    private float invincibilityCounter;

    private float deadRitualCounter;

    private bool isOnFirstBridge = false;
    private bool isOnSecondBridge = false;
    private bool isOnThirdBridge = false;
    private bool isOnFourthBridge = false;
    private bool isOnFifthBridge = false;

    private SpriteRenderer sprite;
    private Animator anim;
    public PlayerMovement playerMovement;

    private enum MovementState { flying, die, follow}

    private bool okayDie = false;
    private bool isPecking;

    public GameObject coin;
    public GameObject myScore;
    public int maxCoinsDropped;
    public int minCoinsDropped;
    public int dropPercentage;

    private int whichSide;

    private CameraShake shake;
    private AudioManager audio;

    // Start is called before the first frame update
    void Start()
    {
        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<CameraShake>();
        audio = FindObjectOfType<AudioManager>();

        currentHealth = maxHealth;

        isPecking = false;
        invincibility = false;
        GlobalEnemyCheck.invincibilityTime = invincibilityTime;
        deadRitualCounter = 0;

        GameObject Spawner1 = GameObject.Find("Spawner1");
        GameObject Spawner2 = GameObject.Find("Spawner2");
        GameObject Spawner3 = GameObject.Find("Spawner3");
        GameObject Spawner4 = GameObject.Find("Spawner4");
        GameObject Spawner5 = GameObject.Find("Spawner5");

        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        targetLeft = GameObject.Find("HoverDetectorLeft").transform;
        targetRight = GameObject.Find("HoverDetectorRight").transform;

        leftWall = GameObject.Find("LeftWall").transform;
        rightWall = GameObject.Find("RightWall").transform;

        isChasing = true;
        isHovering = false;
        hoverPhase = 0;
        hoverGetThereCounter = 0f;

        takeItFromHere = false;
    }

    public void TakeDamage(int damage)
    {
        KBAttackCounter = 0f;
        if (invincibility == false)
        {

            currentHealth -= damage;

            int rand = GetRandomNumber(0, 4);
            switch (rand)
            {
                case 0:
                    audio.Play("BigDamage");
                    audio.Play("BigCall");
                    break;
                case 1:
                    audio.Play("BigDamage");
                    audio.Play("SmallCall2");
                    break;
                case 2:
                    audio.Play("BigDamage");
                    audio.Play("SmallCall1");
                    break;
                case 3:
                    audio.Play("BigDamage");
                    audio.Play("BigCall");
                    break;
            }


            StartCoroutine(TimePauseEffect());
            shake.camShake();

            GlobalEnemyCheck.invincibilityCounter = invincibilityTime;
            invincibility = true;

        }

        KBCounter = KBTotalTime;

        if (this.transform.position.x >= target.transform.position.x)
        {
            Global.knockFromRight = true;
        }
        if (this.transform.position.x < target.transform.position.x)
        {
            Global.knockFromRight = false;
        }

        //play hurt animation

        if ((currentHealth <= 0) && (currentHealth > -1000))
        {
            Die(true);
        }

    }

    void Die(bool wasDefeated)
    {
        if (wasDefeated == true)
        {
            int rand = GetRandomNumber(0, 100);
            if (rand <= dropPercentage)
            {
                int newrand = GetRandomNumber(minCoinsDropped, maxCoinsDropped);
                for (int i = 0; i < newrand; i++)
                {
                    Instantiate(coin, this.transform.position, transform.rotation);
                }
            }
        }

        GetComponent<Collider2D>().enabled = false;
        okayDie = true;
        currentHealth = -2000;

        if (wasDefeated == true)
        {
            OverParent.instance.setPosition(this.transform.position.x, this.transform.position.y);
            GlobalBlip.recentColor = color;
            Instantiate(myScore, this.transform.position, transform.rotation);

            pointValue = 750;
            PointScore.instance.ChangeScore(pointValue);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            AttackPlayer(this.transform.position.x);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GameObject Spawner1 = GameObject.Find("Spawner1");
        GameObject Spawner2 = GameObject.Find("Spawner2");
        GameObject Spawner3 = GameObject.Find("Spawner3");
        GameObject Spawner4 = GameObject.Find("Spawner4");
        GameObject Spawner5 = GameObject.Find("Spawner5");

        Collider2D enemyCollider = GetComponent<Collider2D>();

        if ((isHovering == false) && (currentHealth > 0))
        {
            isPecking = false;

            if ((KBCounter <= 0) && (KBAttackCounter <= 0))
            {
                //Follow Player
                Vector2 direction = (Vector2)target.position - rb.position;
                direction.Normalize();
                float rotateAmount = Vector3.Cross(direction, -transform.up).z;
                rb.angularVelocity = -rotateAmount * rotateSpeed;
                rb.velocity = -transform.up * speed;
            }
            else if (KBAttackCounter > 0)
            {
                if (Global.knockFromRight == false)
                {
                    rb.velocity = new Vector2(-KBAttackForce, KBForceUp);
                }
                if (Global.knockFromRight == true)
                {
                    rb.velocity = new Vector2(KBAttackForce, KBForceUp);
                }
                KBAttackCounter -= Time.deltaTime;
                if (KBAttackCounter > 0)
                {
                    KBAttackCounter -= Time.deltaTime;
                }
            }
            else
            {
                if (Global.knockFromRight == false)
                {
                    rb.velocity = new Vector2(-KBForce, KBForceUp);
                }
                if (Global.knockFromRight == true)
                {
                    rb.velocity = new Vector2(KBForce, KBForceUp);
                }
                KBCounter -= Time.deltaTime;
                if (KBAttackCounter > 0)
                {
                    KBAttackCounter -= Time.deltaTime;
                }
            }

        }
       

        if (invincibility == true)
        {
            GlobalEnemyCheck.invincibilityCounter -= Time.deltaTime;
        }
        if (GlobalEnemyCheck.invincibilityCounter <= 0)
        {
            invincibility = false;
        }

        
        

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Die"))
        {
            rb.velocity = Vector3.zero;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .9f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f))
            {
                this.enabled = false;
                Destroy(gameObject);
            }
        }

        if ((KBCounter > 0) && ((rb.velocity.y != 0) && (rb.velocity.x != 0)))
        {
            transform.up = -(target.position - transform.position);
        }
        if ((KBAttackCounter > 0) && ((rb.velocity.y != 0) && (rb.velocity.x != 0)))
        {
            transform.up = -(target.position - transform.position);
            KBAttackCounter -= Time.deltaTime;
        }

        UpdateAnimationState();

        if (this.rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        if (this.rb.velocity.x > 0)
        {
            sprite.flipX = false;
        }




        if (currentHealth > 0)
        {
            //HOVER SHIT
            if ((hoverPhase == 1) && (whichSide == 1))
            {
                isHovering = true;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("HummingbirdFly"))
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                hoverGetThereCounter += Time.deltaTime;
                hoverSpeed = hoverSpeed + (hoverGetThereCounter * 3);

                if (targetRight.position.x > targetLeft.position.x)
                {
                    float rightStep = hoverSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, targetRight.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, targetRight.transform.position, rightStep);
                }
                else
                {
                    float leftStep = hoverSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, targetLeft.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, targetLeft.transform.position, leftStep);
                }

                float checkDistance;

                if (targetRight.position.x > targetLeft.position.x)
                {
                    checkDistance = Vector3.Distance(transform.position, targetRight.transform.position);
                }
                else
                {
                    checkDistance = Vector3.Distance(transform.position, targetLeft.transform.position);
                }

                if (checkDistance < 0.5f)
                {

                    if (targetRight.position.x > targetLeft.position.x)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetRight.transform.position, 1);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetLeft.transform.position, 1);
                    }
                    if (hoverPhase == 1)
                    {
                        hoverCounter = hoverTime;
                        hoverPhase = 2;
                    }
                }

            }

            if ((hoverPhase == 1) && (whichSide == 0))
            {
                isHovering = true;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("HummingbirdFly"))
                {
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }

                hoverGetThereCounter += Time.deltaTime;
                hoverSpeed = hoverSpeed + (hoverGetThereCounter * 3);

                if (targetRight.position.x < targetLeft.position.x)
                {
                    float rightStep = hoverSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, targetRight.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, targetRight.transform.position, rightStep);
                }
                else
                {
                    float leftStep = hoverSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, targetLeft.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, targetLeft.transform.position, leftStep);
                }

                float checkDistance;

                if (targetRight.position.x < targetLeft.position.x)
                {
                    checkDistance = Vector3.Distance(transform.position, targetRight.transform.position);
                }
                else
                {
                    checkDistance = Vector3.Distance(transform.position, targetLeft.transform.position);
                }

                if (checkDistance < 0.5f)
                {
                    if (targetRight.position.x < targetLeft.position.x)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetRight.transform.position, 1);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, targetLeft.transform.position, 1);
                    }
                    if (hoverPhase == 1)
                    {
                        hoverCounter = hoverTime;
                        hoverPhase = 2;
                    }
                }

            }

            if ((hoverPhase == 2) && (whichSide == 1))
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if (targetRight.position.x > targetLeft.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetRight.transform.position, 1);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetLeft.transform.position, 1);
                }
                hoverCounter -= Time.deltaTime;
                if (hoverCounter <= 0)
                {
                    hoverWindUpCounter = hoverWindUpMax;
                    if (Global.invincibilityCounter <= 0)
                    {
                        hoverPhase = 3;
                    }
                }
            }

            if ((hoverPhase == 2) && (whichSide == 0))
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if (targetRight.position.x < targetLeft.position.x)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetRight.transform.position, 1);
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetLeft.transform.position, 1);
                }
                hoverCounter -= Time.deltaTime;
                if (hoverCounter <= 0)
                {
                    hoverWindUpCounter = hoverWindUpMax;
                    if (Global.invincibilityCounter <= 0)
                    {
                        hoverPhase = 3;
                    }
                }
            }

            if (hoverPhase == 3)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                if (this.transform.position.x > target.position.x)
                {
                    float rightStep = attackSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, rightWall.transform.position, rightStep);
                }
                else
                {
                    float rightStep = attackSpeed * Time.deltaTime;
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    transform.position = Vector3.MoveTowards(transform.position, leftWall.transform.position, rightStep);
                }

                hoverWindUpCounter -= Time.deltaTime;
                if (hoverWindUpCounter <= 0)
                {
                    if (Global.invincibilityCounter <= 0)
                    {
                        hoverPhase = 4;
                    }
                }
            }

            if (hoverPhase == 4)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                float rightStep = attackSpeed * Time.deltaTime;
                float distance = Vector3.Distance(transform.position, target.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, rightStep);
            }

        }
    }

    public void GoToPointRight()
    {
        takeItFromHere = true;
        hoverPhase = 1;
        if (Global.isPlayerFacingRight == true)
        {
            whichSide = 1;
        }
        else
        {
            whichSide = 0;
        }

    }

    public void GoToPointLeft()
    {
        takeItFromHere = true;
        hoverPhase = 1;
        if (Global.isPlayerFacingRight == true)
        {
            whichSide = 0;
        }
        else
        {
            whichSide = 1;
        }
    }

    public void GoToPointTop()
    {
        takeItFromHere = true;
        if(this.transform.position.x > target.transform.position.x)
        {
            GoToPointRight();
        }
        else
        {
            GoToPointLeft();
        }
    }

    void UpdateAnimationState()
    {
        MovementState state;


        if (color == "orange")
        {
            anim.SetFloat("flySpeed", 1.3f);
            anim.SetFloat("flyIdleSpeed", 1.05f);
        }

        /*
        if ((KBCounter <= 0) && (isHovering == false))
        {
            state = MovementState.flying;
        }
        else if ((KBAttackCounter <= 0) && (isPecking == false) && (KBAttackCounter <= 0) && (isHovering == false) && (currentHealth>0))
        {
            state = MovementState.knockback;
        }
        else if ((isHovering == true) && (currentHealth > 0) && (okayDie == false))
        {
            state = MovementState.flyingidle;
        }
        else if(currentHealth > 0)
        {
            state = MovementState.flying;
        }
        else
        {
            state = MovementState.die;
        }
        */

        if(okayDie == true)
        {
            state = MovementState.die;
        }
        else if (hoverPhase > 0)
        {
            state = MovementState.flying;
        }
        else
        {
            state = MovementState.follow; 
        }

        if (Global.currentHealth <= 0)
        {
            state = MovementState.flying;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if (deadRitualCounter < 3.0f)
            {
                rb.velocity = new Vector2(0, 0.5f * speed);
                deadRitualCounter += Time.deltaTime;
            }
            if (deadRitualCounter >= 1.0f)
            {
                rb.velocity = Vector3.zero;
            }

        }

        anim.SetInteger("state", (int)state);
    }

    void AttackPlayer(float position)
    {

        if ((Global.invincibilityCounter <= 0) && (Global.currentHealth > 0))
        {
            target.GetComponent<PlayerMovement>().TakeDamage(attackDamage, position);
            Global.KBDamageCounter = Global.KBDamageTotalTime;
            KBAttackCounter = KBAttackTotalTime;
            Global.invincibilityCounter = Global.invincibilityTime;

            if (position >= target.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (position < target.transform.position.x)
            {
                Global.knockFromRight = false;
            }

            rb.velocity = Vector3.zero;
            Die(false);
        }

    }

    public static int GetRandomNumber(int min, int max)
    {
        lock (getrandom) // synchronize
        {
            return getrandom.Next(min, max);
        }
    }

    private IEnumerator TimePauseEffect()
    {
        float framerate = 0.00035f;
        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(framerate);
        Time.timeScale = 1f;
    }

    private IEnumerator Heisdead()
    {
        float framerate = 0.1f;
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x + 0.5f, this.transform.position.y);
    }

    private IEnumerator HoverAttack()
    {
        float framerate = 1f;
        yield return new WaitForSeconds(framerate);
        lungeAttack();
    }

    private void lungeAttack()
    {
        if (Global.knockFromRight == true)
        {
            rb.velocity = new Vector2(-KBAttackForce, Math.Abs(KBForceUp));
        }
        if (Global.knockFromRight == false)
        {
            rb.velocity = new Vector2(KBAttackForce, Math.Abs(KBForceUp));
        }
        isHovering = false;
    }

}