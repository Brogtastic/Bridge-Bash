using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void Update()
    {
        transform.position = new Vector3(2.14f, 0.4f, -30f);
    }
}
