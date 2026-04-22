using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CurrentScore : MonoBehaviour
{
    public Text scoreText;
    public Text currentScoreText;

    void Start()
    {
        scoreText.text = currentScoreText.text;
    }
}
