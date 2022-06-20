using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class PlayerStats : NetworkBehaviour
{
    TextMeshProUGUI healthText;

    [Header("Stats")]
    [SerializeField][SyncVar(hook = nameof(HealthChanged))] private int health = 100;

    [Header("Max Stats")]
    public int maxHealth = 100;

    private void Start()
    {
        if (!isLocalPlayer) return;
        healthText = GameObject.Find("HUDHealthText").GetComponent<TextMeshProUGUI>();
        healthText.text = health + " HP";
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
}
