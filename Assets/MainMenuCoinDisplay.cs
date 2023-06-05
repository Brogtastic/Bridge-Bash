using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class CoinPositionInstructor
{
    public static int pos;
}

public class MainMenuCoinDisplay : MonoBehaviour
{

    public float coinLengthFactor;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(14.58f - CoinPositionInstructor.pos * coinLengthFactor, 7.8f);
    }
}
