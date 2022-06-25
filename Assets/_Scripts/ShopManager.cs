using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    bool itemsLoaded = false;
    ItemDatabase itemDatabase;
    GameObject shop;
    GameObject shopPanel;
    TextMeshProUGUI descriptionText;
    PlayerStats playerStats;
    Inventory inventory;

    private void Start()
    {
        itemDatabase = GameObject.Find("ItemManager").GetComponent<ItemDatabase>();
        shopPanel = GameObject.Find("Content");
        descriptionText = GameObject.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        shop = gameObject.transform.GetChild(0).gameObject;
        shop.SetActive(false);
    }

    private void Update()
    {
        if(playerStats == null)
        {
            try
            {
                playerStats = GameObject.Find("LocalPlayerObject").GetComponent<PlayerStats>();
                inventory = GameObject.Find("LocalPlayerObject").GetComponent<Inventory>();
            }
            catch { }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            shop.SetActive(!shop.activeSelf);
            if (shop.activeSelf)
                Cursor.lockState = CursorLockMode.None;
            else
                Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = shop.activeSelf;
            if (!itemsLoaded && shop.activeSelf)
                LoadItems();
        }
    }

    void LoadItems()
    {
        int lastX = 44;
        int lastY = -55;
        int itemInRow = 0;
        List<Item> items = itemDatabase.GetItems();
        for (int q = 0; q < 15; q++)
        {
            foreach (Item item in items)
            {
                if(itemInRow == 6)
                {
                    lastY -= 100;
                    lastX = 44;
                    itemInRow = 0;
                    shopPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(0, Mathf.Abs(lastY) + 50);
                }
                GameObject itemObject = Instantiate(Resources.Load("_Prefabs/ShopItemHolder", typeof(GameObject))) as GameObject;
                itemObject.transform.SetParent(shopPanel.transform);
                itemObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("ItemSprites/" + item.name.ToLower());
                itemObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(lastX, lastY, 1);
                itemObject.GetComponent<Button>().onClick.AddListener(() => ShowItemInfo(item.id));
                lastX += 80;
                itemInRow++;
            }
        }
    }

    void ShowItemInfo(int id)
    {
        Item item = itemDatabase.GetItem(id);

        descriptionText.text = "";
        descriptionText.text += @"<color=""white""><b>" + item.name;
        if(item.bonusHealth != 0)
            descriptionText.text += "\n" + @"<color=""green"">Bonus Health: " + item.bonusHealth;
        descriptionText.text += "\n\n<color=#455a64>" + item.description;

        GameObject.Find("PriceText").GetComponent<TextMeshProUGUI>().text = item.price + " $";
        GameObject.Find("BuyButton").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() => inventory.BuyItem(item.id));
    }
}
