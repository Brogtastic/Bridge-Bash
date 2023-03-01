using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Animator camAnim;

    public void camShake()
    {
        camAnim.SetTrigger("Shake");
    }

    public void camShake2()
    {
        camAnim.SetTrigger("Shake2");
    }

    public void camShake3()
    {
        camAnim.SetTrigger("Shake3");
    }
}
