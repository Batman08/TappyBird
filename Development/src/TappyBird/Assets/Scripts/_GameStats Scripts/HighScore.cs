using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HighScore : MonoBehaviour
{
    public Text scoreText;

    void Start()
    {
        scoreText.text = PlayerPrefs.GetInt("score").ToString();
    }
}
