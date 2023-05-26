using System.Collections;
using System.Collections.Generic;
using UnityEngine;



static class Global
{
    public static bool isPlayerFacingRight;
    public static float playerCoordinatesX;
    public static float playerCoordinatesY;
    public static float nearestEnemyPositionx;

    public static int maxHealth;
    public static int currentHealth;
    public static int highScore;

    public static float KBCounter;
    public static float KBDamageCounter;
    public static float KBTotalTime;
    public static float KBDamageTotalTime;
    public static bool knockFromRight;
    public static float invincibilityTime;
    public static float invincibilityCounter;

    public static bool isPaused;

}


public class PlayerMovement : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public int attackDamage;

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private float dirX = 0f;
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float jumpForce = 14f;

    private bool isGrounded;
    private bool isDeadGrounded;
    private bool previouslyGrounded;
    public float checkRadius;
    public LayerMask whatIsGround;
    public Transform feetPos;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    private bool isPaused;

    public float KBForce;
    public float KBDamageForce;
    public float KBForceUp;
    public float KBAttackForceUp;
    public float KBTime;
    public float KBDamageTime;
    private bool invincibility;
    public float invincibilityTime;
    private float invincibilityCounter;

    private float soundCoolCounter;
    private float soundCoolGroundCounter;
    private float pauseCoolDownCounter;

    private bool isSquatting;
    private bool dontRotate;

    private bool dontrepeat;

    public Transform attackPoint;
    public Transform attackPoint1;
    public Transform attackPoint2;
    public Transform HoverDetectorRight;
    public Transform HoverDetectorLeft;
    public Transform HoverDetectorTop;
    private bool isAttacking;
    private bool dontJump;
    public LayerMask enemyLayers;
    public LayerMask enemyHoverLayers;
    public float attackRange = 0.5f;
    public float hoverDetectRange;

    private CameraShake shake;
    private PauseTextScript pauseText;
    private RestartButton restartButton;
    private MainMenuButton mainMenuButton;
    private ResumeButton resumeButton;
    private PointScore pointScore;
    public ParticleSystem hammerDust;
    public HealthScript healthScript;

    private int groundHitStore = 0;

    [SerializeField] public AudioClip GroundHit;
    [SerializeField] public AudioClip AirHit;
    [SerializeField] public AudioClip Ouch;
    private AudioSource audioSource;
    private AudioManager audio;

    private enum MovementState { idle, running, jumping, falling, groundhit, jumphit, squat, squatup, grounddie}

    // Start is called before the first frame update
    private void Start()
    {
        isPaused = false;
        Global.isPaused = false;
        dontrepeat = true;

        audioSource = GetComponent<AudioSource>();

        shake = GameObject.FindGameObjectWithTag("ScreenShake").GetComponent<CameraShake>();
        pauseText = GameObject.Find("Pause Text").GetComponent<PauseTextScript>();
        restartButton = GameObject.Find("Restart Button Idle").GetComponent<RestartButton>();
        mainMenuButton = GameObject.Find("Main Menu Button Idle").GetComponent<MainMenuButton>();
        pointScore = GameObject.Find("ScoreText").GetComponent<PointScore>();
        resumeButton = GameObject.Find("Resume Button").GetComponent<ResumeButton>();
        audio = FindObjectOfType<AudioManager>();

        GlobalEnemyCheck.isEnemyFirst = false;
        GlobalEnemyCheck.isEnemySecond = false;
        GlobalEnemyCheck.isEnemyThird = false;
        GlobalEnemyCheck.isEnemyFourth = false;
        GlobalEnemyCheck.isEnemyFifth = false;

        GlobalEnemyCheck.bridge1dead = false;
        GlobalEnemyCheck.bridge2dead = false;
        GlobalEnemyCheck.bridge3dead = false;
        GlobalEnemyCheck.bridge4dead = false;
        GlobalEnemyCheck.bridge5dead = false;

        GlobalEnemyCheck.bridge1dead = false;

        Global.isPlayerFacingRight = true;

        Global.KBTotalTime = KBTime;
        Global.KBDamageTotalTime = KBDamageTime;
        Global.invincibilityTime = invincibilityTime;
        invincibility = false;

        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentHealth = maxHealth;
        Global.currentHealth = maxHealth;

        healthScript.SetMaxHealth(maxHealth);

        Hammer.fly = false;

    }

    private void FixedUpdate()
    {

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_Running"))
        {
            CreateHammerDust();
        }

        if(this.transform.position.y < -10)
        {
            currentHealth = -1;
        }

        if ((Global.KBCounter <= 0) && (Global.KBDamageCounter <=0))
        {
            if (isSquatting == false)
            {
                dirX = Input.GetAxisRaw("Horizontal");
                rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
            }
        }
        else if(Global.KBDamageCounter > 0)
        {
            if (Global.KBDamageCounter > (Global.KBDamageTotalTime / 2))
            {
                if (Global.knockFromRight == true)
                {
                    rb.velocity = new Vector2(-KBDamageForce, KBAttackForceUp);
                }
                if (Global.knockFromRight == false)
                {
                    rb.velocity = new Vector2(KBDamageForce, KBAttackForceUp);
                }
            }
            else
            {
                if (Global.knockFromRight == true)
                {
                    rb.velocity = new Vector2(-KBDamageForce, 0);
                }
                if (Global.knockFromRight == false)
                {
                    rb.velocity = new Vector2(KBDamageForce, 0);
                }
            }
            Global.KBDamageCounter -= Time.deltaTime;
        }
        else
        {
            if (Global.KBCounter > (Global.KBTotalTime / 2))
            {
                if (Global.knockFromRight == true)
                {
                    rb.velocity = new Vector2(-KBForce, KBForceUp);
                }
                if (Global.knockFromRight == false)
                {
                    rb.velocity = new Vector2(KBForce, KBForceUp);
                }
            }
            else
            {
                if (Global.knockFromRight == true)
                {
                    rb.velocity = new Vector2(-KBForce, 0);
                }
                if (Global.knockFromRight == false)
                {
                    rb.velocity = new Vector2(KBForce, 0);
                }
            }
            Global.KBCounter -= Time.deltaTime;
        }

        if(invincibility == true)
        {
            Global.invincibilityCounter -= Time.deltaTime;
        }
        if(Global.invincibilityCounter <= 0)
        {
            invincibility = false;
        }
        if(Global.invincibilityCounter > 0)
        {
            invincibility = true;
        }


        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_GroundHit"))
        {
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < .2f))
            {
                //UpperAttack
                GroundAttack1();
                if (soundCoolGroundCounter <= 0)
                {
                    audioSource.clip = GroundHit;
                    audioSource.volume = SFXVolume.sfxVolume * 0.15f;
                    audioSource.PlayOneShot(GroundHit);
                    soundCoolGroundCounter = 0.4f;
                }
            }
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .2f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < .60f) && (Global.KBDamageCounter <= 0))
            {
                //Stop the player from moving during time hammer is on ground
                rb.velocity = Vector3.zero;
                shake.camShake();
            }
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .2f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < .4f))
            {
                //HitBridgeAttack
                GroundAttack();
            }
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.7f))
            {
                isAttacking = true;
                dontJump = true;
            }
            else
            {
                isAttacking = false;
                dontJump = false;
            }
        }

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_JumpHit"))
        {
            
            if ((anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .10f) && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < .35f) && (currentHealth > 0))
            {
                JumpAttack();
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }

        }

        if(currentHealth <= 0)
        {
            GetComponent<Collider2D>().enabled = false;
            //audioSource.Stop();
            //Time.timeScale = 0.1f;
        }
        if((currentHealth <= 0) && (isDeadGrounded == true))
        {
            rb.velocity = Vector3.zero;
            dontRotate = true;
            Hammer.fly = true;
            if (dontrepeat == true)
            {
                StartCoroutine(DeadSounds());
                dontrepeat = false;
            }

        }
        Global.currentHealth = currentHealth;

    }

    // Update is called once per frame
    private void Update()
    {
        

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
        isDeadGrounded = Physics2D.OverlapCircle(feetPos.position, 0.1f, whatIsGround);

        if ((isGrounded == true) && (Input.GetButtonDown("Jump")) && (dontJump == false) && (currentHealth > 0))
        {
            rb.velocity = Vector2.up * jumpForce;
            jumpTimeCounter = jumpTime;
            isJumping = true;
            if (isPaused == false)
            {
                StartCoroutine(JumpStretch());
            }
        }
        if (Input.GetButton("Jump") && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }
        }
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }

        UpdateAnimationState();
        HoverEnemyCheck();
        SoundCoolDown();

        Global.playerCoordinatesX = this.transform.position.x;
        Global.playerCoordinatesY = this.transform.position.y;

        if((Input.GetButtonDown("GroundHit")) && (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_GroundHit")))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .8f)
            {
                if (groundHitStore <= 1)
                {
                    groundHitStore += 1;
                }
            }
        }

        if (((Input.GetKeyDown(KeyCode.P)) || (Input.GetKeyDown(KeyCode.Escape))) && (isPaused == true) && (pauseCoolDownCounter <= 0) && (currentHealth > 0))
        {
            resumeGame();
        }
        if (((Input.GetKeyDown(KeyCode.P)) || (Input.GetKeyDown(KeyCode.Escape))) && (isPaused == false) && (pauseCoolDownCounter <= 0) && (currentHealth > 0))
        {
            pauseGame();
        }

    }

    public void resumeGame()
    {
        pointScore.UnpauseTheMusic();
        pauseText.UnpauseThisWhore();
        restartButton.PauseRestartHide();
        mainMenuButton.MainMenuRestartHide();
        resumeButton.ResumeHide();
        isPaused = false;
        Global.isPaused = false;
        pauseCoolDownCounter = 1f;
        StartCoroutine(PauseCoolDown());
        Time.timeScale = 1f;
    }

    public void pauseGame()
    {
        pointScore.PauseTheMusic();
        pauseText.PauseThisWhore();
        restartButton.PauseRestartShow();
        mainMenuButton.MainMenuRestartShow();
        resumeButton.ResumeShow();
        isPaused = true;
        Global.isPaused = true;
        pauseCoolDownCounter = 1f;
        StartCoroutine(PauseCoolDown());
        Time.timeScale = 0f;
    }

    private IEnumerator PauseCoolDown()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        pauseCoolDownCounter = 0;
    }

    private void UpdateAnimationState()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        MovementState state;

        //anim.SetFloat("DieSpeed", 10f);

        if ((dirX > 0f) && (isAttacking == false) && (currentHealth > 0))
        {
            state = MovementState.running;
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Global.isPlayerFacingRight = true;
        }
        else if ((dirX < 0f) && (isAttacking == false) && (currentHealth > 0))
        {
            state = MovementState.running;
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            Global.isPlayerFacingRight = false;
        }
        else
        {
            state = MovementState.idle;
        }

        if((isGrounded == true) && (Input.GetKey(KeyCode.DownArrow)))
        {
            if (isPaused == false)
            {
                state = MovementState.squat;
                isSquatting = true;
            }
            
        }

        if((isSquatting == true) && (Input.GetKey(KeyCode.DownArrow) == false))
        {
            state = MovementState.squatup;
            isSquatting = false;
        }

        if ((rb.velocity.y > .0001f)  && (isGrounded == false) && (Input.GetButtonDown("GroundHit") == false))
        {
            state = MovementState.jumping;
        }
        else if((rb.velocity.y < -.0001f) && (isGrounded == false) && (Input.GetButtonDown("GroundHit") == false))
        {
            state = MovementState.falling;
        }

        if (Input.GetButtonDown("GroundHit") && isGrounded == false)
        {
            state = MovementState.jumphit;
            if (soundCoolCounter <= 0)
            {
                audioSource.clip = AirHit;
                audioSource.volume = SFXVolume.sfxVolume * 0.2f;
                audioSource.PlayOneShot(AirHit);
                soundCoolCounter = 1.5f;
            }
        }
        if ((((Input.GetButtonDown("GroundHit")) || (groundHitStore > 0)) && (isGrounded == true) && (Input.GetButton("Jump") == false)) && (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_GroundHit") == false))
        {

            dontJump = true;
            if (isPaused == false)
            {
                state = MovementState.groundhit;
            }
            if(groundHitStore > 0)
            {
                groundHitStore -= 1;

            }
        }
        else
        {
            dontJump = false;
        }

        if(groundHitStore > 1)
        {
            groundHitStore = 1;
        }

        if(state == MovementState.falling && (Input.GetButtonDown("GroundHit"))){
            state = MovementState.jumphit;
        }
        if (state == MovementState.jumping && (Input.GetButtonDown("GroundHit"))){
            state = MovementState.jumphit;
        }


        //glitch fixes
        if (state == MovementState.idle && (dirX != 0))
        {
            state = MovementState.running;
        }

        if (state != MovementState.groundhit)
        {
            dontRotate = true;
        }
        else
        {
            dontRotate = false;
        }

        if(state == MovementState.running)
        {
            dontRotate = false;
        }
        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Player_GroundHit"))
        {
            dontRotate = true;
        }

        if ((state == MovementState.running) && (dirX < 0) && (dontRotate == false) && (currentHealth > 0))
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        if ((state == MovementState.running) && (dirX > 0) && (dontRotate == false) && (currentHealth > 0))
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.G) == true)
        {
            Debug.Log("dontRotate is: " + dontRotate);
        }

        if((currentHealth <= 0) && (isDeadGrounded == true))
        {
            state = MovementState.grounddie;
        }

        anim.SetInteger("state", (int)state);
    }

    void GroundAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemies_Movement>().TakeDamage(attackDamage);
        }

        Collider2D[] hitHoverEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyHoverLayers);

        foreach (Collider2D enemy in hitHoverEnemies)
        {
            enemy.GetComponent<HoverEnemyScript>().TakeDamage(attackDamage);
        }
        
    }

    void GroundAttack1()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint1.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemies_Movement>().TakeDamage(attackDamage);
        }

        Collider2D[] spitEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in spitEnemies)
        {
            enemy.GetComponent<Enemies_Movement>().TakeDamage(attackDamage);
        }



        Collider2D[] hitHoverEnemies = Physics2D.OverlapCircleAll(attackPoint1.position, attackRange, enemyHoverLayers);
        foreach (Collider2D enemy in hitHoverEnemies)
        {
            enemy.GetComponent<HoverEnemyScript>().TakeDamage(attackDamage);
        }

        Collider2D[] spitHoverEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, enemyHoverLayers);
        foreach (Collider2D enemy in spitHoverEnemies)
        {
            enemy.GetComponent<HoverEnemyScript>().TakeDamage(attackDamage);
        }
    }

    void JumpAttack()
    {

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint1.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Global.KBCounter = Global.KBTotalTime;
            Global.nearestEnemyPositionx = enemy.transform.position.x;
            if (Global.nearestEnemyPositionx >= this.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (Global.nearestEnemyPositionx < this.transform.position.x)
            {
                Global.knockFromRight = false;
            }
            
            enemy.GetComponent<Enemies_Movement>().TakeDamage(attackDamage);
        }

        Collider2D[] sitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in sitEnemies)
        {
            Global.KBCounter = Global.KBTotalTime;
            Global.nearestEnemyPositionx = enemy.transform.position.x;
            if (Global.nearestEnemyPositionx >= this.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (Global.nearestEnemyPositionx < this.transform.position.x)
            {
                Global.knockFromRight = false;
            }

            enemy.GetComponent<Enemies_Movement>().TakeDamage(attackDamage);
        }

        Collider2D[] witEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in witEnemies)
        {
            Global.KBCounter = Global.KBTotalTime;
            Global.nearestEnemyPositionx = enemy.transform.position.x;
            if (Global.nearestEnemyPositionx >= this.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (Global.nearestEnemyPositionx < this.transform.position.x)
            {
                Global.knockFromRight = false;
            }

            enemy.GetComponent<Enemies_Movement>().TakeDamage(attackDamage);
        }





        Collider2D[] hitHoverEnemies = Physics2D.OverlapCircleAll(attackPoint1.position, attackRange, enemyHoverLayers);

        foreach (Collider2D enemy in hitHoverEnemies)
        {
            Global.KBCounter = Global.KBTotalTime;
            Global.nearestEnemyPositionx = enemy.transform.position.x;
            if (Global.nearestEnemyPositionx >= this.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (Global.nearestEnemyPositionx < this.transform.position.x)
            {
                Global.knockFromRight = false;
            }

            enemy.GetComponent<HoverEnemyScript>().TakeDamage(attackDamage);
        }

        Collider2D[] sitHoverEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyHoverLayers);

        foreach (Collider2D enemy in sitHoverEnemies)
        {
            Global.KBCounter = Global.KBTotalTime;
            Global.nearestEnemyPositionx = enemy.transform.position.x;
            if (Global.nearestEnemyPositionx >= this.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (Global.nearestEnemyPositionx < this.transform.position.x)
            {
                Global.knockFromRight = false;
            }

            enemy.GetComponent<HoverEnemyScript>().TakeDamage(attackDamage);
        }

        Collider2D[] witHoverEnemies = Physics2D.OverlapCircleAll(attackPoint2.position, attackRange, enemyHoverLayers);

        foreach (Collider2D enemy in witHoverEnemies)
        {
            Global.KBCounter = Global.KBTotalTime;
            Global.nearestEnemyPositionx = enemy.transform.position.x;
            if (Global.nearestEnemyPositionx >= this.transform.position.x)
            {
                Global.knockFromRight = true;
            }
            if (Global.nearestEnemyPositionx < this.transform.position.x)
            {
                Global.knockFromRight = false;
            }

            enemy.GetComponent<HoverEnemyScript>().TakeDamage(attackDamage);
        }


    }

    void HoverEnemyCheck()
    {
        Collider2D[] detectEnemies = Physics2D.OverlapCircleAll(HoverDetectorRight.position, hoverDetectRange, enemyHoverLayers);

        foreach (Collider2D enemy in detectEnemies)
        {
            if (enemy.GetComponent<HoverEnemyScript>().takeItFromHere == false)
            {
                enemy.GetComponent<HoverEnemyScript>().GoToPointRight();
            }
        }

        Collider2D[] wetectEnemies = Physics2D.OverlapCircleAll(HoverDetectorLeft.position, hoverDetectRange, enemyHoverLayers);

        foreach (Collider2D enemy in wetectEnemies)
        {
            if (enemy.GetComponent<HoverEnemyScript>().takeItFromHere == false)
            {
                enemy.GetComponent<HoverEnemyScript>().GoToPointLeft();
            }
        }

        Collider2D[] setectEnemies = Physics2D.OverlapCircleAll(HoverDetectorTop.position, hoverDetectRange, enemyHoverLayers);

        foreach (Collider2D enemy in setectEnemies)
        {
            if (enemy.GetComponent<HoverEnemyScript>().takeItFromHere == false)
            {
                enemy.GetComponent<HoverEnemyScript>().GoToPointTop();
            }
        }
    }

    public void TakeDamage(int damage, float enemyx )
    {
        audioSource.clip = Ouch;
        audioSource.volume = SFXVolume.sfxVolume * 0.2f;
        audioSource.PlayOneShot(Ouch);

        currentHealth -= damage;
        healthScript.SetHealth(currentHealth);

        shake.camShake3();

        Global.nearestEnemyPositionx = enemyx;

        if (currentHealth > 0)
        {
            //play hurt animation
            StartCoroutine(FlashAfterDamage());
        }

    }
    
    private IEnumerator FlashAfterDamage()
    {

        float flashDelayInvisible = 0.05f;
        float flashDelayVisible = 0.07f;

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
            yield return new WaitForSeconds(flashDelayInvisible/2f);
            sprite.enabled = true;
            yield return new WaitForSeconds(flashDelayVisible/2f);
        }
        for (int i = 0; i < 2; i++)
        {
            sprite.enabled = false;
            yield return new WaitForSeconds(flashDelayInvisible / 3f);
            sprite.enabled = true;
            yield return new WaitForSeconds(flashDelayVisible / 3f);
        }

    }
    

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null){
            return;
        }
        if (attackPoint1 == null)
        {
            return;
        }
        if (feetPos == null)
        {
            return;
        }
        if (HoverDetectorRight == null)
        {
            return;
        }
        if (HoverDetectorLeft == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(attackPoint1.position, attackRange);
        Gizmos.DrawWireSphere(feetPos.position, checkRadius);

        Gizmos.DrawWireSphere(HoverDetectorRight.position, hoverDetectRange);
        Gizmos.DrawWireSphere(HoverDetectorLeft.position, hoverDetectRange);
        Gizmos.DrawWireSphere(HoverDetectorTop.position, hoverDetectRange);
    }

    private void SoundCoolDown()
    {
        if(soundCoolCounter > -0.1f)
        {
            soundCoolCounter -= Time.deltaTime;
        }
        if (soundCoolGroundCounter > -0.1f)
        {
            soundCoolGroundCounter -= Time.deltaTime;
        }
    }

    void CreateHammerDust()
    {
        hammerDust.Play();
    }

    private IEnumerator JumpStretch()
    {
        isGrounded = false;
        // Gets the local scale of game object
        Vector2 objectScale = transform.localScale;

        float jumpstretchframerate = 0.06f;

        // Sets the local scale of game object
        transform.localScale = new Vector2(0.85f, objectScale.y + 0.2f);
        yield return new WaitForSeconds(jumpstretchframerate);
        transform.localScale = new Vector2(0.9f, objectScale.y + 0.1f);
        yield return new WaitForSeconds(jumpstretchframerate);
        transform.localScale = new Vector2(0.97f, objectScale.y + 0.03f);
        yield return new WaitForSeconds(jumpstretchframerate);
        transform.localScale = new Vector2(0.99f, objectScale.y + 0.01f);
        yield return new WaitForSeconds(jumpstretchframerate);
        transform.localScale = new Vector2(1, objectScale.y);
    }

    private IEnumerator TimePauseEffect()
    {
        float framerate = 1.4f;
        Time.timeScale = 0.7f;
        moveSpeed = moveSpeed * 1.7f;
        yield return new WaitForSeconds(framerate);
        Time.timeScale = 1f;
        moveSpeed = moveSpeed / 1.7f;
    }

    private IEnumerator DeadSounds()
    {
        yield return new WaitForSeconds(1.7f);
        if (soundCoolGroundCounter <= 0)
        {
            audioSource.clip = GroundHit;
            audioSource.volume = SFXVolume.sfxVolume * 0.2f;
            audioSource.PlayOneShot(GroundHit);
            soundCoolGroundCounter = 0.2f;
        }
        yield return new WaitForSeconds(0.5f);
        if (soundCoolGroundCounter <= 0)
        {
            audioSource.clip = GroundHit;
            audioSource.volume = SFXVolume.sfxVolume * 0.2f;
            audioSource.PlayOneShot(GroundHit);
            soundCoolGroundCounter = 0.2f;
        }

    }


}



