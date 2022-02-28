using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    CharacterController charCtrl;

    [SerializeField] float groundSpeed = 5f;
    [SerializeField] float airSpeed = 5f;
    [SerializeField] float jumpHeight = 3f;

    Vector3 input, moveDirection;
    float gravity = Physics.gravity.magnitude;
    float yAccel;
    float yVel;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();

        yAccel = -gravity;
    }

    void Update()
    {
        input = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        if (charCtrl.isGrounded)
        {
            yVel = 0;
            if (Input.GetButtonDown("Jump")) Jump();

            moveDirection = input * groundSpeed;
            
        }
        else
        {
            moveDirection.y = 0;
            moveDirection = Vector3.Lerp(moveDirection, input * airSpeed, 0.05f);
        }

        yVel += yAccel * Time.deltaTime;
        moveDirection.y = yVel;

        charCtrl.Move(moveDirection * Time.deltaTime);
    }

    void Jump()
    {
        var initialVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        yVel = initialVelocity;
    }
}
