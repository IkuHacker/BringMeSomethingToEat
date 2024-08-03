using System.Collections;
using UnityEngine;

public class FallingPlateform : MonoBehaviour
{
    public float fallDelay = 1f;
    public float destroyDelay = 2f;
    private Vector2 initialPosition;
    public SpriteRenderer[] spriteRenderers;
    public Collider2D collider;
     private float shakeAmountMax = 0.2f;
    private bool isShake;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());
        }
    }

    private void Update()
    {
        if (isShake)
        {
            transform.position = initialPosition + Random.insideUnitCircle * shakeAmountMax;
        }


    }

    private IEnumerator Fall()
    {
        isShake = true;
        yield return new WaitForSeconds(fallDelay);
        isShake = false;
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = false;
        }
        
        collider.enabled = false;
        yield return new WaitForSeconds(destroyDelay);
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.enabled = true;
        }

        collider.enabled = true;
        transform.position = initialPosition;
    }
}