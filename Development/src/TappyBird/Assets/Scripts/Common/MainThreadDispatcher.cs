using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    public static MainThreadDispatcher Instance { get; private set; }

    private static readonly Queue<Action> _executionQueue = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep it alive across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Lock the queue while we read from it so background threads don't write at the same time
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                // Pull the action out of the line and execute it
                _executionQueue.Dequeue().Invoke();
            }
        }
    }

    /// <summary>
    /// Call this from any background thread to force code to run on the main thread.
    /// </summary>
    public static void Enqueue(Action action)
    {
        if (action == null) return;

        // Lock the queue while we write to it
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(action);
        }
    }
}