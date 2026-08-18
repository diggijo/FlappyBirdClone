using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.CullingGroup;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private const float JUMP_FORCE = 7f;
    private const float GRAVITY_SCALE = 2f;
    private const float Y_MAX = 10f;
    private Rigidbody2D rb;
    private State state;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler OnPointScored;
    public event EventHandler OnGameOver;


    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }

    private void Awake()
    {
        Instance = this;
        state = State.WaitingToStart;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.WaitingToStart:
                {
                    if (GameInput.Instance.isJumpPressed())
                    {
                        SetState(State.Normal);
                        rb.gravityScale = GRAVITY_SCALE;
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JUMP_FORCE);
                    }
                }
                break;
            case State.Normal:
                {
                    if (GameInput.Instance.isJumpPressed())
                    {
                        rb.linearVelocity = new Vector2(rb.linearVelocity.x, JUMP_FORCE);
                    }
                }
                break;
            case State.GameOver:          
                break;
        }
    }

    private void LateUpdate()
    {
        float screenTop = Camera.main.transform.position.y + Camera.main.orthographicSize;

        if (transform.position.y > screenTop)
        {
            transform.position = new Vector3(
                transform.position.x,
                screenTop,
                transform.position.z
            );

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        SetState(State.GameOver);
        OnGameOver?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnPointScored?.Invoke(this, EventArgs.Empty);
    }

    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }
}
