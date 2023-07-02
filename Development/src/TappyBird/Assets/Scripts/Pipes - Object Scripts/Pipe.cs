using UnityEngine;

public class Pipe : MonoBehaviour
{
    public static Pipe instance;

    [SerializeField] private float speed = 0.0015f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(-0.58f, 0.66f), 0);
    }

    void Update()
    {
        if (GameControl.GameControlInstance.GameOver == false)
        {
            transform.Translate(Vector3.left * speed);
        }
        else if (GameControl.GameControlInstance.GameOver == true)
        {
            transform.Translate(Vector3.zero);
        }
    }
}
