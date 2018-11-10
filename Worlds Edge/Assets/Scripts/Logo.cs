using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logo : MonoBehaviour {

    public static Image logo;
    private float targetAlpha = 0;
    private float speed = 1;
    private bool active = false;
    // Use this for initialization

    void Start() {
        logo = GameObject.Find("Logo").GetComponent<Image>();
        StartCoroutine(swxitch ());
        Cursor.visible = false;

    }

    private void Update()
    {
        if(active)
        {
            Color curColor = logo.color;
            float alphaDiff = Mathf.Abs(curColor.a - 0);
            if (alphaDiff > 0.0001f)
            {
                curColor.a = Mathf.Lerp(curColor.a, targetAlpha, speed * Time.deltaTime);
                logo.color = curColor;

            }
        }
       
    }

    public static void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    IEnumerator swxitch()
    {
        yield return new WaitForSeconds(0.3f);
        active = true;
        targetAlpha = 1;
        yield return new WaitForSeconds(3);
        speed = 2;
        targetAlpha = 0;
        yield return new WaitForSeconds(2);
        LoadScene("TitleScreen");
    }
}
