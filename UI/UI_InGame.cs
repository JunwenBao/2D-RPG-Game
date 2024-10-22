using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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

    [Header("Mission")]
    public TextMeshProUGUI content1;
    public TextMeshProUGUI content2;

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
        if (playerStats.getCurrentHealth() <= 0) curHealth.text = "0";

        coinAmount.text = Inventory.instance.getCoinAmount().ToString();

        slider.value = playerStats.currentHealth;

        content1.text = "Skeleton: " + MissionManager.instance.num1.ToString();
        content2.text = "Mino: " + MissionManager.instance.num2.ToString();
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.getMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }
}
