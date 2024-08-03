using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{



    [SerializeField]
    public List<ItemData> content = new List<ItemData>();
    public Transform inventorySlotsParent;
    const int InventorySize = 6;
    public Sprite emptySlotVisual;


    public static Inventory instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la sc�ne");
            return;
        }

        instance = this;
    }


    void Start()
    {
        RefreshContent();
    }

    public void AddItem(ItemData item) 
    {
        if (!isFull()) 
        {
            content.Add(item);
            RefreshContent();
        }
        
    }

    public void RefreshContent()
    {
        for (int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.item = null;
            currentSlot.itemVisual.sprite = emptySlotVisual;
        }

        // On peuple le visuel des slots selon le contenu réel de l'inventaire
        for (int i = 0; i < content.Count; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();

            currentSlot.item = content[i];
            currentSlot.itemVisual.sprite = content[i].visual;

        }


    }

    public bool isFull()
    {
        return InventorySize == content.Count;
    }
}
