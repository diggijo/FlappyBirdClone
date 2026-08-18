using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerController.Instance.OnGameOver += PlayerController_OnGameOver;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (GameInput.Instance.isJumpPressed())
        {
            animator.SetBool("isFlying", true);
        }
        else
        {
            animator.SetBool("isFlying", false);
        }
    }

    private void PlayerController_OnGameOver(object sender, System.EventArgs e)
    {
        animator.SetBool("isFlying", false);
        animator.enabled = false;
    }
}
