using System.Collections;
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
        Debug.Log(scorePanel.childCount);
        // foreach (Transform child in scorePanel)
        // {
        //     Debug.Log("Des");
        //     Destroy(child);
        // }

        var cardData = DataSaver.LoadFile();
        if (cardData != null)
        {
            foreach (var card in cardData)
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
            var cardData = new CardData(nameText, movesValue, timeValue);
            DataSaver.SaveFile(cardData);
            AddScoreToLeaderboard(cardData);
            button.interactable = false;
            inputField.interactable = false;
        }
    }

    public void AddScoreToLeaderboard(CardData cardData)
    {
        GameObject scoreObj = Instantiate(score, scorePanel);
        scoreObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(cardData.name);
        scoreObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("{0}", cardData.moves);
        scoreObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("{0:2}", cardData.time);
    }
}
