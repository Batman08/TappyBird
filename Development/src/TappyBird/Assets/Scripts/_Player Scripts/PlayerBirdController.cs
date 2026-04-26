using System.Collections;
using UnityEngine;

public class PlayerBirdController : MonoBehaviour
{
    //Upforce original -- 125f
    public float upForce; // Inspector: 100f
    public float tiltSmooth = 5f; // Inspector: 5f

    [SerializeField] private bool isDead = false;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource source;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _rotationZ; // Inspector: 0f
    [SerializeField] private float vectorUp; // Inspector: 1.26f
    [SerializeField] private float vectorDown; // Inspector: -1.0f

    private const string pipeTag = "Pipe";

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _transform = GetComponent<Transform>();
        _rotationZ = _transform.rotation.z;
    }

    private void Start()
    {
        _rotationZ = Mathf.Clamp(transform.rotation.z, 0, 25);
    }

    void OnEnable()
    {
        Events_Subscribe();
    }

    private void OnDisable()
    {
        Events_Unsubscribe();
    }

    private void LateUpdate()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        ClampPos();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerDeath();
    }


    #region PositionContraints

    private Vector2 ClampPos()
    {
        Vector2 pos = transform.position;
        pos.y = Mathf.Clamp(transform.position.y, vectorDown, vectorUp);
        transform.position = pos;

        return transform.position;
    }

    #endregion


    #region Movement

    private void Movement()
    {
        if (isDead == false)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                source.Play();
                rb2d.linearVelocity = Vector2.zero;
                rb2d.AddForce(MovementForce());
                anim.SetTrigger("Flap");
                if (Input.GetMouseButton(0))
                {
                    _rotationZ = 25;
                    transform.rotation = RotationAmount();
                }
            }

            else
            {
                _transform.rotation = ResetRotation();
            }
        }
    }

    private Vector2 MovementForce() => new Vector2(0f, upForce);

    private Quaternion RotationAmount() => Quaternion.Euler(0, 0, _rotationZ);

    private Quaternion ResetRotation() => Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), /*1.0f * */2.5f * Time.deltaTime);

    #endregion


    #region PlayerDeath

    private void PlayerDeath()
    {
        StartCoroutine(DestroyPlayer(time: 4f));
        _collider2D.enabled = false;
        rb2d.linearVelocity = Vector2.zero;
        isDead = true;
        GameControl.GameControlInstance.BirdDied();
    }

    private IEnumerator DestroyPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        rb2d.bodyType = RigidbodyType2D.Kinematic;
    }

    #endregion


    #region Events

    private void Events_Subscribe()
    {
        GameControl.GameControlInstance.OnResetPlayer += EventListener_OnResetPlayer;
        GameControl.GameControlInstance.OnKeepPlaying += EventListener_OnKeepPlaying;
    }

    private void Events_Unsubscribe()
    {
        GameControl.GameControlInstance.OnResetPlayer -= EventListener_OnResetPlayer;
        GameControl.GameControlInstance.OnKeepPlaying -= EventListener_OnKeepPlaying;
    }

    private void EventListener_OnResetPlayer()
    {
        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0f;

        rb2d.bodyType = RigidbodyType2D.Kinematic;

        transform.position = new Vector2(-0.52f, 0.8f);
        transform.rotation = Quaternion.identity;
    }

    private void EventListener_OnKeepPlaying()
    {
        _collider2D.enabled = true;
        isDead = false;
        rb2d.bodyType = RigidbodyType2D.Dynamic;
    }

    #endregion
}
