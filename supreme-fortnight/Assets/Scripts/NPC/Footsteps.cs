using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{

    public AudioSource source;
    public AudioClip active;
    public AudioClip walkSound;
    public AudioClip runSound;
    public float timeSince;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Walk() {
        AudioClip clip = active;
        source.PlayOneShot(active);
    }

    // Update is called once per frame
    void Update()
    {
        timeSince += Time.deltaTime;
    }
}
