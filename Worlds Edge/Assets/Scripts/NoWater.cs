using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class NoWater : MonoBehaviour {

    [SerializeField]
    Transform b1;
    [SerializeField]
    float magn = 5;
	// Use this for initialization
	void Start () {
		
       
	}
	
	// Update is called once per frame
	void Update () {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector3[] vertices = mesh.vertices;
        int i = 0;
        while (i < vertices.Length)
        {
            if((b1.position - mesh.vertices[i]).magnitude < magn)
            {
                vertices[i] = new Vector3(0,-100,0);
            }
            i++;
        }
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
    }
}
