using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider _slider;
    public Gradient gradient;
    public Image fill;
    /// <summary>
    /// Sets max health of healthbar
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;

        fill.color=gradient.Evaluate(health);
    }
    /// <summary>
    /// Sets current health of healthbar
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        _slider.value = health;
        fill.color = gradient.Evaluate(_slider.normalizedValue);
    }
}
