using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ethereal : MonoBehaviour
{
    // Start is called before the first frame update
    Collider m_collider;
    void Start()
    {
        m_collider = GetComponent<Collider>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            m_collider.enabled = false;
        }
        if(Input.GetKeyUp(KeyCode.E))
        {
            m_collider.enabled = true;
        }
        
    }
}
