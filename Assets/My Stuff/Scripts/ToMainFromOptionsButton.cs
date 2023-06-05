using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToMainFromOptionsButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }
}
