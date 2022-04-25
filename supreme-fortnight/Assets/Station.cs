using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public GameObject player;
    public GameObject panel;
    public float dist;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= dist) {
            panel.SetActive(true);
            panel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = player.GetComponent<FPSController>().cardsHeld + " out of " + player.GetComponent<FPSController>().cardsNeeded + " cards obtained";
        }
        else {
            panel.SetActive(false);
        }
    }
}
