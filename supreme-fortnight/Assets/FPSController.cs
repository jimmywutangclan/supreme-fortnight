using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    CharacterController charCtrl;
  

    [SerializeField] float groundSpeed;
    [SerializeField] float baseGroundSpeed = 5f;
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
    public GameObject deathScreenFade;
    public GameObject etherealEffect;
    
    public float deathTransitionTime = 0.5f;
    public bool freezePlayer;
    bool etherealActive;

    public Sprite ETHEREAL_ACTIVE;
    public Sprite ETHEREAL_INACTIVE;
    
    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        charCtrl.enabled = true;

        yAccel = -gravity;
        currentEtherealTime = maxEtherealTime;

        freezePlayer = false;

        groundSpeed = baseGroundSpeed;
    }

    void Update()
    {
        if (!freezePlayer) {
            input = Input.GetAxis("Horizontal") * transform.right
                + Input.GetAxis("Vertical") * transform.forward;

            etherealActive = true;
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
                groundSpeed = runFactor;
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                groundSpeed = baseGroundSpeed;
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
        }

        if (etherealActive) {
            UnityEngine.UI.Image activeImage = etherealEffect.GetComponent<UnityEngine.UI.Image>();
            etherealActiveOrNot.GetComponent<UnityEngine.UI.Image>().sprite = ETHEREAL_ACTIVE;
            activeImage.color = new Color(activeImage.color.r, activeImage.color.g, activeImage.color.b, 1 - currentEtherealTime / maxEtherealTime);
        }
        else {
            UnityEngine.UI.Image activeImage = etherealEffect.GetComponent<UnityEngine.UI.Image>();
            etherealActiveOrNot.GetComponent<UnityEngine.UI.Image>().sprite = ETHEREAL_INACTIVE;
            activeImage.color = new Color(activeImage.color.r, activeImage.color.g, activeImage.color.b, 0);
        }
        etherealTimer.GetComponent<UnityEngine.UI.Slider>().value = (currentEtherealTime / maxEtherealTime);

    }

    void Jump()
    {
        var initialVelocity = Mathf.Sqrt(2 * gravity * jumpHeight);
        yVel = initialVelocity;
    }

    public void RespawnPlayer() {
        charCtrl.enabled = false;
        transform.position = spawnPoint.transform.position;
        charCtrl.enabled = true;

        currentEtherealTime = maxEtherealTime;
        freezePlayer = false;
    }

    public void FreezePlayer() {
        freezePlayer = true;
    }

    public void UnfreezePlayer() {
        freezePlayer = false;
    }

    public void ScreenFadeToDie(float currentTime) {
        float transparency = (currentTime / deathTransitionTime);
        UnityEngine.UI.Image activeImage = deathScreenFade.GetComponent<UnityEngine.UI.Image>();
        activeImage.color = new Color(activeImage.color.r, activeImage.color.g, activeImage.color.b, transparency);
    }

    public void ScreenGoFullBlack() {
        UnityEngine.UI.Image activeImage = deathScreenFade.GetComponent<UnityEngine.UI.Image>();
        activeImage.color = new Color(activeImage.color.r, activeImage.color.g, activeImage.color.b, 1);
    }

    public void ResetDeathScreen() {
        UnityEngine.UI.Image activeImage = deathScreenFade.GetComponent<UnityEngine.UI.Image>();
        activeImage.color = new Color(activeImage.color.r, activeImage.color.g, activeImage.color.b, 0);
    }
}
