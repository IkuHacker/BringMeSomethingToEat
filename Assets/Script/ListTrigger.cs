using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTrigger : MonoBehaviour
{
    public List<ItemData> possibleItem = new List<ItemData>();
    public List<ItemData> listItem = new List<ItemData>();
    public int maxCountImePossible;

    public DialogTrigger dialogTrigger;

    void Start()
    {
        int possibleItemCount = Random.Range(1, maxCountImePossible);
        for (int i = 0; i < possibleItemCount; i++)
        {

            int indexItem = Random.Range(0, maxCountImePossible);
            listItem.Add(possibleItem[indexItem]);
            

        }

        string requiredItemsString = GetRequiredItemsAsString(listItem);
        dialogTrigger.dialog.sentences.Add(requiredItemsString);
    



    }

   
    private string GetRequiredItemsAsString(List<ItemData> items)
    {
        string itemsString = "";
        foreach (ItemData item in items)
        {
            itemsString += "- " + item.itemName + "\n"; // Assumant que ItemData a un champ itemName
        }
        return itemsString.TrimEnd('\n'); // Retire la dernière nouvelle ligne
    }

    
}
