using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    public static GameControl GameControlInstance;

    public AudioSource Source;
    public GameObject GameOverText;
    public Text ScoreText;
    public Text ScoreText2;
    public bool GameOver = false;
    public float ScrollSpeed;

    private int _score = 0;

    void Awake()
    {
        GameControlSingleton();
        Source = GetComponent<AudioSource>();
    }

    private GameControl GameControlSingleton()
    {
        if (GameControlInstance == null)
            GameControlInstance = this;
        else if (GameControlInstance != this)
            Destroy(gameObject);

        return GameControlInstance;
    }

    void Update()
    {
        //ReloadScene();
        ChangeTextWhenGameOver();
    }

    private void ChangeTextWhenGameOver()
    {
        if (GameOver)
        {
            ScoreText.text = null;
            ScoreText2.text = null;
        }
    }

    public void BirdScored()
    {
        if (GameOver)
            return;

        AddScore();
        ScoreText.text = $"{_score}";
    }

    private int AddScore() => _score++;

    private void EndGame()
    {
        const string playerPrefScoreString = "score";

        bool HighScoreLessThanCurrentScore = PlayerPrefs.GetInt("score") < _score;
        if (HighScoreLessThanCurrentScore)
        {
            PlayerPrefs.SetInt(playerPrefScoreString, _score);
        }

        GameOver = true;
    }

    public void BirdDied()
    {
        EndGame();
        GameOverText.SetActive(true);
        GameOver = true;
        Source.Play();
    }

    public void ReloadGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ExitGame() => SceneManager.LoadScene(0);
}
