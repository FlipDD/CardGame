using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshProUGUI timerCounter;
    private float time;

    void Update()
    {
        time += Time.deltaTime;
        timerCounter.SetText("{0:2}", time);
    }
}
