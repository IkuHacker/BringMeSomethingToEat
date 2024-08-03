using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpItem : MonoBehaviour
{
    public ItemData item;
    private bool isInRange;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)  && isInRange)
        {
            TakeItem();
        }
    }

    void TakeItem()
    {

        if (!Inventory.instance.isFull())
        {
            Inventory.instance.AddItem(item);
            Destroy(gameObject);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

}
