using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Hammer
{
    public static bool fly = true;
}

public class HammerDieScript : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private bool once;
    private bool twice;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        once = false;
        twice = false;
        GetComponent<Collider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if((Global.currentHealth <= 0) && (once == false) && (Hammer.fly == true))
        {
            this.transform.position = new Vector2(Global.playerCoordinatesX - 2, Global.playerCoordinatesY) ;
            sprite.enabled = true;
            GetComponent<Collider2D>().enabled = true;
            once = true;
        }

        if((once == true) && (twice == false) && (Hammer.fly == true))
        {
            if (Global.knockFromRight == true)
            {
                var impulse = (300 * Mathf.Deg2Rad) * rb.inertia;
                rb.AddTorque(impulse, ForceMode2D.Impulse);
                rb.velocity = new Vector2(-5, 10);
            }
            if (Global.knockFromRight == false)
            {
                var impulse = (-300 * Mathf.Deg2Rad) * rb.inertia;
                rb.AddTorque(impulse, ForceMode2D.Impulse);
                rb.velocity = new Vector2(5, 10);
            }
            twice = true;
        }

    }
}
