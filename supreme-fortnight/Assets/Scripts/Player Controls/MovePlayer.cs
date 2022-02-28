using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    CharacterController control;
    Vector3 input;
    Vector3 moveDirection;
    public float speed = 4;
    public float jumpHeight = 3;
    public float gravity = 8;
    public float airControl = 7;

    // Start is called before the first frame update
    void Start()
    {
        control = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        input = Vector3.ClampMagnitude(transform.right * moveHorizontal +  transform.forward * moveVertical, 1f);

        input *= speed;

        if (control.isGrounded) {
            moveDirection = input;

            if (Input.GetButton("Jump")) {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            }
            else {
                moveDirection.y = 0.0f;
            }
        }
        else {
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime * airControl);
        }

        control.Move(moveDirection * Time.deltaTime);
    }
}
