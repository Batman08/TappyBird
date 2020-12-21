using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class PlayerBirdController : MonoBehaviour
{

    //Upforce original -- 125f
    public float upForce;
    public float tiltSmooth = 5f;

    [SerializeField] private bool isDead = false;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource source;
    [SerializeField] private Collider2D _collider2D;
    [SerializeField] private Transform _transform;
    [SerializeField] private float _rotationZ;
    [SerializeField] private float vectorUp;
    [SerializeField] private float vectorDown;

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

    private void Update()
    {
        Debug.Log(transform.rotation.z);
        //Movement();
    }

    private void LateUpdate()
    {
        Movement();
    } // fix scrolling pipe speed or background scrolling

    private void FixedUpdate()
    {
        ClampPos();

        //Vector2 t = transform.position;
        //t.y = Mathf.Clamp(transform.position.y, vectorDown, vectorUp);
        //t.y = new Vector2(transform.position.x, transform.position.y);
    }

    private Vector2 ClampPos()
    {
        Vector2 pos = transform.position;
        pos.y = Mathf.Clamp(transform.position.y, vectorDown, vectorUp);
        transform.position = pos;

        return transform.position;
    }

    private void Movement()
    {
        if (isDead == false)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                source.Play();
                rb2d.velocity = Vector2.zero;
                rb2d.AddForce(MovementForce());
                anim.SetTrigger("Flap");
                if (Input.GetMouseButton(0))
                {
                    _rotationZ = 25;
                    transform.rotation = RotationAmount();
                    //_transform.Rotate(RotationAmount());
                }
                print("turn");
            }



            else /*if (Input.GetMouseButtonUp(0))*/
            {
                //_transform.Rotate(ResetRotation());
                _transform.rotation = ResetRotation();
                print("reset");
            }
        }
    }
    private Vector2 MovementForce() => new Vector2(0f, upForce);

    private Quaternion RotationAmount() => Quaternion.Euler(0, 0, _rotationZ);

    private Quaternion ResetRotation() => Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, 0f), /*1.0f * */2.5f * Time.deltaTime);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.CompareTag(pipeTag))
        //{
        //    StartCoroutine(DestroyPlayer(8));
        //    _collider2D.enabled = false;
        //    rb2d.velocity = Vector2.zero;
        //    isDead = true;
        //    GameControl.GameControlInstance.BirdDied();
        //}
        PlayerDeath();
    }

    private void PlayerDeath()
    {
        StartCoroutine(DestroyPlayer(8));
        _collider2D.enabled = false;
        rb2d.velocity = Vector2.zero;
        isDead = true;
        GameControl.GameControlInstance.BirdDied();
    }

    private IEnumerator DestroyPlayer(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
