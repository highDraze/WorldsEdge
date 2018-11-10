using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCoordinator : MonoBehaviour
{
    public static void LoadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public void StartGame()
    {
        LoadScene("Logo");
        Cursor.visible = true;
    }

    public static void FinishGame()
    {
        LoadScene("StartScreen");
        Cursor.visible = true;
    }

    public void Quit()
    {
        UnityEngine.Application.Quit();
    }
}
