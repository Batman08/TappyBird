using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl GameControlInstance;

    public event Action OnKeepPlaying;
    public event Action OnResetPlayer;
    public event Action OnResetPipeSpawner;

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

    void Start()
    {
        EventListener_OnShowRewardedAdCompleted();
    }

    private GameControl GameControlSingleton()
    {
        if (GameControlInstance == null)
            GameControlInstance = this;
        else if (GameControlInstance != this)
            Destroy(gameObject);

        return GameControlInstance;
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
        ScoreText.text = null;
        ScoreText2.text = null;
        Source.Play();
    }

    public float GetScore() => _score;

    public void ReloadGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public void ExitGame() => SceneManager.LoadScene(0);



    #region Keep Playing Ad Logic

    private void EventListener_OnShowRewardedAdCompleted()
    {
        Log.Info("Subscribing to AdManager.Instance.OnShowRewardedAdCompleted event.");
        AdManager.Instance.OnShowRewardedAdCompleted += HandleRewardedAd_KeepPlaying;
    }

    private void HandleRewardedAd_KeepPlaying(Reward reward)
    {
        Log.Info("KEEP PLAYING!!!!");

        ScoreText2.text = "Score:";
        ScoreText.text = $"{_score}";

        GameOverText.SetActive(false);

        OnResetPlayer.Invoke();
        OnResetPipeSpawner.Invoke();

        Time.timeScale = 0f;

        //ToDo: start a UI countdown timer here

        StartCoroutine(KeepPlayingDelay());
    }

    private IEnumerator KeepPlayingDelay()
    {
        yield return new WaitForSeconds(seconds: 5f);

        Time.timeScale = 1f;
        GameOver = false;
        OnKeepPlaying.Invoke();
    }


    public void OnClickDone_KeepPlaying()
    {
        try
        {
            AdManager.Instance.ShowRewardedAd();
        }
        catch (Exception)
        {
            string errorMessage = "Reward Ad unavailable. Please try again in a moment.";

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() => {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, errorMessage, 0);
                    toastObject.Call("show");
                }));
        }  
#else
            Log.Error(errorMessage);
#endif
        }
    }

    #endregion
}