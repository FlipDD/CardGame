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

    void Start() => inputField.onValueChanged.AddListener(UpdateNameText);

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

    public void AddScore(Button button)
    {
        if (nameText.Length > 0)
        {
            GameObject scoreObj = Instantiate(score, scorePanel);
            scoreObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(nameText);
            scoreObj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("{0}", movesValue);
            scoreObj.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("{0:2}", timeValue);
            button.interactable = false;
            inputField.interactable = false;
        }
    }
}
