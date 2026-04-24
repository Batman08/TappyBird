using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipe;

    private const float _startTime = 1f;
    private const float _repeatRate = 2.6f;

    private void Start()
    {
        Init_PipeSpawner();
        Events();
    }

    private void Init_PipeSpawner()
    {
        if (GameControl.GameControlInstance.GameOver == false)
        {
            InvokeRepeating(nameof(SpawnPipe), _startTime, _repeatRate);
        }
    }

    private void Update()
    {
        StopSpawning();
    }

    private void StopSpawning()
    {
        bool hasGameEnded = GameControl.GameControlInstance.GameOver == true;
        if (hasGameEnded)
        {
            CancelInvoke(nameof(SpawnPipe));
        }
    }

    private void SpawnPipe()
    {
        Instantiate(pipe, transform.position, Quaternion.identity, transform);
    }


    #region Events

    private void Events()
    {
        GameControl.GameControlInstance.OnResetPipeSpawner += EventListener_OnResetPipeSpawner;
        GameControl.GameControlInstance.OnKeepPlaying += EventListener_OnKeepPlaying;
    }

    private void EventListener_OnResetPipeSpawner()
    {
        //remove all current pipes on screen
        foreach (Transform child in transform) Destroy(child.gameObject);
    }

    private void EventListener_OnKeepPlaying()
    {
        //restart spawner
        Init_PipeSpawner();
    }

    #endregion
}
