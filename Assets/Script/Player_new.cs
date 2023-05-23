using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player_new : MonoBehaviour
{
    [Header("For Movement")] 
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float groundDrag;
    private float XDirectionalInput;
    private bool facingRight = true;
    private bool isMoving;
    private Vector3 movement;

    [Header("For Jumping")] 
    [SerializeField] float jumpForce = 8f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckPoint;
    [SerializeField] private float groundRadius;
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] private float airMultiplier;
    private bool isGrounded;
    private bool canJump = true;
    private bool pressedJump;

    [Header("For WallSliding")] 
    [SerializeField] float wallSlideSpeed = 0f;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] Transform wallCheckPoint;
    [SerializeField] private float wallRadius;
    [SerializeField] private bool isWall;
    [SerializeField] private bool isWallSliding;
    
    [Header("For WallJump")] 
    [SerializeField] float wallJumpForce = 6f;
    [SerializeField] Vector3 wallJumpAngle;
    private float wallJumpDirection = -1;
    
    [Header("Others")] 
    [SerializeField] private Rigidbody RB; 
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-5f, 1f, 0f);
        RB = this.GetComponent<Rigidbody>();
        canJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        CheckWorld();
        SpeedControl();
        WallJump();
        if (isGrounded)
        {
            RB.drag = groundDrag;
        }
        else
        {
            RB.drag = 0;
        }
    }

    void FixedUpdate()
    {
        Movement();
        WallSlide();
        
    }
    
    void Inputs()
    {
        XDirectionalInput = Input.GetAxisRaw("Horizontal");
        // Jumping
        if (Input.GetKeyDown("w"))
        {
            pressedJump = true;
            Debug.Log("Jup JUMP");
        }
        else
        {
            pressedJump = false;
        }
        if (isGrounded && canJump && pressedJump)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void Jump()
    {
        RB.velocity = new Vector3(RB.velocity.x, 0f, 0f);
        RB.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void ResetJump()
    {
        canJump = true;
    }

    void CheckWorld()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, groundRadius, groundLayer);
        isWall = Physics.CheckSphere(wallCheckPoint.position, wallRadius, wallLayer);
    }

    void Movement()
    {
        if (XDirectionalInput != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        
        movement = new Vector3(XDirectionalInput, 0f, 0f);
        
        
        if (isGrounded)
        {
            //RB.MovePosition(transform.position + (movement * moveSpeed * Time.deltaTime));  
            RB.AddForce((movement.normalized * moveSpeed * 5f), ForceMode.Force);
        }

        if (!isGrounded)
        {
            RB.AddForce((movement.normalized * moveSpeed * 5f * airMultiplier), ForceMode.Force);
        }
        
        if (XDirectionalInput < 0 && facingRight)
        {
            Flip();
        }
        else if (XDirectionalInput > 0 && !facingRight)
        {
            Flip();
        }
        
        if (transform.position.y < -10f)
        {
            transform.position = new Vector3(0, 2, 0);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(RB.velocity.x, 0f, 0f);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            RB.velocity = new Vector3(limitedVel.x, RB.velocity.y, RB.velocity.z);
        }
    }
    void Flip()
    {
        wallJumpDirection *= -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }

    void WallSlide()
    {
        if (isWall && !isGrounded)
        {
            isWallSliding = true; 
            //RB.velocity = new Vector3(0f, 0f, 0f);
            RB.velocity = new Vector3(0, wallSlideSpeed, 0f);
        }
        else
        {
            isWallSliding = false;
        }
    }

    void WallJump()
    {
        if ((isWallSliding) && canJump && pressedJump)
        {
            RB.velocity = new Vector3(0f, 0f, 0f);
            RB.AddForce(new Vector3(wallJumpForce*wallJumpDirection*wallJumpAngle.x,wallJumpForce*wallJumpAngle.y), ForceMode.Impulse);
            canJump = false;
            Invoke(nameof(ResetJump), jumpCooldown);
            
        }
    }


    
}
