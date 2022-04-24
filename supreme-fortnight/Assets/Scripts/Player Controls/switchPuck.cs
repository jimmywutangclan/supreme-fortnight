using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchPuck : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool teleport = true;
    public static bool distraction = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if teleport is currently equipped then switch to distraction ability
        if (Input.GetKeyDown(KeyCode.Q) && teleport)
        {
            teleport = false;
            distraction = true;
        }

        //if distraction is currently equipped then switch to teleport ability
        else if (Input.GetKeyDown(KeyCode.Q) && distraction)
        {
            teleport = true;
            distraction = false;
        }

    }
}
