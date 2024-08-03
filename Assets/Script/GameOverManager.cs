using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOverManager : MonoBehaviour
{
    public List<ItemData> requiredItems = new List<ItemData>();
    public List<Transform> spawnPoints = new List<Transform>();
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public Text cointWin;
    public Text cointLost;
    public int coin;

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de GameOverManager dans la scène");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        coin = PlayerPrefs.GetInt("coin", 0);
        requiredItems = StaeNameValue.listItem;
        Shuffle(requiredItems);
        Shuffle(spawnPoints);
        SpawnRequiredItems();
        if(requiredItems.Count <= 3) 
        {
            GameTimer.instance.duration = 45f;
        }else if (requiredItems.Count == 3) 
        {
            GameTimer.instance.duration = 60f;
        }
        else if (requiredItems.Count >= 3)
        {
            GameTimer.instance.duration = 75f;
        }

        GameTimer.instance.StartTimer();
    }

    public bool CheckVictoryCondition()
    {
        Inventory playerInventory = Inventory.instance;

        Dictionary<ItemData, int> requiredItemCounts = GetItemCounts(requiredItems);
        Dictionary<ItemData, int> inventoryItemCounts = GetItemCounts(playerInventory.content);

        foreach (var requiredItem in requiredItemCounts)
        {
            if (!inventoryItemCounts.ContainsKey(requiredItem.Key) || inventoryItemCounts[requiredItem.Key] < requiredItem.Value)
            {
                return false; // Un des objets requis manque ou la quantité est insuffisante
            }
        }
        return true;
    }

    public void GameOver()
    {
        PlayerMovement.instance.MovePlayer(0f, 0f);
        PlayerMovement.instance.enabled = false;

        if (CheckVictoryCondition())
        {
           
            coin += 20;
            cointWin.text = "+20";
            victoryPanel.SetActive(true);
            defeatPanel.SetActive(false);

            if (isOnPortal.instance.isOnThePortal) 
            {
                cointWin.text = "+35";
                coin += 15;
            }
        }
        else
        {
            cointLost.text = "-20";
            coin -= 20;
            defeatPanel.SetActive(true);
            victoryPanel.SetActive(false);
        }
    }

    private void SpawnRequiredItems()
    {
        for (int i = 0; i < requiredItems.Count; i++)
        {
            if (i < spawnPoints.Count)
            {
                Instantiate(requiredItems[i].prefab, spawnPoints[i].position, spawnPoints[i].rotation);
            }
            else
            {
                Debug.LogWarning("Pas assez de points de spawn pour tous les items requis");
                break;
            }
        }
    }

    public void LoadLobby() 
    {
        StaeNameValue.coin = coin;
        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainScene");
    }

    public void LoadMainMenu()
    {
        StaeNameValue.coin = coin;
        PlayerPrefs.SetInt("coin", coin);
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private Dictionary<ItemData, int> GetItemCounts(List<ItemData> items)
    {
        Dictionary<ItemData, int> itemCounts = new Dictionary<ItemData, int>();
        foreach (ItemData item in items)
        {
            if (itemCounts.ContainsKey(item))
            {
                itemCounts[item]++;
            }
            else
            {
                itemCounts[item] = 1;
            }
        }
        return itemCounts;
    }
}

