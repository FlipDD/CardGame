using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    // public
    public TextMeshProUGUI timerCounter;
    public TextMeshProUGUI scoreCounter;
    public GameObject endScreen;

    // private
    private int numberOfPairs;
    private int numberOfMoves;
    private float time;

    // Start with update disabled
    void Start() => enabled = false;

    // Set the time UI
    void Update()
    {
        time += Time.deltaTime;
        timerCounter.SetText("{0:2}", time);
    }

    public void AddMoveScore()
    {
        numberOfMoves++;
        scoreCounter.SetText("{0}", numberOfMoves);
    }

    // Add +1 to the score
    public void AddPairScore()
    {
        numberOfPairs++;

        if (numberOfPairs == 12)
        {
            FindObjectOfType<CardGameController>().gameObject.SetActive(false);
            FindObjectOfType<PauseMenuController>().enabled = false;
            endScreen.SetActive(true);
            endScreen.GetComponent<EndScreenController>().SetScore(time, numberOfMoves);
        }
    }
}
