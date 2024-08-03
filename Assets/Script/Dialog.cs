using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Dialog
{
    public string name;
    [TextArea(5, 5)]
    public List<string> sentences = new List<string>();


}
