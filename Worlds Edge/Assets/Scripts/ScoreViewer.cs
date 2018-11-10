using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreViewer : MonoBehaviour {

    // = GameObject.Find("Player2Win (3)").GetComponent<Image>();
    public Image imgWin1P1;
    public Image imgWin2P1;
    public Image imgWin3P1;
    public Image imgWin1P2;
    public Image imgWin2P2;
    public Image imgWin3P2;
    public Image gewinnt;
    public Sprite sprWin;
    public Sprite sprNoWin;
    public Sprite sprLose;
    public Sprite s1;
    public Sprite s2;
    private int winsP1 = 0;
    private int winsP2 = 0;

    public GameObject Player1;
    public GameObject Player2;

    private bool stopped = false;
    public int mode = 0;

    // Use this for initialization
    void Start () {
         imgWin1P1 = GameObject.Find("Player1Win (1)").GetComponent<Image>();
         imgWin2P1 = GameObject.Find("Player1Win (2)").GetComponent<Image>();
         imgWin3P1 = GameObject.Find("Player1Win (3)").GetComponent<Image>();
         imgWin1P2 = GameObject.Find("Player2Win (1)").GetComponent<Image>();
         imgWin2P2 = GameObject.Find("Player2Win (2)").GetComponent<Image>();
         imgWin3P2 = GameObject.Find("Player2Win (3)").GetComponent<Image>();
         gewinnt = GameObject.Find("gewinnt").GetComponent<Image>();
         gewinnt.fillAmount = 0;
         imgWin1P1.sprite = imgWin2P1.sprite = imgWin3P1.sprite = imgWin1P2.sprite = imgWin2P2.sprite = imgWin3P2.sprite = sprNoWin;
    }
	
	// Update is called once per frame
	void Update () {
		if(Player1.transform.position.y < 0 && !stopped)
        {
            pointFor(2);
        }
        else if(Player2.transform.position.y < 0 && !stopped)
        {
            pointFor(1);
        }
        if(Input.GetKeyDown("joystick button 7"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
        }
	}

    public void pointFor(int player)
    {
        stopped = true;
        /*
        if(mode == 1)
        {
            if (player == 1)
            {
                StartCoroutine(Player2.GetComponent<BoatControl>().respawn());
                winsP1++;
                if (winsP1 + winsP2 != 4)
                {
                    if (winsP1 + winsP2 == 1)
                    {
                        imgWin1P1.sprite = sprWin;
                        imgWin1P2.sprite = sprLose;
                    }
                    else if (winsP1 + winsP2 == 2)
                    {
                        imgWin2P1.sprite = sprWin;
                        imgWin2P2.sprite = sprLose;
                        if (winsP1 == 2) StartCoroutine(winner(1));
                    }
                    else
                    {
                        imgWin3P1.sprite = sprWin;
                        imgWin3P2.sprite = sprLose;
                        StartCoroutine(winner(1));
                    }
                }

            }
            if (player == 2)
            {
                StartCoroutine(Player1.GetComponent<BoatControl>().respawn());
                winsP2++;
                if (winsP1 + winsP2 != 4)
                {
                    if (winsP1 + winsP2 == 1)
                    {
                        imgWin1P2.sprite = sprWin;
                        imgWin1P1.sprite = sprLose;
                    }
                    else if (winsP1 + winsP2 == 2)
                    {
                        imgWin2P2.sprite = sprWin;
                        imgWin2P1.sprite = sprLose;
                        if (winsP2 == 2) StartCoroutine(winner(2));
                    }
                    else
                    {
                        imgWin3P2.sprite = sprWin;
                        imgWin3P1.sprite = sprLose;
                        StartCoroutine(winner(2));
                    }
                }


            }
        }
        else*/
        {
            if (player == 1)
            {
                StartCoroutine(Player2.GetComponent<BoatControl>().respawn());
                winsP1++;
                if (winsP1 != 4)
                {
                    if (winsP1 == 1)
                    {
                        imgWin1P1.sprite = sprWin;
                    }
                    else if (winsP1 == 2)
                    {
                        imgWin2P1.sprite = sprWin;
                    }
                    else if (winsP1 == 03)
                    {
                        imgWin3P1.sprite = sprWin;
                        StartCoroutine(winner(1));
                    }
                }

            }
            if (player == 2)
            {
                StartCoroutine(Player1.GetComponent<BoatControl>().respawn());
                winsP2++;
                if (winsP2 != 4)
                {
                    if (winsP2 == 1)
                    {
                        imgWin1P2.sprite = sprWin;
                    }
                    else if (winsP2  == 2)
                    {
                        imgWin2P2.sprite = sprWin;
                    }
                    else if(winsP2 == 03)
                    {
                        imgWin3P2.sprite = sprWin;
                        StartCoroutine(winner(2));
                    }
                }
            }
        }
        
        StartCoroutine(free());
    }
    IEnumerator free()
    {
        yield return new WaitForSeconds(4f);
        stopped = false;
    }
    IEnumerator winner(int player)
    {
        yield return new WaitForSeconds(2f);
        if (player == 1) gewinnt.sprite = s1;
        else gewinnt.sprite = s2;
        gewinnt.fillAmount = 100;
        yield return new WaitForSeconds(3f);
        //UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
        imgWin1P1.sprite = imgWin2P1.sprite = imgWin3P1.sprite = imgWin1P2.sprite = imgWin2P2.sprite = imgWin3P2.sprite = sprNoWin;
        gewinnt.fillAmount = 0;
        winsP1 = winsP2 = 0;
        Player1.GetComponent<BoatControl>().rdyToReload = true;
        Player1.GetComponent<BoatControl>().rdyToShoot = false;
        Player2.GetComponent<BoatControl>().rdyToReload = true;
        Player2.GetComponent<BoatControl>().rdyToShoot = false;


    }

    
}
