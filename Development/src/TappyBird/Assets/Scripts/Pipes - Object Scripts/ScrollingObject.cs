using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = new Vector2(GameControl.GameControlInstance.ScrollSpeed, 0);
    }

    void OnEnable()
    {
        Events_Subscribe();
    }

    private void OnDisable()
    {
        Events_Unsubscribe();
    }

    void Update()
    {
        if (GameControl.GameControlInstance.GameOver == true)
        {
            rb2d.linearVelocity = Vector2.zero;
        }
    }

    #region Events

    private void Events_Subscribe()
    {
        GameControl.GameControlInstance.OnResetScrollingObject += EventListener_OnResetScrollingObject;
    }

    private void Events_Unsubscribe()
    {
        GameControl.GameControlInstance.OnResetScrollingObject -= EventListener_OnResetScrollingObject;
    }

    private void EventListener_OnResetScrollingObject()
    {
        rb2d.linearVelocity = new Vector2(GameControl.GameControlInstance.ScrollSpeed, 0);
    }

    #endregion
}
