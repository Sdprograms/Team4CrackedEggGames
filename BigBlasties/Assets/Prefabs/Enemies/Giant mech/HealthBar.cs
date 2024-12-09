using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float HP, float MaxHP)
    {
        slider.value = HP/MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
