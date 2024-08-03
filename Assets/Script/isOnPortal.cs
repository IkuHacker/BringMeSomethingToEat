using UnityEngine;

public class isOnPortal : MonoBehaviour
{
    public bool isOnThePortal;
    public static isOnPortal instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de MainPlayerMovement dans la sc�ne");
            return;
        }

        instance = this;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")) 
        {
            isOnThePortal = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isOnThePortal = false;
    }
}
