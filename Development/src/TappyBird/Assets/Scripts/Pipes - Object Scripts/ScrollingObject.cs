using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = new Vector2(GameControl.GameControlInstance.ScrollSpeed, 0);

        Events();
    }

    void Update()
    {
        if (GameControl.GameControlInstance.GameOver == true)
        {
            rb2d.linearVelocity = Vector2.zero;
        }
    }

    #region Events

    private void Events()
    {
        GameControl.GameControlInstance.OnResetScrollingObject += EventListener_OnResetScrollingObject;
    }

    private void EventListener_OnResetScrollingObject()
    {
        rb2d.linearVelocity = new Vector2(GameControl.GameControlInstance.ScrollSpeed, 0);
    }

    #endregion
}
