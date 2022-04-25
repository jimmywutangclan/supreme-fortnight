using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBounds : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Player") {
            GameObject player = GameObject.FindWithTag("Player");
            FPSController cont = player.GetComponent<FPSController>();
            cont.RespawnPlayer();
        }
    }
}
