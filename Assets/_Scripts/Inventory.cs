using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

// Script which handles player's inventory

public class Inventory : NetworkBehaviour
{
    ItemDatabase itemDatabase;
    public readonly SyncList<Item> inventory = new SyncList<Item>();

    private void Start()
    {
        itemDatabase = GameObject.Find("ItemManager").GetComponent<ItemDatabase>();
        inventory.Callback += OnInventoryChanged;
    }

    [TargetRpc]
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
    public void CmdAddItem(Item item)
    {
        if (itemDatabase.ItemExists(item.id))
        {
            if(inventory.Count < 4)
            {
                if (!inventory.Contains(item))
                {
                    inventory.Add(item);
                }
            }
        }
    }

    void GetItems()
    {
        foreach (Item item in inventory)
        {
            Debug.Log(item.name);
        }
    }

    // Tests
    
    private void Update()
    {
        if (!isLocalPlayer) return;
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("Inventory");
            CmdAddItem(itemDatabase.GetItem(0));
            CmdAddItem(itemDatabase.GetItem(1));
        }
    }
}
