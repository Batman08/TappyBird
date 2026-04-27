using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloaderManager : MonoBehaviour
{
    private void Awake()
    {
        AdManager.Instance.OnInitialized += () =>
        {
            MainThreadDispatcher.Enqueue(() =>
            {
                SceneManager.LoadScene(sceneName: SceneNameConstants.StartSceneName); ;
                Log.Info("Loading start scene.");
            });
        };
    }
}
