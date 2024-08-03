using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToGame : MonoBehaviour
{
    public int maxLevel;
    public int level;
    public ListTrigger listTrigger;
    void Start()
    {
        level = Random.Range(1, maxLevel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player")) 
        {
            StaeNameValue.listItem = listTrigger.listItem;
            SceneManager.LoadScene("Level" + level);
        }
    }
  
}
