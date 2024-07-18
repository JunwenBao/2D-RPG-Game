using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;

    [SerializeField] private TextMeshProUGUI curHealth;
    [SerializeField] private TextMeshProUGUI maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        if (playerStats != null)
            playerStats.onHealthChanged += UpdateHealthUI;
    }

    // Update is called once per frame
    void Update()
    {
        maxHealth.text = playerStats.getMaxHealthValue().ToString();
        curHealth.text = playerStats.getCurrentHealth().ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.getMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
}
