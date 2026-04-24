using UnityEngine;
using UnityEngine.UI;

public class HighScore : MonoBehaviour
{
    public Text scoreText;

    void OnEnable()
    {
        int currentHighscore = PlayerPrefs.GetInt("score");
        int.TryParse(scoreText.text, out int currentScore);

        int highscore = currentScore > currentHighscore ? currentScore : currentHighscore;

        scoreText.text = $"{highscore}";
    }
}
