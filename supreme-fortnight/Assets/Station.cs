using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    public GameObject player;
    public GameObject panel;
    public float dist;

    public bool levelWon;

    // Start is called before the first frame update
    void Start()
    {
        levelWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= dist) {
            panel.SetActive(true);
            if (player.GetComponent<FPSController>().cardsHeld != player.GetComponent<FPSController>().cardsNeeded) {
                panel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = player.GetComponent<FPSController>().cardsHeld + " out of " + player.GetComponent<FPSController>().cardsNeeded + " cards verified";
            }
            else {
                levelWon = true;
                panel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Main gate opened";
            }
        }
        else {
            panel.SetActive(false);
        }
    }
}
