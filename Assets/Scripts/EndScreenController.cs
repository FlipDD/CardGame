using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreenController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI movesText;
    public TMP_InputField inputField;
    public Transform scorePanel;
    public GameObject score;
    
    private float timeValue;
    private int movesValue;
    private string nameText = "";

    void Start()
    {
        var roundData = DataSaver.LoadFile();
        if (roundData != null)
        {
            foreach (var card in roundData)
                AddScoreToLeaderboard(card);
        }

        inputField.onValueChanged.AddListener(UpdateNameText);
    } 

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyUp(KeyCode.Escape))
            SceneManager.LoadScene(0);    
    }

    public void UpdateNameText(string text) => nameText = text;

    public void SetScore(float time, int moves)
    {
        timeText.SetText("{0:2} seconds", time);
        movesText.SetText("{0} moves", moves);
        timeValue = time;
        movesValue = moves;
    }

    public void AddEntry(Button button)
    {
        if (nameText.Length > 0)
        {
            var roundData = new RoundData(nameText, movesValue, timeValue);
            DataSaver.SaveFile(roundData);
            AddScoreToLeaderboard(roundData);
            button.interactable = false;
            inputField.interactable = false;
        }
    }

    public void AddScoreToLeaderboard(RoundData roundData)
    {
        GameObject scoreObj = Instantiate(score, scorePanel);
        scoreObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(roundData.name);
        scoreObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("{0}", roundData.moves);
        scoreObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("{0:2}", roundData.time);
    }
}
