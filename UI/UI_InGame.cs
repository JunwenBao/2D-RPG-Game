using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI curHealth;
    [SerializeField] private TextMeshProUGUI maxHealth;

    [Header("Coin Amount")]
    [SerializeField] private ItemObject coin;
    [SerializeField] private TextMeshProUGUI coinAmount;

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

        coinAmount.text = Inventory.instance.getCoinAmount().ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.getMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
}
