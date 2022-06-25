using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

// Script which handles player's inventory

public class Inventory : NetworkBehaviour
{
    ItemDatabase itemDatabase;
    PlayerStats playerStats;
    public readonly SyncList<Item> inventory = new SyncList<Item>();

    private void Start()
    {
        if (!isLocalPlayer) return;
        Setup();
        inventory.Callback += OnInventoryChanged;
    }
    [Command]
    void Setup()
    {
        playerStats = GetComponent<PlayerStats>();
        itemDatabase = GameObject.Find("ItemManager").GetComponent<ItemDatabase>();
    }

    void OnInventoryChanged(SyncList<Item>.Operation op, int index, Item oldItem, Item newItem)
    {
        switch (op)
        {
            case SyncList<Item>.Operation.OP_ADD:
                GameObject.Find("Item" + index.ToString()).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/" + newItem.name.ToLower());
                break;
            case SyncList<Item>.Operation.OP_SET:
                GameObject.Find("Item" + index.ToString()).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/" + newItem.name.ToLower());
                break;
        }
    }

    [Command]
    public void BuyItem(int id)
    {
        if (inventory.Count < 4)
        {
            if (itemDatabase.ItemExists(id))
            {
                Item item = itemDatabase.GetItem(id);
                if (!inventory.Contains(item))
                {
                    if (playerStats.money >= item.price)
                    {
                        if (item.requireToBuild == "")
                        {
                            playerStats.money -= item.price;
                            SrvAddItem(item);
                        }
                    }
                }
            }
        }
    }

    [Server]
    public void SrvAddItem(Item item)
    {
        inventory.Add(item);
    }

    void GetItems()
    {
        foreach (Item item in inventory)
        {
            Debug.Log(item.name);
        }
    }
}
