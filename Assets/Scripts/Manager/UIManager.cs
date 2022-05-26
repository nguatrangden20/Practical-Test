using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{   [SerializeField]
    private TextMeshProUGUI _scoreText;
    private int score = 0;

    [SerializeField]
    private TextMeshProUGUI _timeText;

    [SerializeField]
    private TextMeshProUGUI _highScore;


    void Start()
    {
        this.RegisterListener(EventID.OnExplosion, (param) => UpdateScore((List<Tile>)param));

        _highScore.text = PlayerPrefs.GetInt("score").ToString();
    }

    private void UpdateScore(List<Tile> param)
    {
        AudioManager.instance.Play("Score");
        score += param.Count;
        _scoreText.text = score.ToString();
    }

    void Update()
    {
        _timeText.text = ((int)Time.time).ToString();
    }
}
