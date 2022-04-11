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
    [SerializeField] float runFactor = 1.65f;
    [SerializeField] float etherealRechargeFactor = 0.10f;
    [SerializeField] float etherealDecayRate = 0.30f;
    public float etherealExpirationBuffer = -0.1f;
    public float etherealCooldownTime = 0.5f;

    [SerializeField] float maxEtherealTime = 8f;
    public float currentEtherealTime;
    
    public GameObject spawnPoint;

    Vector3 input, moveDirection;
    float gravity = Physics.gravity.magnitude;
    float yAccel;
    float yVel;

    public GameObject etherealTimer;
    public GameObject etherealActiveOrNot;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        charCtrl.enabled = true;

        yAccel = -gravity;
        currentEtherealTime = maxEtherealTime;
    }

    void Update()
    {
        input = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

        bool etherealActive = true;
        // managing ethereal charges
        if(Input.GetKey(KeyCode.E) && currentEtherealTime > etherealExpirationBuffer)
        {
            currentEtherealTime -= etherealDecayRate * Time.deltaTime;
            etherealActive = true;
        }
        else {
            currentEtherealTime += etherealRechargeFactor * Time.deltaTime;
            etherealActive = false;
        }
        currentEtherealTime = Mathf.Clamp(currentEtherealTime, etherealExpirationBuffer, maxEtherealTime);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            groundSpeed *= runFactor;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            groundSpeed /= runFactor;
        }

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

        etherealActiveOrNot.GetComponent<UnityEngine.UI.Text>().text = "Active: " + etherealActive;
        etherealTimer.GetComponent<UnityEngine.UI.Text>().text = "Ethereal: " + currentEtherealTime;

    }

    void Jump()
    {
        var initialVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        yVel = initialVelocity;
    }

    public void RespawnPlayer() {
        Debug.Log(transform.position + " on way to " + spawnPoint.transform.position);
        charCtrl.enabled = false;
        transform.position = spawnPoint.transform.position;
        charCtrl.enabled = true;

        currentEtherealTime = maxEtherealTime;
    }
}
