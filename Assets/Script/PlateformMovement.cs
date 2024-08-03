using System.Collections;
using UnityEngine;

public class MovementPlatform : MonoBehaviour
{
    public float speed;
    public Transform[] wayPoint;
    public float stopDuration = 2.0f; // Dur�e de l'arr�t en secondes

    private Transform target;
    private int destPoint;
    [HideInInspector]
    public bool isWaiting = false;

    void Start()
    {
        target = wayPoint[0];
    }

    void Update()
    {
        if (!isWaiting)
        {
            Vector3 dir = target.position - transform.position;
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            // Si la plateforme est quasiment arriv�e � sa destination
            if (Vector3.Distance(transform.position, target.position) < 0.3f)
            {
                StartCoroutine(WaitAtWaypoint());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       

        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    public void MoveToNextWaypoint()
    {
        destPoint = (destPoint + 1) % wayPoint.Length;
        target = wayPoint[destPoint];
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true; // La plateforme est en train d'attendre
        yield return new WaitForSeconds(stopDuration); // Attendre pendant la dur�e sp�cifi�e
        isWaiting = false; // La plateforme peut reprendre son mouvement
        MoveToNextWaypoint();
    }
}
