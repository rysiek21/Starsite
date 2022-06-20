using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Script which handles item modifiers

public class Modifiers : NetworkBehaviour
{
    Inventory playerInventory;
    [SyncVar(hook=nameof(HealthModifierChanged))] public int healthModifier = 0;
    [SyncVar] public int damageModifier = 0;
    [SyncVar(hook=nameof(MovementSpeedChanged))] public float movementSpeedModifier = 0;

    private void Start()
    {
        if (!isLocalPlayer) return;
        playerInventory = GetComponent<Inventory>();
        playerInventory.inventory.Callback += OnInventoryChanged;
    }

    [Command]
    void OnInventoryChanged(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
    {
        switch (op)
        {
            case SyncList<Item>.Operation.OP_ADD:
                movementSpeedModifier += playerInventory.inventory[index].movementSpeed;
                healthModifier += playerInventory.inventory[index].bonusHealth;
                break;
        }
    }

    #region Syncing Vars

    // Health

    void HealthModifierChanged(int _, int newValue)
    {
        OnHealthModifierChanged(newValue);
    }

    void OnHealthModifierChanged(int newHealth)
    {
        healthModifier = newHealth;
    }

    // Movement Speed

    void MovementSpeedChanged(float _, float newValue)
    {
        OnMovementSpeedChamged(newValue);
    }

    void OnMovementSpeedChamged(float newMovementSpeed)
    {
        movementSpeedModifier = newMovementSpeed;
    }

    #endregion
}
