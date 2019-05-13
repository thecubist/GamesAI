using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void quitGame()
    {
        //Debug.Log("quit");
        //UnityEditor.EditorApplication.isPlaying = false; //quit if using editor in engine
        Application.Quit(); //quit if in build
    }

    public void startSinglePlayer()
    {
        SceneManager.LoadScene(1);
    }

    public void startMultiPlayer()
    {
        SceneManager.LoadScene(2);
    }

    public void startScene(int sceneID)
    {
        //Debug.Log("startsc int");
        SceneManager.LoadScene(sceneID);
    }

    public void startScene(string sceneName)
    {
        //Debug.Log("startsc string");
        SceneManager.LoadScene(sceneName);
    }
}