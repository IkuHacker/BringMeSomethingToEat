using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private Transform playerTransform;
    public static SpawnPoint instance;

    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de SpawnPoint dans la sc�ne");
            return;
        }

        instance = this;

        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerTransform.position = gameObject.transform.position;
    }

    public void TeleportPlayerToSpawn() 
    {
        playerTransform.position = gameObject.transform.position;
    }
}
