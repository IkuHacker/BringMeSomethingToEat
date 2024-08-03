using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject door;
    public Animator doorAnimator;

    private void Start()
    {
        doorAnimator.SetBool("isOpen", false);

    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        doorAnimator.SetBool("isOpen", true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        doorAnimator.SetBool("isOpen", false);
    }
}
