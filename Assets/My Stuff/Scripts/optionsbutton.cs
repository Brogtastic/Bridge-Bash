using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class optionsbutton : MonoBehaviour
{
    private Animator anim;
    private enum MovementState { idle, forwards, backwards }
    private int mouseOver;
    private float coolCounter;
    private bool mouseOverBool;

    private bool growHappening;
    private bool shrinkHappening;


    // Start is called before the first frame update
    void Start()
    {
        mouseOverBool = false;
        mouseOver = 0;
        anim = GetComponent<Animator>();
        coolCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Backwards"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .95f)
            {
                mouseOver = 0;
            }
        }

        if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("Forwards") && (mouseOverBool == false))
        {
            mouseOver = 2;
        }

        if (mouseOver == 0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        UpdateAnimationState();
        CoolDown();
    }

    void OnMouseEnter()
    {
        mouseOverBool = true;
        if ((coolCounter <= 0f) && (mouseOver == 0))
        {
            mouseOver = 1;

            StartCoroutine(Grow());
            coolCounter = 0.01f;
        }
    }

    void onMouseOver()
    {
        mouseOverBool = true;
    }

    private void OnMouseDown()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    void OnMouseExit()
    {
        mouseOverBool = false;
        if ((coolCounter <= 0f) && (mouseOver == 1))
        {
            mouseOver = 2;
            StartCoroutine(Shrink());
            coolCounter = 0.01f;
        }
    }

    void UpdateAnimationState()
    {
        MovementState state;

        if (mouseOver == 0)
        {
            state = MovementState.idle;
        }
        else if (mouseOver == 1)
        {
            state = MovementState.forwards;
        }
        else if (mouseOver == 2)
        {
            state = MovementState.backwards;
        }
        else
        {
            state = MovementState.idle;
        }


        anim.SetInteger("state", (int)state);


    }

    private IEnumerator Grow()
    {
        if ((shrinkHappening == false) && mouseOverBool == true)
        {
            growHappening = true;
            Vector2 objectScale = transform.localScale;

            float framerateanimate = 0.05f;

            transform.localScale = new Vector2(objectScale.x + 0.01f, objectScale.y + 0.01f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x + 0.02f, objectScale.y + 0.02f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x + 0.04f, objectScale.y + 0.04f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x + 0.06f, objectScale.y + 0.06f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x + 0.07f, objectScale.y + 0.07f);
            yield return new WaitForSeconds(framerateanimate);
        }
        growHappening = false;
    }
    private IEnumerator Shrink()
    {
        if ((growHappening == false) && mouseOverBool == false)
        {
            shrinkHappening = true;
            Vector2 objectScale = transform.localScale;
            float framerateanimate = 0.05f;

            transform.localScale = new Vector2(objectScale.x - 0.01f, objectScale.y - 0.01f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x - 0.02f, objectScale.y - 0.02f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x - 0.04f, objectScale.y - 0.04f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x - 0.06f, objectScale.y - 0.06f);
            yield return new WaitForSeconds(framerateanimate);
            transform.localScale = new Vector2(objectScale.x - 0.07f, objectScale.y - 0.07f);
            yield return new WaitForSeconds(framerateanimate);
            mouseOver = 0;
        }
        shrinkHappening = false;
    }

    private void CoolDown()
    {
        if (coolCounter > -0.1f)
        {
            coolCounter -= Time.deltaTime;
        }
    }
}
