using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : MonoBehaviour
{
    public GameObject player;
    public GameObject panel;
    public float dist;

    public AudioClip sfx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= dist) {
            panel.SetActive(true);
            panel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Press C to collect card";

            if(Input.GetKey(KeyCode.C))
            {
                panel.SetActive(false);
                player.GetComponent<FPSController>().source.PlayOneShot(sfx);
                player.GetComponent<FPSController>().cardsHeld++;
                Destroy(this.gameObject);
            }
        }
        else {
            panel.SetActive(false);
        }
    }
}
