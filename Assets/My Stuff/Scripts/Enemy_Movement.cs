using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool firstBridge = false;

    private float targetY = 0f;
    private float targetX = 0f;

    [SerializeField] private float moveSpeed = 9f;

    
    GameObject player = GameObject.FindGameObjectWithTag("Player");

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //dirY = -1;
        
        if (firstBridge == true)
        {
            if (this.transform.position.x < Global.playerCoordinatesX)
            {
                targetX = 1f;
            }
            else if (this.transform.position.x > Global.playerCoordinatesX)
            {
                targetX = -1f;
            }

            if (this.transform.position.y < Global.playerCoordinatesY)
            {
                targetY = 1f;
            }
            else if (this.transform.position.y > Global.playerCoordinatesY)
            {
                targetY = -1f;
            }

            rb.velocity = new Vector2(targetX * moveSpeed, targetY * moveSpeed);
        }
        else
        {
            if (this.transform.position.y > -4.55f)
            {
                rb.velocity = new Vector2(0, -1 * moveSpeed);
            }
            else
            {
                rb.velocity = Vector3.zero;
                if (this.transform.position.x < -9f)
                {
                    firstBridge = true;
                }
            }
        }
        
    }
}
