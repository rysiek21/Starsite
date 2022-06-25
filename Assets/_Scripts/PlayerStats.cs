using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    TextMeshProUGUI healthText;
    TextMeshProUGUI moneyText;

    [Header("Stats")]
    [SerializeField][SyncVar(hook = nameof(HealthChanged))] public int health = 100;
    [SerializeField][SyncVar(hook = nameof(MoneyChanged))] public int money = 0;

    [Header("Max Stats")]
    public int maxHealth = 100;

    private void Start()
    {
        if (!isLocalPlayer) return;
        healthText = GameObject.Find("HUDHealthText").GetComponent<TextMeshProUGUI>();
        moneyText = GameObject.Find("HUDMoneyText").GetComponent<TextMeshProUGUI>();
        healthText.text = health + " HP";
        moneyText.text = money + " $";
    }

    [Server]
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            Heal();
        if (Input.GetKeyDown(KeyCode.M))
            AddMoney();
    }

    [Command]
    public void AddMoney()
    {
        money += 1000;
    }

    [Command]
    public void Heal()
    {
        health = maxHealth + GetComponent<Modifiers>().healthModifier;
    }

    [Server]
    public void Die()
    {
        gameObject.GetComponent<NetworkIdentity>().connectionToClient.Disconnect();
    }

    void HealthChanged(int _, int newHealth)
    {
        OnHealthChanged(newHealth);
    }

    void OnHealthChanged(int newHealth)
    {
        health = newHealth;
        if (!isLocalPlayer) return;
        healthText.text = health + " HP";
    }

    void MoneyChanged(int _, int newMoney)
    {
        OnMoneyChanged(newMoney);
    }

    void OnMoneyChanged(int newMoney)
    {
        money = newMoney;
        if (!isLocalPlayer) return;
        moneyText.text = money + " $";
    }
}
