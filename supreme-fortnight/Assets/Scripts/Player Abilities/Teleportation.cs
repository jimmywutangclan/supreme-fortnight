using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject puckPrefab;
    public float speed = 20f;
    bool isPuckThrown = false;
    GameObject projectile;
    public GameObject camera;
    CharacterController controller;
    bool teleport = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)){
            teleport = true;

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            teleport = false;
        }
        
        //throw the puck
        if (Input.GetButtonDown("Fire1") && !(isPuckThrown) && teleport)
        {
            RaycastHit hit;
            if (!Physics.Raycast(transform.position, transform.forward, out hit, 1)) {
                projectile = Instantiate(puckPrefab, camera.transform.position + camera.transform.forward, camera.transform.rotation) as GameObject;
                var rb = projectile.GetComponent<Rigidbody>();
                rb.AddForce(camera.transform.forward * speed, ForceMode.VelocityChange);
                isPuckThrown = true;
            }
         
        }

        //if the puck is thrown teleport the player to the pucks location and delete puck
        else if (Input.GetKeyDown(KeyCode.R) && isPuckThrown && projectile.GetComponent<PuckBehaviour>().active)
        {
            controller.enabled = false;
            transform.position = projectile.transform.position;
            Destroy(projectile);
            isPuckThrown = false;
            controller.enabled = true;
            projectile.GetComponent<PuckBehaviour>().active = false;
        }

        //if player right clicks then spawn the puck at their feet
        else if(Input.GetMouseButtonDown(1) && !(isPuckThrown) && teleport)
        {
            projectile = Instantiate(puckPrefab, transform.position, camera.transform.rotation) as GameObject;
            isPuckThrown = true;
        }

        //if the player doesnt want to teleport to that spot remove the puck so they can throw it again
        else if(Input.GetKeyDown(KeyCode.V) && isPuckThrown)
        {
            Destroy(projectile);
            isPuckThrown = false;

        }



    }

   
}

