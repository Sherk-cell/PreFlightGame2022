using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovementAdvanced : MonoBehaviour
{


    public CharacterController CharacterController;

    public Camera PlayerCamera;
    [SerializeField] public Transform Debughitpoint;
    [SerializeField] public Transform Hookshottransform;
    private float hookshotsize;

    float shotrng = 60;

    [Header("Movement")]
    private float moveSpeed;
    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float slideSpeed;
    public float wallrunSpeed;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public States Stats;

    public Vector3 hookshotPos;

    Rigidbody rb;


    public bool IsusingCharController;


    public enum States
    {
        Normal,
        HookshotFlyingPlayer,
        hookshotthrown
    }


    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    public bool sliding;
    public bool crouching;
    public bool wallrunning;

  
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        Hookshottransform.gameObject.SetActive(false);

        startYScale = transform.localScale.y;
        Stats = States.Normal;
    }

    private void Update()
    {

        

        switch (Stats)
        {
            default:
            case States.Normal:
                grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
                HandleHookshotStart();
                MyInput();
                SpeedControl();
                StateHandler();
                TextStuff();
                Debughitpoint.gameObject.SetActive(false);

                break;

            case States.hookshotthrown:
                HandlehookshotThrow();
                Debughitpoint.gameObject.SetActive(true);

                break;
            case States.HookshotFlyingPlayer:
                HandleHookshotMovement();

                break;
        }

        // ground check
        if (Input.GetMouseButtonUp(1))
        {
            IsusingCharController = false;
            CharacterController.enabled = true;
            Debug.Log("Test");
        }

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;


        if(IsusingCharController == true)
        {
            CharacterController.enabled = true;
        }

        if(IsusingCharController == false)
        {
            CharacterController.enabled = false;
        }


    }

    private void HandleMovement()
    {
        float MoveX = Input.GetAxisRaw("Horizontal");
        float MoveZ = Input.GetAxisRaw("Vertical");
        float speeds = 20f;
        Vector3 characterVelocity = transform.right * MoveX * MoveX * speeds + transform.forward * MoveZ * speeds;



    }
    /// <summary>
    /// ///////////////////////
    /// </summary>

    #region Movement stuff

    private void FixedUpdate()
    {

        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey) && horizontalInput == 0 && verticalInput == 0)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

            crouching = true;
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);

            crouching = false;
        }
    }

    private void StateHandler()
    {
        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }

        // Mode - Sliding
        else if (sliding)
        {
            state = MovementState.sliding;

            // increase speed by one every second
            if (OnSlope() && rb.velocity.y < 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Crouching
        else if (crouching)
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }

        // check if desired move speed has changed drastically
        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());

            print("Lerp Started!");
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        // smoothly lerp movementSpeed to desired value
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void TextStuff()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

       
    }

    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }
    #endregion ///////////////////////////////////////////////////////////////////////////////////////////// ////////////////////////////////////////////////////////////////////////////////////////
    private void HandleHookshotStart()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsusingCharController = true;
            RaycastHit raycastHit;
            if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out raycastHit, shotrng))
            {



                if (raycastHit.transform.gameObject.tag == "Player")
                {
                    Debug.Log("REeh");

                }

                if (raycastHit.transform.gameObject.tag == "GrappableObject")
                {
                    Debug.Log(raycastHit.transform.name);
                    Debughitpoint.position = raycastHit.point;
                    hookshotPos = raycastHit.point;

                    Hookshottransform.gameObject.SetActive(true);
                    Hookshottransform.localScale = Vector3.zero;
                    hookshotsize = 0f;
                    Stats = States.hookshotthrown;
                }
            }

            else
            {
                Debug.Log("Out of Range");
            }


        }

    }

    private void HandlehookshotThrow()
    {
        Hookshottransform.LookAt(hookshotPos);

        float hookshotthrowspeed = 300;
        hookshotsize += hookshotthrowspeed * Time.deltaTime;
        Hookshottransform.localScale = new Vector3(1, 1, hookshotsize);

        if(hookshotsize >= Vector3.Distance(transform.position, hookshotPos))
        {
            Stats = States.HookshotFlyingPlayer;



        }
    }


    private void HandleHookshotMovement()
    {
        Hookshottransform.LookAt(hookshotPos);
        float hookshotspeedmin = 10f;
        float hookshotspeedmax = 240F;

        Vector3 hookshotdir = (hookshotPos - transform.position).normalized;

        float Hookspeed = Mathf.Clamp(Vector3.Distance(transform.position, hookshotPos), hookshotspeedmin, hookshotspeedmax);
        float hookshotMultiplier = 3f;



        CharacterController.Move(hookshotdir * Hookspeed * hookshotMultiplier * Time.deltaTime);
        float reachedhookshotposdis = 1f;
            rb.AddForce(hookshotdir * Hookspeed * hookshotMultiplier * 3.5f * Time.deltaTime, ForceMode.Impulse);
        if(Vector3.Distance(transform.position, hookshotPos) < reachedhookshotposdis)
        {
            Hookshottransform.gameObject.SetActive(false);
            Stats = States.Normal;
        }

    }




}
