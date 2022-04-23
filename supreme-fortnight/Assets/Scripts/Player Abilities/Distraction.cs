using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject puckPrefab;
    public float speed = 20f;
    bool isPuckThrown = false;
    GameObject projectile;
    public GameObject camera;
    CharacterController controller;
    public float distractionCooldown = 10;
    float currentCooldownTime = 0;
    public float distractionLifespan = 8;
    float distractionTimer = 0;
    bool distraction = false;
    bool isCooldown = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            distraction = false;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            distraction = true;
        }

        //throw the puck
        if (Input.GetButtonDown("Fire1") && !(isPuckThrown) && distraction && !(isCooldown))
        {
            projectile = Instantiate(puckPrefab, camera.transform.position + camera.transform.forward, camera.transform.rotation) as GameObject;
            var rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(camera.transform.forward * speed, ForceMode.VelocityChange);
            isPuckThrown = true;

        }

        //if player right clicks then spawn the puck at their feet
        else if (Input.GetMouseButtonDown(1) && !(isPuckThrown) && distraction)
        {
            projectile = Instantiate(puckPrefab, transform.position, camera.transform.rotation) as GameObject;
            isPuckThrown = true;
        }

        //if the puck is thrown add to the distraction timer
        if (isPuckThrown)
        {
            distractionTimer += Time.deltaTime;

        }

        //if countdown reaches pucksLifespan then delete puck
        if (distractionTimer >= distractionLifespan)
        {
            Destroy(projectile);
            isPuckThrown = false;
            distractionTimer = 0;
            isCooldown = true;

        }

        //if the puck is on cooldown add to the cooldown timer
        if(isCooldown)
        {
            currentCooldownTime += Time.deltaTime;
        }

        //if the current cooldown timer reaches the cooldown time then let player throw puck again
        if(currentCooldownTime >= distractionCooldown)
        {
            isCooldown = false;
            currentCooldownTime = 0;
        }



    }

}
