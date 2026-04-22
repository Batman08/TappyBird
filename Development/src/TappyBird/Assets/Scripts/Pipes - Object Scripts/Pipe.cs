using UnityEngine;

public class Pipe : MonoBehaviour
{
    public static Pipe instance;

    [SerializeField] private float speed = 0.35f; //0.0015f
    [SerializeField] private float speedIncreasePerPoint = 0.03f;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(-0.58f, 0.66f), 0);
    }

    void Update()
    {
        if (GameControl.GameControlInstance.GameOver == false)
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime);
            float currentSpeed = speed + (GameControl.GameControlInstance.GetScore() * speedIncreasePerPoint);
            transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);

        }
        else if (GameControl.GameControlInstance.GameOver == true)
        {
            transform.Translate(Vector3.zero);
        }
    }
}
