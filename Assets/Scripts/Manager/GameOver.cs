using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    void Start()
    {
        this.RegisterListener(EventID.OnGameOver, (param) => GameOverMethod());
    }

    private void GameOverMethod()
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 2f);

        AudioManager.instance.Play("GameOver");
    }

    public void PlayAgain()
    {
        SaveScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SaveScore();
        Application.Quit();
    }

    private void SaveScore()
    {
        int score = Int32.Parse(scoreText.text);

        if (PlayerPrefs.GetInt("score") < score)
        {
            PlayerPrefs.SetInt("score", score);
            PlayerPrefs.Save();
        }
    }
}
