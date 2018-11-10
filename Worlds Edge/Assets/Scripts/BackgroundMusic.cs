using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class BackgroundMusic : MonoBehaviour {
    public AudioClip soundTrack;
    AudioSource audioSource;

    // Use this for initialization
    void Start() {
        AudioSource sound = gameObject.GetComponent<AudioSource>();
        sound.loop = true;
        sound.clip = soundTrack;
        sound.Play();
    }
	
	// Update is called once per frame
	void Update() {
		
	}
}
