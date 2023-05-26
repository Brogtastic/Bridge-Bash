using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverParent : MonoBehaviour
{
    public static OverParent instance;

    private string overParentColor;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        overParentColor = "red";
    }

    public void setPosition(float x, float y)
    {
        transform.position = new Vector2(x, y);
    }

}
