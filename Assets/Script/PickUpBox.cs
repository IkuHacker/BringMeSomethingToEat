using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBox : MonoBehaviour
{
    private bool isInRange;
    private Transform initialPointObject;
    public string initialPointObjectName;
    private Vector3 initialPoint;


    private void Start()
    {
        initialPointObject = GameObject.Find(initialPointObjectName).GetComponent<Transform>();
        initialPoint = initialPointObject.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) && isInRange) 
        {
            TakingBox.instance.TakeBox(gameObject);
        }
    }

    public void ReturnToInitialPoint() 
    {
        transform.position = initialPoint;
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
