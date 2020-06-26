using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenController : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI movesText;

    public void SetScore(float time, int moves)
    {
        timeText.SetText("{0:2} seconds", time);
        movesText.SetText("{0} moves", moves);
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyUp(KeyCode.Escape))
            SceneManager.LoadScene(0);    
    }
}
