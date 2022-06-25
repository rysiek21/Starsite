using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script which contains all of available items

public class ItemDatabase : MonoBehaviour
{
    #region Items

    public List<Item> items = new List<Item>()
    {
        new Item {
            id = 0,
            name = "Boots",
            description = "Movement speed",
            price = 300,
            bonusHealth = 0,
            bonusAmmo = 0,
            shootCooldown = 0,
            movementSpeed = 5f,
            requireToBuild = ""
        },
        new Item {
            id = 1,
            name = "Armor",
            description = "Gives you more HP",
            price = 300,
            bonusHealth = 50,
            bonusAmmo = 0,
            shootCooldown = 0,
            movementSpeed = 0,
            requireToBuild = ""
        },
    };

    #endregion

    public bool ItemExists(int id)
    {
        return items.Exists(x => x.id == id);
    }

    public Item GetItem(int id)
    {
        return items.Find(x => x.id == id);
    }

    /*public Item GetItem(string name)
    {
        return items.Find(x => x.name == name);
    }*/

    public List<Item> GetItems()
    {
        return items;
    }
}
