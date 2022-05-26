using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{   [SerializeField]
    private TextMeshProUGUI _scoreText;
    private int score = 0;

    [SerializeField]
    private TextMeshProUGUI _timeText;


    void Start()
    {
        this.RegisterListener(EventID.OnExplosion, (param) => UpdateScore((List<Tile>)param));
    }

    private void UpdateScore(List<Tile> param)
    {
        score += param.Count;
        _scoreText.text = score.ToString();
    }

    void Update()
    {
        _timeText.text = ((int)Time.time).ToString();
    }
}
