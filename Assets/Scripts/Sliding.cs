using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("references")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private FirstPersonMovement fp;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slidekey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    //private bool sliding;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        fp = GetComponent<FirstPersonMovement>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slidekey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        if (Input.GetKeyUp(slidekey) && fp.sliding)
            StopSlide();
    }
    private void FixedUpdate()
    {
        if (fp.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        fp.sliding = true;

        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //sliding normal
        if(!fp.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }

        //sliding down a slope
        else
        {
            rb.AddForce(fp.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

        slideTimer -= Time.deltaTime;

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        fp.sliding = false;
        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}
