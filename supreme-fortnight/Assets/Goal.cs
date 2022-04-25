using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public GameObject player;
    public GameObject panel;
    public float dist;
    public GameObject station;
    public int sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= dist) {
            panel.SetActive(true);
            if (station.GetComponent<Station>().levelWon) {
                panel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Press C to escape room";
                if(Input.GetKey(KeyCode.C)) {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
            else {
                panel.transform.Find("Text").GetComponent<UnityEngine.UI.Text>().text = "Permission Denied: Gate not activated";
            }
        }
        else {
            panel.SetActive(false);
        }
    }
}
