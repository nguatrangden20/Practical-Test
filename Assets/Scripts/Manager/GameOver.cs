using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.RegisterListener(EventID.OnGameOver, (param) => GameOverMethod());
    }

    private void GameOverMethod()
    {
        LeanTween.move(gameObject.GetComponent<RectTransform>(), new Vector3(0, 0, 0), 2f);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
