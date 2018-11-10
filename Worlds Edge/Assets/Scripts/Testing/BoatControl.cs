using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class BoatControl : MonoBehaviour {
    public float drag;
    public float angularDrag;
    public float floatSpeed;
    public float rowForce;
    public float waterLevel;
    public float updrift;

    public int playerNr;

    public Transform leftRow;
    public Transform rightRow;
    public Transform frontLeft;
    public Transform frontRight;
    public Transform rearLeft;
    public Transform rearRight;
    public Transform stageMidpoint;
    public Transform torpedoLauncher;

    public GameObject torpedo;

    private Rigidbody playerRigidbody;

    public bool rdyToShoot;
    public bool rdyToReload;

    public Image imgReload;
    public Sprite sprReload;
    public Sprite sprTorpedo;
    public Sprite sprNoTorpedo;

    // Sounds
    public AudioClip torpedoSound;
    public AudioClip paddleSound;
    public float torpedoSoundVolume;
    public float paddleSoundVolume;

    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerRigidbody.drag = drag;
        playerRigidbody.angularDrag = angularDrag;
        rdyToShoot = false;
        rdyToReload = true;
        imgReload = GameObject.Find("Player" + (playerNr + 1) + "Reload").GetComponent<Image>();
        imgReload.sprite = sprNoTorpedo;

        // Sound init
        audioSource = gameObject.GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Water physics
        if (transform.position.y < 25 && Mathf.Abs(frontLeft.position.x) < 40 && Mathf.Abs(frontLeft.position.z) < 40 && frontLeft.position.y < waterLevel) playerRigidbody.AddForceAtPosition(Vector3.up * 1.3f * updrift * (waterLevel - frontLeft.position.y), frontLeft.position, ForceMode.Force);
        if (transform.position.y < 25 && Mathf.Abs(rearLeft.position.x) < 40 && Mathf.Abs(rearLeft.position.z) < 40 && rearLeft.position.y < waterLevel) playerRigidbody.AddForceAtPosition(Vector3.up * updrift * (waterLevel - rearLeft.position.y), rearLeft.position, ForceMode.Force);
        if (transform.position.y < 25 && Mathf.Abs(frontRight.position.x) < 40 && Mathf.Abs(frontRight.position.z) < 40 && frontRight.position.y < waterLevel) playerRigidbody.AddForceAtPosition(Vector3.up * 1.3f * updrift * (waterLevel - frontRight.position.y), frontRight.position, ForceMode.Force);
        if (transform.position.y < 25 && Mathf.Abs(rearRight.position.x) < 40 && Mathf.Abs(rearRight.position.z) < 40 && rearRight.position.y < waterLevel) playerRigidbody.AddForceAtPosition(Vector3.up * updrift * (waterLevel - rearRight.position.y), rearRight.position, ForceMode.Force);
        
        //Drift away from the center 
        playerRigidbody.AddForce(((transform.position - stageMidpoint.position).normalized * floatSpeed), ForceMode.Force);
    }

    private void FixedUpdate()
    {
        if(Mathf.Abs(transform.position.x) <= 40.0f && Mathf.Abs(transform.position.z) <= 40.0f)
            getInput();
    }

    

    void getInput()
    {
        if (playerNr == 0)
        {
            //Movement
            if (Input.GetButtonDown("T1RR") && Input.GetButtonDown("T1RL"))
            {
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.4f, leftRow.position, ForceMode.Impulse);
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.4f, rightRow.position, ForceMode.Impulse);
                //Sound
                audioSource.PlayOneShot(paddleSound, paddleSoundVolume);
            }
            else if (Input.GetButtonDown("T1RR"))
            {
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.3f, leftRow.position, ForceMode.Impulse);
                //Sound
                audioSource.PlayOneShot(paddleSound, paddleSoundVolume);
            }
            else if (Input.GetButtonDown("T1RL"))
            {
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.3f, rightRow.position, ForceMode.Impulse);
                //Sound
                audioSource.PlayOneShot(paddleSound, paddleSoundVolume);
            }
            //Shooting
            if ((Input.GetAxis("T1S") < -0.5f || Input.GetKeyDown("g")) && rdyToShoot)
            {
                rdyToShoot = false;
                StartCoroutine(fireTorpedo());
            }
            else if ((Input.GetAxis("T1R") > 0.5 || Input.GetKeyDown("left shift")) && rdyToReload)
            {
                rdyToReload = false;
                StartCoroutine(reload());
            }
        }
        else if (playerNr == 1)
        {
            //Movement
            if (Input.GetButtonDown("T2RR") && Input.GetButtonDown("T2RL"))
            {
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.4f, leftRow.position, ForceMode.Impulse);
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.4f, rightRow.position, ForceMode.Impulse);
                //Sound
                audioSource.PlayOneShot(paddleSound, paddleSoundVolume);
            }
            else if (Input.GetButtonDown("T2RR"))
            {
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.3f, leftRow.position, ForceMode.Impulse);
                //Sound
                audioSource.PlayOneShot(paddleSound, paddleSoundVolume);
            }
            else if (Input.GetButtonDown("T2RL"))
            {
                playerRigidbody.AddForceAtPosition(transform.forward * rowForce * 0.3f, rightRow.position, ForceMode.Impulse);
                //Sound
                audioSource.PlayOneShot(paddleSound, paddleSoundVolume);
            }
            //Shooting
            if ((Input.GetAxis("T2S") < -0.5 || Input.GetKeyDown(KeyCode.Return)) && rdyToShoot)
            {
                rdyToShoot = false;
                StartCoroutine(fireTorpedo());
            }
            else if ((Input.GetAxis("T2R") > 0.5 || Input.GetKeyDown("j")) && rdyToReload)
            {
                rdyToReload = false;
                StartCoroutine(reload());
            }
        }
        //Quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
        }
    }

    IEnumerator fireTorpedo()
    {
        imgReload.sprite = sprNoTorpedo;
        Instantiate(torpedo, torpedoLauncher.position, transform.rotation);
        //Sound
        audioSource.PlayOneShot(torpedoSound, torpedoSoundVolume);

        yield return new WaitForSeconds(3f);
        rdyToReload = true;
    }
    IEnumerator reload()
    {
        imgReload.sprite = sprReload;
        yield return new WaitForSeconds(3f);
        imgReload.sprite = sprTorpedo;
        rdyToShoot = true;
    }
    public IEnumerator respawn()
    {
        yield return new WaitForSeconds(2f);
        //scoreviewer.pointFor((playerNr + 1) & 2);
        transform.rotation = new Quaternion();
        transform.position = new Vector3(0, 18, 0);
        rdyToReload = true;
        rdyToShoot = false;
        imgReload.sprite = sprNoTorpedo;
    }

    public void playSound(AudioClip sound) {
        audioSource.clip = sound;
        audioSource.Play();
    }
}
