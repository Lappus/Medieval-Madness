using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 100f;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private float horizontalInput;
    [SerializeField] private bool _isFacingRight = true;
    [SerializeField] private bool _isWall = false;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private bool isTouchingGround = false;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask _wallLayer;
    
    // Moving Variables
    public Vector3 movement;
    private float _inputBlock = 1f; 
    
    // Jumping Variables
    private float _nextJumpTime = 1f;
    private float _cooldownTime = 1f;
    private float _jumpingSpeed = 7f;
    private float _fallingSpeed = 7f;
    private bool _isWallJumping;
    private float _wallJumpingDirection;
    private float _wallJumpingTime = 0.2f;
    private float _wallJumpingCounter;
    private float _wallJumpingDuration = 5f;
    private Vector3 _wallJumpingPower = new Vector3(2f, 6f, 0f);

    private bool _isWallSliding;
    private float _wallSlidingSpeed = 10f;



    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0f, 1f, 0f);
        RB = this.GetComponent<Rigidbody>();
    }

    private bool IsWalled()
    {
        
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, _wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !isTouchingGround && horizontalInput != 0)
        {
            _isWallSliding = true;
            RB.velocity = new Vector3(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -_wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            _isWallSliding = false;
        }
    }
    // Update is called once per frame
    void Update()
    { 
        PlayerMovement();
        WallSlide();
        WallJump();
        Flip();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_bulletPrefab, transform.position + new Vector3(0f, 1f,0f), Quaternion.identity);
            _bulletPrefab.GetComponent<Rigidbody>().AddForce(transform.forward * 10);
        }
    }
    void PlayerMovement()
    {
        // Moving 
        
        horizontalInput = Input.GetAxisRaw("Horizontal");
        movement = new Vector3(horizontalInput, 0f, 0f);

        // Jumping
        if (Input.GetKeyDown("w") && isTouchingGround && _nextJumpTime < Time.time)
        { 
            RB.velocity += new Vector3(0f, _jumpingSpeed, 0f); 
            _nextJumpTime = Time.time + _cooldownTime;
        }

        if (Input.GetKeyDown("w") && isTouchingGround && _nextJumpTime < Time.time && _isWall)
        {
            _inputBlock = 1f;
        }
        
        // Forced Falling
        if (Input.GetKeyDown("s"))
        {
            RB.velocity += new Vector3(0f, -_fallingSpeed, 0f);
        }

        //Teleporting back, if you falling down
        if (transform.position.y < -10f)
        {
            transform.position = new Vector3(0, 2, 0);
        }
    } 
    private void FixedUpdate()
    {
        if (_inputBlock < 0)
        {
            MoveCharacter(movement);
        }
        else
        {
            _inputBlock -= Time.deltaTime;
        }
    }
    
    void MoveCharacter(Vector3 direction)
    {
        RB.MovePosition(transform.position + (direction * _speed * Time.deltaTime));
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "weapon")
        {
            transform.position = new Vector3(0, 2, 0);
        }

        if (collision.gameObject.tag == "Playground")
        {
            isTouchingGround = true;
        }

        if (collision.gameObject.tag == "Wall")
        {
            _isWall = true;
            Debug.Log("wand is da");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Playground")
        {
            isTouchingGround = false;
        }

        if (collision.gameObject.tag == "Wall")
        {
            _isWall = false;
        }
    }

    private void Flip()
    {
        if (_isFacingRight && horizontalInput < 0f || !_isFacingRight && horizontalInput > 0f)
        {
            _isFacingRight = !_isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    } 
    private void WallJump()
    {
        if ( _isWall)
        {
            _isWallJumping = false;
            _wallJumpingDirection = -horizontalInput;
            _wallJumpingCounter = _wallJumpingTime;
            _nextJumpTime = _wallJumpingCounter;
            if (_nextJumpTime < Time.time)
            {
                CancelInvoke(nameof(StopWallJumping));
                _nextJumpTime = 0.5f;
                _isWallJumping = true;
            }
              
        }
        else
        {
            _wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown("w") && _wallJumpingCounter > Time.deltaTime && _isWallJumping)
        {
            RB.velocity = new Vector3(_wallJumpingDirection * _wallJumpingPower.x, _wallJumpingPower.y, 0f);
            _wallJumpingCounter = 1f;

            if (transform.localScale.x != _wallJumpingDirection)
            {
                _isFacingRight = !_isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            } 
            Invoke(nameof(StopWallJumping), _wallJumpingDuration);
        }
    }  
    private void StopWallJumping()
    {
        _isWallJumping = false;
    }
    
    
}
