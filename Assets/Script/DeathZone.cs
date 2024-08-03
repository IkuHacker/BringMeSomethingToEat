using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public GameObject[] boxs;
    public float fallDelay;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            StartCoroutine(EnterDeathZone());
        }
        if (collision.transform.CompareTag("Box"))
        {
            BoxReturnToInitialPoint();
        }

    }

    

    private void Update()
    {
        boxs = GameObject.FindGameObjectsWithTag("Box");
    }
    private IEnumerator EnterDeathZone()
    {
        yield return new WaitForSeconds(fallDelay);
        SpawnPoint.instance.TeleportPlayerToSpawn();

    }

    private void BoxReturnToInitialPoint()
    {
        foreach (GameObject box in boxs)
        {
            box.GetComponent<PickUpBox>().ReturnToInitialPoint();
        }

    }
}
