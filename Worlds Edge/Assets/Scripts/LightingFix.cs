using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingFix : MonoBehaviour {
    void OnLevelWasLoaded(int level)
    {
    #if UNITY_EDITOR
     if (UnityEditor.Lightmapping.giWorkflowMode == UnityEditor.Lightmapping.GIWorkflowMode.Iterative) {
         DynamicGI.UpdateEnvironment();
     }
    #endif
    }

    // Use this for initialization
    void Start () {

	}

	
	// Update is called once per frame
	void Update () {
		
	}
}
