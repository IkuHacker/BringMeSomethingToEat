using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text npcName;
    public Text dialogContent;
    public Animator animator;
    public bool isDialogOpen;
    

    public float speedTypeSentence;

    private Queue<string> sentences;

    public static DialogManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de DialogManager dans la sc�ne");
            return;
        }

        instance = this;

        sentences = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextSentence();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndDialog();
            return;
        }

    }

    public void StartDialog(Dialog dialogue)
    {
        isDialogOpen = true;
        npcName.text = dialogue.name;
        animator.SetBool("IsOpen", true);


        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        var sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public void EndDialog()
    {
        isDialogOpen = false;

        animator.SetBool("IsOpen", false);



    }

  

    IEnumerator TypeSentence(string sentence)
    {
        dialogContent.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogContent.text += letter;
            yield return new WaitForSeconds(speedTypeSentence);
        }
    }
}
