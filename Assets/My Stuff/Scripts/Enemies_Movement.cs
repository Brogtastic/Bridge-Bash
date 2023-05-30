using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;


static class GlobalEnemyCheck
{
    public static bool isEnemyFirst;
    public static bool isEnemySecond;
    public static bool isEnemyThird;
    public static bool isEnemyFourth;
    public static bool isEnemyFifth;

    public static float invincibilityTime;
    public static float invincibilityCounter;

    public static bool bridge1dead;
    public static bool bridge2dead;
    public static bool bridge3dead;
    public static bool bridge4dead;
    public static bool bridge5dead;
}

[RequireComponent(typeof(Rigidbody2D))]
public class Enemies_Movement : MonoBehaviour
{
    private static readonly Random getrandom = new Random();

    public LayerMask playerLayer;
    public float attackRange = 8f;
    public int attackDamage = 10;
    public string color;

    public int maxHealth;
    int currentHealth;

    public Transform target;

    public int speed = 5;
    public int pointValue;
    public int hitPointValue;
    public float rotateSpeed = 200f;
    private float dirX = 0f;

    private Rigidbody2D rb;

    private int whereEnemy = 0;
    private bool isChasing = false;

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

    private enum MovementState { flying, pecking, die, knockback, peckingknockback, flyingidle}

    private bool okayDie = false;
    private bool isPecking;

    public GameObject coin;
    public GameObject myScore;
    public int maxCoinsDropped;
    public int minCoinsDropped;
    public int dropPercentage;

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

