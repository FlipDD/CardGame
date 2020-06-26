[System.Serializable]
public class RoundData
{
    public float time;
    public int moves;
    public string name = "";
    
    public RoundData(string nameText, int movesValue, float timeValue)
    {
        name = nameText;
        moves = movesValue; 
        time = timeValue;
    }
}
