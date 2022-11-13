using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Introscreen : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Werkt maar Unityhub ");
    }
    public void Go2Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Start");
    }

}