        if (this.transform.position.x == Spawner1.transform.position.x)
        {
            whereEnemy = 1;
            if ((GlobalEnemyCheck.isEnemyFirst == true) || (GlobalEnemyCheck.bridge1dead == true))
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else if (Spawner2.transform.position.x == this.transform.position.x)
        {
            whereEnemy = 2;
            if ((GlobalEnemyCheck.isEnemySecond == true) || (GlobalEnemyCheck.bridge2dead == true))
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else if (Spawner3.transform.position.x == this.transform.position.x)
        {
            whereEnemy = 3;
            if ((GlobalEnemyCheck.isEnemyThird == true) || (GlobalEnemyCheck.bridge3dead == true))
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else if (Spawner4.transform.position.x == this.transform.position.x)
        {
            whereEnemy = 4;
            if ((GlobalEnemyCheck.isEnemyFourth == true) || (GlobalEnemyCheck.bridge4dead == true))
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else if (Spawner5.transform.position.x == this.transform.position.x)
        {
            whereEnemy = 5;
            if ((GlobalEnemyCheck.isEnemyFifth == true) || (GlobalEnemyCheck.bridge5dead == true))
            {
                isChasing = true;
            }
            else
            {
                isChasing = false;
            }
        }
        else
        {
            isChasing = true;
        }
    }

    public void TakeDamage(int damage)
    {
        KBAttackCounter = 0f;
        if (invincibility == false)
        {
            currentHealth -= damage;
            if (currentHealth > 0)
            {
                int rand = GetRandomNumber(0, 2);
                switch (rand)
                {
                    case 0:
                        audio.Play("SmallCall1");
                        break;
                    case 1:
                        audio.Play("SmallCall2");
                        break;
                }

                audio.Play("SmallDamage");
                GlobalBlip.recentColor = "hitPoint";
                PointScore.instance.ChangeScore(hitPointValue);
                OverParent.instance.setPosition(this.transform.position.x, this.transform.position.y);
                Instantiate(myScore, this.transform.position, transform.rotation);
            }
            else
            {
                if (color == "blue")
                {
                    audio.Play("BigDamage");
                    audio.Play("BigCall");
                }
                if (color == "red")
                {
                    audio.Play("BigDamage");
                    audio.Play("SmallCall2");
                }
                if (color == "green")
                {
                    audio.Play("BigDamage");
                    audio.Play("SmallCall1");
                }
                if (color == "white")
                {
                    audio.Play("BigDamage");
                    audio.Play("BigCall");
                }
            }
            GlobalEnemyCheck.invincibilityCounter = invincibilityTime;
            invincibility = true;

            if(rb.velocity.y != 0)
            {
                shake.camShake2();
            }
            if (isChasing == false)
            {
                shake.camShake();
            }
            if((rb.velocity.y != 0) && (currentHealth > 0))
            {
                StartCoroutine(TimePauseEffect());
            }
        }

        if((isChasing == false) && (isOnFirstBridge == false) && (isOnSecondBridge == false) && (isOnThirdBridge == false) && (isOnFourthBridge == false) && (isOnFifthBridge == false))
        {
            isChasing = true;
        }

        if (isPecking == true)
        {
            KBCounter = 0.15f;
        }
        else
        {
            KBCounter = KBTotalTime;
        }

        if (this.transform.position.x >= target.transform.position.x)
        {
            Global.knockFromRight = true;
        }
        if (this.transform.position.x < target.transform.position.x)
        {
            Global.knockFromRight = false;
        }

        //play hurt animation

        if (currentHealth <= 0) {
            Die();
        }

    }

    void Die()
    {
        int rand = GetRandomNumber(0, 100);
        if(rand <= dropPercentage)
        {
            int newrand = GetRandomNumber(minCoinsDropped, maxCoinsDropped);
            for (int i = 0; i < newrand; i++)
            {
                Instantiate(coin, this.transform.position, transform.rotation);
            }
        }

        if (isOnFirstBridge == true)
        {
            isOnFirstBridge = false;
            GlobalEnemyCheck.isEnemyFirst = false;
        }
        if (isOnSecondBridge == true)
        {
            isOnSecondBridge = false;
            GlobalEnemyCheck.isEnemySecond = false;
        }
        if (isOnThirdBridge == true)
        {
            isOnThirdBridge = false;
            GlobalEnemyCheck.isEnemyThird = false;
        }
        if (isOnFourthBridge == true)
        {
            isOnFourthBridge = false;
            GlobalEnemyCheck.isEnemyFourth = false;
        }
        if (isOnFifthBridge == true)
        {
            isOnFifthBridge = false;
            GlobalEnemyCheck.isEnemyFifth = false;
        }
        GetComponent<Collider2D>().enabled = false;
        okayDie = true;

        // score blip gets generated in this coroutine
        // I have to stagger them in millisecond intervals to prevent some weird overlap glitch 
        // that happens when you kill two birds of differing colors at the same time
        // and they both display the same score which they shouldn't but that's because
        // score blip can't display two separate scores at the exact same time unfortunately
        StartCoroutine(RandomScoreBlipStallTimeForPussies());

        PointScore.instance.ChangeScore(pointValue);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (isChasing == false)
        {
            if (collision.CompareTag("Bridge1"))
            {
                isOnFirstBridge = true;
                rb.velocity = Vector3.zero;
                isPecking = true;
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.65f);
            }
            if (collision.CompareTag("Bridge2"))
            {
                isOnSecondBridge = true;
                rb.velocity = Vector3.zero;
                isPecking = true;
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.65f);
            }
            if (collision.CompareTag("Bridge3"))
            {
                isOnThirdBridge = true;
                rb.velocity = Vector3.zero;
                isPecking = true;
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.65f);
            }
            if (collision.CompareTag("Bridge4"))
            {
                isOnFourthBridge = true;
                rb.velocity = Vector3.zero;
                isPecking = true;
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.65f);
            }
            if (collision.CompareTag("Bridge5"))
            {
                isOnFifthBridge = true;
                rb.velocity = Vector3.zero;
                isPecking = true;
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.65f);
            }
        }
        
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
        //Collider2D Bridge1Collider = GameObject.Find("Bridge1").GetComponent<BoxCollider2D>();

        dirX = Input.GetAxisRaw("Horizontal");

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_PeckingKnockback")){
            sprite.sortingOrder = 10;
        }

        if (isChasing == true)
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
                if(KBAttackCounter > 0)
                {
                    KBAttackCounter -= Time.deltaTime;
                }
            }

            
        }
        else if (isChasing == false)
        {

            if (whereEnemy == 1)
            {
                if (isOnFirstBridge == false)
                {
                    rb.velocity = new Vector2(0, -1 * speed);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    GlobalEnemyCheck.isEnemyFirst = true;
                }
            }
            else if (whereEnemy == 2)
            {
                if (isOnSecondBridge == false)
                {
                    rb.velocity = new Vector2(0, -1 * speed);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    GlobalEnemyCheck.isEnemySecond = true;
                    
                }
            }
            else if (whereEnemy == 3)
            {
                if (isOnThirdBridge == false)
                {
                    rb.velocity = new Vector2(0, -1 * speed);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    GlobalEnemyCheck.isEnemyThird = true;
                    
                }
            }
            else if (whereEnemy == 4)
            {
                if (isOnFourthBridge == false)
                {
                    rb.velocity = new Vector2(0, -1 * speed);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    GlobalEnemyCheck.isEnemyFourth = true;
                    
                }
            }
            else if (whereEnemy == 5)
            {
                if (isOnFifthBridge == false)
                {
                    rb.velocity = new Vector2(0, -1 * speed);
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    GlobalEnemyCheck.isEnemyFifth = true;
                    
                }
            }

            if(Global.currentHealth <= 0)
            {
                GlobalEnemyCheck.isEnemyFirst = false;
                GlobalEnemyCheck.isEnemySecond = false;
                GlobalEnemyCheck.isEnemyThird = false;
                GlobalEnemyCheck.isEnemyFourth = false;
                GlobalEnemyCheck.isEnemyFifth = false;
            }

        }

        //HEERERRRE
        //HEEERRREEEE
        if (invincibility == true)
        {
            GlobalEnemyCheck.invincibilityCounter -= Time.deltaTime;
        }
        if (GlobalEnemyCheck.invincibilityCounter <= 0)
        {
            invincibility = false;
        }

        if ((whereEnemy == 1) && (GlobalEnemyCheck.isEnemyFirst == true) && (isOnFirstBridge == false))
        {
            isChasing = true;
        }
        if ((whereEnemy == 1) && (isOnFirstBridge == true) && (GlobalEnemyCheck.bridge1dead == true))
        {
            isChasing = true;
            isOnFirstBridge = false;
        }

        if ((whereEnemy == 2) && (GlobalEnemyCheck.isEnemySecond == true) && (isOnSecondBridge == false))
        {
            isChasing = true;
        }
        if ((whereEnemy == 2) && (isOnSecondBridge == true) && (GlobalEnemyCheck.bridge2dead == true))
        {
            isChasing = true;
            isOnSecondBridge = false;
        }

        if ((whereEnemy == 3) && (GlobalEnemyCheck.isEnemyThird == true) && (isOnThirdBridge == false))
        {
            isChasing = true;
        }
        if ((whereEnemy == 3) && (isOnThirdBridge == true) && (GlobalEnemyCheck.bridge3dead == true))
        {
            isChasing = true;
            isOnThirdBridge = false;
        }

        if ((whereEnemy == 4) && (GlobalEnemyCheck.isEnemyFourth == true) && (isOnFourthBridge == false))
        {
            isChasing = true;
        }
        if ((whereEnemy == 4) && (isOnFourthBridge == true) && (GlobalEnemyCheck.bridge4dead == true))
        {
            isChasing = true;
            isOnFourthBridge = false;
        }

        if ((whereEnemy == 5) && (GlobalEnemyCheck.isEnemyFifth == true) && (isOnFifthBridge == false))
        {
            isChasing = true;
        }
        if ((whereEnemy == 5) && (isOnFifthBridge == true) && (GlobalEnemyCheck.bridge5dead == true))
        {
            isChasing = true;
            isOnFifthBridge = false;
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
        if((KBAttackCounter > 0) && ((rb.velocity.y != 0) && (rb.velocity.x != 0)))
        {
            transform.up = -(target.position - transform.position);
            KBAttackCounter -= Time.deltaTime;
        }

        BridgeDamageCheck();
        UpdateAnimationState();
        PeckingKB();

        if (this.rb.velocity.x < 0)
        {
            sprite.flipX = true;
        }
        if (this.rb.velocity.x > 0)
        {
            sprite.flipX = false;
        }

    }

    void PeckingKB()
    {
        if((isPecking == true) && (KBCounter > 0)){
            KBCounter -= Time.deltaTime;
        }
    }

    void UpdateAnimationState()
    {
        MovementState state;
        Random rnd = new Random();
        float idleRand = (float) rnd.Next(-3, 5) * 0.1f;

        if (color == "red")
        {
            anim.SetFloat("flySpeed", 2f);
            anim.SetFloat("flyIdleSpeed", 1.15f + idleRand);
        }
        if (color == "blue")
        {
            anim.SetFloat("flySpeed", 1f);
            anim.SetFloat("flyIdleSpeed", 0.93f + idleRand);
        }
        if (color == "green")
        {
            anim.SetFloat("flySpeed", 1.3f);
            anim.SetFloat("flyIdleSpeed", 1f + idleRand);
        }
        if (color == "white")
        {
            anim.SetFloat("flySpeed", 1.3f);
            anim.SetFloat("flyIdleSpeed", 1.05f + idleRand);
        }


        if ((isPecking == true) && (KBCounter <= 0))
        {
            state = MovementState.pecking;
        }
        else if ((isPecking == true) && (KBAttackCounter > 0))
        {
            state = MovementState.pecking;
        }
        else if ((isPecking == true) && (KBCounter > 0) && (KBAttackCounter <= 0))
        {
            state = MovementState.peckingknockback;
        }
        else if (KBCounter <= 0)
        {
            state = MovementState.flying;
        }
        else if ((KBAttackCounter <= 0) && (isPecking == false) && (KBAttackCounter <= 0))
        {
            state = MovementState.knockback;
        }
        else
        {
            state = MovementState.flying;
        }

        if (okayDie == true)
        {
            state = MovementState.die;
        }
        
        if(Global.currentHealth <= 0)
        {
            state = MovementState.flyingidle;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            if(deadRitualCounter < 3.0f)
            {
                rb.velocity = new Vector2(0, 0.5f * speed);
                deadRitualCounter += Time.deltaTime;
            }
            if(deadRitualCounter >= 1.0f)
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
        }
        
    }


    public void BridgeDamageCheck()
    {
        if ((this.anim.GetCurrentAnimatorStateInfo(0).IsName("Enemy_PeckingKnockback") == false) && ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.25f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.8f)))
        {
            Bridge1_Script bridge1;
            bridge1 = GameObject.Find("Bridge1").GetComponent<Bridge1_Script>();

            if ((GlobalEnemyCheck.isEnemyFirst) && (GlobalEnemyCheck.bridge1dead == false))
            {
                bridge1.GetComponent<Bridge1_Script>().TakeDamage(attackDamage);
            }

            Bridge2_Script bridge2;
            bridge2 = GameObject.Find("Bridge2").GetComponent<Bridge2_Script>();

            if ((GlobalEnemyCheck.isEnemySecond) && (GlobalEnemyCheck.bridge2dead == false))
            {
                bridge2.GetComponent<Bridge2_Script>().TakeDamage(attackDamage);
            }

            Bridge3_Script bridge3;
            bridge3 = GameObject.Find("Bridge3").GetComponent<Bridge3_Script>();

            if ((GlobalEnemyCheck.isEnemyThird) && (GlobalEnemyCheck.bridge3dead == false))
            {
                bridge3.GetComponent<Bridge3_Script>().TakeDamage(attackDamage);
            }

            Bridge4_Script bridge4;
            bridge4 = GameObject.Find("Bridge4").GetComponent<Bridge4_Script>();

            if ((GlobalEnemyCheck.isEnemyFourth) && (GlobalEnemyCheck.bridge4dead == false))
            {
                bridge4.GetComponent<Bridge4_Script>().TakeDamage(attackDamage);
            }


            Bridge5_Script bridge5;
            bridge5 = GameObject.Find("Bridge5").GetComponent<Bridge5_Script>();

            if ((GlobalEnemyCheck.isEnemyFifth) && (GlobalEnemyCheck.bridge5dead == false))
            {
                bridge5.GetComponent<Bridge5_Script>().TakeDamage(attackDamage);
            }
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

    private IEnumerator RandomScoreBlipStallTimeForPussies()
    {
        float framerate = UnityEngine.Random.Range(0.00001f, 0.09999f);

        yield return new WaitForSeconds(framerate);

        GlobalBlip.recentColor = color;
        OverParent.instance.setPosition(this.transform.position.x, this.transform.position.y);
        Instantiate(myScore, this.transform.position, transform.rotation);
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


}