using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardData
{
    public float time;
    public int moves;
    public string name = "";
    
    public CardData(string nameText, int movesValue, float timeValue)
    {
        name = nameText;
        moves = movesValue; 
        time = timeValue;
    }
}
