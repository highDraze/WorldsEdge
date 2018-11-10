using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {

    private int currentPos = 0;
    public Material mat;

    private void Start()
    {
        currentPos = 0;
        cycle(currentPos);
    }

    // Update is called once per frame
    void Update () {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetKeyDown(KeyCode.A))
        {
            currentPos = (currentPos + 1) % 4;
            cycle(currentPos);
        }
    }

    private void cycle(int i)
    {
        switch (i)
        {
            case 0:
                {
                    mat.SetFloat("_Speed", 0.65f);
                    mat.SetFloat("_Height", 1.35f);
                    mat.SetFloat("_NSpeed", 1.42f);
                    mat.SetFloat("_NHeight", 0.45f);
                    mat.SetFloat("_Length", 3.7f);
                    mat.SetFloat("_Stretch", 9.72f);
                }
                break;
            case 1:
                {
                    mat.SetFloat("_Speed", 0.65f);
                    mat.SetFloat("_Height", 1.35f);
                    mat.SetFloat("_NSpeed", 0f);
                    mat.SetFloat("_NHeight", 0f);
                    mat.SetFloat("_Length", 3.7f);
                    mat.SetFloat("_Stretch", 9.72f);
                }
                break;
            case 2:
                {
                    mat.SetFloat("_Speed", 0f);
                    mat.SetFloat("_Height", 0f);
                    mat.SetFloat("_NSpeed", 1.42f);
                    mat.SetFloat("_NHeight", 0.3f);
                    mat.SetFloat("_Length", 3.7f);
                    mat.SetFloat("_Stretch", 9.72f);
                }
                break;
            case 3:
                {
                    mat.SetFloat("_Speed", 0.8f);
                    mat.SetFloat("_Height", 3.41f);
                    mat.SetFloat("_NSpeed", 2.38f);
                    mat.SetFloat("_NHeight", 0.61f);
                    mat.SetFloat("_Length", 9.23f);
                    mat.SetFloat("_Stretch", 20.54f);
                }
                break;
        }
    }
}
