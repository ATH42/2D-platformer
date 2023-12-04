using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;

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

    // determines the current Movement speed via the isMoving and isRunning parameters
    public float CurrentWalkSpeed
    {
        get
        {
            if (IsMoving == true)
            {
                if (IsRunning)
                {
                    return runSpeed;
                }
                else
                {
                    return walkSpeed;
                }
            }
            else
            {
                return 0;
            }
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
            animator.SetBool("isMoving", value);
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
            animator.SetBool("isRunning", value);
        }
    }


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * CurrentWalkSpeed, rb.velocity.y);

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
}
