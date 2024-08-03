using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakingBox : MonoBehaviour
{
    public bool haveAnBox = false;
    public GameObject boxVisual;
    public GameObject boxPrefab;
    public Transform pickUpPoint;
    public static TakingBox instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la sc�ne");
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.B) && haveAnBox) 
        {
            boxVisual.SetActive(false);
            Instantiate(boxPrefab, pickUpPoint.position, Quaternion.identity) ;
            haveAnBox = false;

        }
    }
    public void TakeBox(GameObject box) 
    {
        if (!haveAnBox) 
        {
            Destroy(box);
            boxVisual.SetActive(true);
            haveAnBox = true;
        }
    }
}
