using UnityEngine;


public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;
    public bool IsInRange;

  

    public static DialogTrigger instance;


    private void Awake()
    {

        if (instance != null)
        {

            return;
        }

        instance = this;


    }



    void Update()
    {
        if (IsInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialog();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IsInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            IsInRange = false;
            DialogManager.instance.EndDialog();


        }
    }


    public void TriggerDialog()
    {
        DialogManager.instance.StartDialog(dialog);


    }
}


