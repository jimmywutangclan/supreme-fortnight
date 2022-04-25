using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.N)) {
            transform.Find("StoryPanel").gameObject.SetActive(true);
        }
        else {
            transform.Find("StoryPanel").gameObject.SetActive(false);
        }
    }
}
