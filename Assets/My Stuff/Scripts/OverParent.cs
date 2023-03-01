using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverParent : MonoBehaviour
{
    public static OverParent instance;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void setPosition(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }
}
