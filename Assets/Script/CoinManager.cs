using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CoinManager : MonoBehaviour
{
    public Text coinText;
    public int coin;

    private void Start()
    {
        StaeNameValue.coin = PlayerPrefs.GetInt("coin", 0);
    }
    private void Update()
    {

        coinText.text = StaeNameValue.coin.ToString();
    }
}
