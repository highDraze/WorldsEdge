using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torpedo : MonoBehaviour
{
    public float torpedoSpeed;
    public float force;
    private bool kinematic;
    private Rigidbody tRigidbody;
    public GameObject pExploooooosion;

    // Use this for initialization
    void Start ()
	{
	    tRigidbody = gameObject.GetComponent<Rigidbody>();
        kinematic = true;
    }
	
	// Update is called once per frame
    private void Update ()
    {
        if (kinematic)
        {
           tRigidbody.AddForce(transform.forward * torpedoSpeed);
        }
        if (transform.position.y < -20)
            Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
           
            GameObject plosion = Instantiate(pExploooooosion, other.ClosestPoint(gameObject.transform.position), Quaternion.identity);
            plosion.transform.SetParent(other.transform);

            Destroy(gameObject);
            other.attachedRigidbody.AddForceAtPosition(transform.forward * force, transform.position, ForceMode.Impulse);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            tRigidbody.constraints = RigidbodyConstraints.None;
            this.kinematic = false;
            tRigidbody.useGravity = true;
            tRigidbody.AddExplosionForce(100,tRigidbody.velocity,100);
            tRigidbody.isKinematic = false;
            
        }
    }
}
