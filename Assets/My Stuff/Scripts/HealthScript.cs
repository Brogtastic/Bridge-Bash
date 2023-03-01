using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        StartCoroutine(healthAnimation());
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private IEnumerator healthAnimation()
    {
        float framerate = 0.06f;

        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.1f);
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.1f);
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.15f);
        yield return new WaitForSeconds(framerate);
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.15f);
    }
}
