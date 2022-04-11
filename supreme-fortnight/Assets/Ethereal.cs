using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ethereal : MonoBehaviour
{
    // Start is called before the first frame update
    Collider m_collider;
    bool playerInside = false;

    void Start()
    {
        m_collider = GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindWithTag("Player");
        FPSController cont = player.GetComponent<FPSController>();

        // if the player holds E down, change the collider to a trigger so player can walk through it
        if(Input.GetKeyDown(KeyCode.E) && cont.currentEtherealTime > cont.etherealCooldownTime)
        {
            m_collider.isTrigger = true;
        }
        // if the player releases E, remove
        if(Input.GetKeyUp(KeyCode.E))
        {
            m_collider.isTrigger = false;
        }
        
        // if the player is still inside an object and exceeds maximum time to be inside, respawn the player
        if (playerInside && cont.currentEtherealTime < 0) {
            cont.RespawnPlayer();
        }
    }

    // This section manages whether or not the player is inside the object, it is separately managed from the above logic to enable/disable ethereal abilities since the colliders do all the work of determining.
    // if player enters the object while ethereal, set the playerInside boolean
    // no OnCollisionStay since Collider would not be solid while ethereal.
    void OnTriggerStay(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Player") {
            playerInside = true;
        }
    }

    // if player exits the collision, un-set the playerInside boolean
    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Player") {
            playerInside = false;
        }
    }

    void OnTriggerExit(Collider collisionInfo)
    {
        if (collisionInfo.tag == "Player") {
            playerInside = false;
        }
    }
}
