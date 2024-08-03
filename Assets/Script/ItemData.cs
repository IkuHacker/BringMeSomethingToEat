using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefab;
    public Sprite visual;

}
