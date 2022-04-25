using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInstructions : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.I)) {
            transform.Find("Panel").gameObject.SetActive(true);
        }
        else {
            transform.Find("Panel").gameObject.SetActive(false);
        }
    }
}
