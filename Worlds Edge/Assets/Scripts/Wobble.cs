using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobble : MonoBehaviour {
  
    Rigidbody boat_rig;


	// Use this for initialization
	void Start ()
    {
        boat_rig = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        boat_rig.AddForce(Vector3.down * 6f);
    }
}
