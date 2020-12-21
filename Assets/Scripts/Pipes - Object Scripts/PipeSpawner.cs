using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    public GameObject pipe;

    private const float _startTime = .2f;
    private const float _repeatRate = 2f;

    void Start()
    {

        if (GameControl.GameControlInstance.GameOver == false)
        {
            //InvokeRepeating(nameof(SpawnPipe), 1, 1.2f);
            InvokeRepeating(methodName:nameof(SpawnPipe), time:_startTime, repeatRate: _repeatRate);
        }
    }

    void Update()
    {
        if (GameControl.GameControlInstance.GameOver == true)
        {
            CancelInvoke(nameof(SpawnPipe));
        }
    }

    void SpawnPipe()
    {
        Instantiate(pipe, transform.position, Quaternion.identity, transform);
    }

}
