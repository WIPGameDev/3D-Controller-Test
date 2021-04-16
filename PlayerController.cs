using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement variables
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    private float moveSpeed;
    [SerializeField] float jumpForce;

    [SerializeField] Animator theAnim;
    Rigidbody theRB;

    //Variables for checking grounding
    private bool grounded;
    private Collider playerCol;
    [SerializeField] LayerMask whatIsGround;

    //Variables for Rotating
    [SerializeField] float rotateSpeed;
    [SerializeField] Transform pivot;

    //Variables for Anim Blend trees
    float velocityX = 0f;
    float velocityZ = 0f;

    float acceleration = 10f;
    float decceleration = 10f;
    

    private void Awake()
    {
        theRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<CapsuleCollider>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    void Update()
    {
        RunToggle();
        Move();
        Jump();
        BlendTreeCalculation();
        Quit();
    }

    private void RunToggle()
    {
        //Switches between running and walking
        if (Input.GetButton("Fire3"))
        {
            moveSpeed = runSpeed;
            theAnim.SetBool("running", true);
        }
        else
        {
            moveSpeed = walkSpeed;
            theAnim.SetBool("running", false);
        }
    }

    private void Move()
    {
        //Movement Variables
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        //Adjusts Movement for Camera & Char Rotation
        Vector3 movementInput = (transform.forward * verticalInput * moveSpeed) + (transform.right * horizontalInput * moveSpeed);

        //Normalizes Movement for Diagonal
        if (movementInput.magnitude > 0)
        {
            movementInput.Normalize();
        }

        //Animates Input
        if (horizontalInput != 0 || verticalInput != 0 && grounded)
        {
            theAnim.SetBool("moving", true);
        }
        else
        {
            theAnim.SetBool("moving", false);
        }

        //Moves the Char
        theRB.velocity = new Vector3(movementInput.x * moveSpeed, theRB.velocity.y, movementInput.z * moveSpeed);
    }

    private void Jump()
    {
        //Jump
        if (grounded && Input.GetButtonDown("Jump"))
        {
            theRB.velocity = new Vector3(theRB.velocity.x, jumpForce, theRB.velocity.z);
            theAnim.SetTrigger("jump");
        }
    }

    private void GroundCheck()
    {
        //Checks to see if grounded
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, playerCol.bounds.extents.y, whatIsGround))
        {
            grounded = true;
            theAnim.SetBool("grounded", true);
        }
        else
        {
            grounded = false;
            theAnim.SetBool("grounded", false);
        }

        Debug.DrawRay(playerCol.bounds.center, Vector3.down * (playerCol.bounds.extents.y));
    }

    private void BlendTreeCalculation()
    {
        //Bools are true or false depending on the direction the char is going in
        bool forwardPress = Input.GetKey("w") || Input.GetKey("up");
        bool backwardPress = Input.GetKey("s") || Input.GetKey("down");
        bool rightwardPress = Input.GetKey("d") || Input.GetKey("right");
        bool leftwardPress = Input.GetKey("a") || Input.GetKey("left");

        //Acceleration
        if (forwardPress && velocityX < 1f)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (rightwardPress && velocityZ < 1f)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (leftwardPress && velocityZ > -1f)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }

        if (backwardPress && velocityX > -1f)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        //Decceleration
        if (!forwardPress && velocityX > 0)
        {
            velocityX -= Time.deltaTime * decceleration;
        }

        if (!rightwardPress && velocityZ > 0)
        {
            velocityZ -= Time.deltaTime * decceleration;
        }

        if (!leftwardPress && velocityZ < 0)
        {
            velocityZ += Time.deltaTime * decceleration;
        }

        if (!backwardPress && velocityX < 0)
        {
            velocityX += Time.deltaTime * decceleration;
        }

        //Send local paramenters to Anim
        theAnim.SetFloat("velocityZ", velocityZ);
        theAnim.SetFloat("velocityX", velocityX);
    }

    public bool isGrounded()
    {
        return grounded;
    }

    private void Quit()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
