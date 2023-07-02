using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipe;

    private const float _startTime = 1f;
    private const float _repeatRate = 1.2f;

    private void Start()
    {
        Repeat();
    }

    private void Repeat()
    {
        if (GameControl.GameControlInstance.GameOver == false)
            InvokeRepeating(nameof(SpawnPipe), _startTime, _repeatRate);
    }

    private void Update()
    {
        StopSpawning();
    }

    private void StopSpawning()
    {
        bool hasGameEnded = GameControl.GameControlInstance.GameOver == true;
        if (hasGameEnded)
            CancelInvoke(nameof(SpawnPipe));
    }

    private void SpawnPipe()
    {
        Instantiate(pipe, transform.position, Quaternion.identity, transform);
    }
}
