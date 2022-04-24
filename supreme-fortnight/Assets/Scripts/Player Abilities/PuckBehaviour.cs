using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool teleport = true;
    public bool distraction = false;

    public bool active = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }

    //check to see if the teleportation object is touching the ground and if it is disable gravity in rigidbody component or
    //turn off rigidbody all together and make object stationary
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezePosition;
            active = true;

        }
    }
}
