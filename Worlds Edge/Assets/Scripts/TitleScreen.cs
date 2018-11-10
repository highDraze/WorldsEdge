using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour {

    public GameObject heading;

	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        heading.transform.position = Vector3.MoveTowards(new Vector3(
               heading.transform.position.x,
               heading.transform.position.y,
               heading.transform.position.z), new Vector3(heading.transform.position.x,
               77,
               heading.transform.position.z), (1 + 100000/Mathf.Pow(heading.transform.position.y + 100, 2)) * 3 * Time.deltaTime);

        if (Input.GetKey("joystick button 5") || Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Ingame");
        }
    }

    
}
