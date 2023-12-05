using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    private Animator animator;
    Vector2 moveInput;
    public float walkSpeed = 3f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 9f;
    TouchingDirections touchingDirs;
    public float jumpImpulse = 9f;

    [SerializeField]
    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {

            if (_isFacingRight != value)
            {
                // flip local scale to face opposite direction 
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        Debug.Log("IsFacingRight: " + IsFacingRight);
        if (moveInput.x > 0 && !IsFacingRight)
        {
            //face right
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public bool CanMove { get { return animator.GetBool(AnimationStrings.canMove); } }

    // determines the current Movement speed via the isMoving and isRunning parameters
    public float CurrentWalkSpeed
    {
        get
        {
            if (!CanMove)
            {
                // Movement is locked
                return 0;
            }

            if (!IsMoving || touchingDirs.IsOnWall)
            {
                // Idle speed is 0 or affected by wall
                return 0;
            }

            if (!touchingDirs.IsGrounded)
            {
                // Air Movement
                return airWalkSpeed;
            }

            // Ground Movement
            return IsRunning ? runSpeed : walkSpeed;
        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;
    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirs = GetComponent<TouchingDirections>();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentWalkSpeed, rb.velocity.y);
        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    public void OnRun(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            IsRunning = true;
        }
        else if (ctx.canceled)
        {
            IsRunning = false;
        }

    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        // TODO: check if alive as well
        if (ctx.started && touchingDirs.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            animator.SetTrigger(AnimationStrings.attackTrigger);
            rb.velocity = new Vector2(moveInput.x * 0f, 0f); // Adjust the y-component if necessary
        }

    }

}
