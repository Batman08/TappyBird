using UnityEngine;
using UnityEngine.UI;

public class CurrentScore : MonoBehaviour
{
    public Text scoreText;
    public Text currentScoreText;

    void OnEnable()
    {
        scoreText.text = $"{GameControl.GameControlInstance.GetScore()}";
    }
}
