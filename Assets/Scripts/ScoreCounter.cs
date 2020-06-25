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

    // private
    private int numberOfPairs;
    private float time;

    // Start with update disabled
    void Start() => enabled = false;

    // Set the time UI
    void Update()
    {
        time += Time.deltaTime;
        timerCounter.SetText("{0:2}", time);
    }

    // Add +1 to the score
    public void AddPairScore()
    {
        numberOfPairs++;
        scoreCounter.SetText("{0}", numberOfPairs);
    }
}
