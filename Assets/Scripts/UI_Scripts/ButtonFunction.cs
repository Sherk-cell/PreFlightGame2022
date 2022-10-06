using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunction : MonoBehaviour
{
    public GameObject canvas;
    public GameObject mainmenu;
    public GameObject settings;
    public PauzeGame pauzegamescripts;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainmenu.SetActive(true);
        }
       
    }
    public void StartGame()
    {
        Playinggame();
    }

    public void Go2Main()
    {
        mainmenu.SetActive(true);
        settings.SetActive(false);
    }

    public void Go2Settings()
    {
        mainmenu.SetActive(false);
        settings.SetActive(true);
    }



    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Werkt maar Unityhub ");
    }


    private void Playinggame()
    {
        pauzegamescripts.toggler = false;
        Time.timeScale = 1f;
        canvas.SetActive(false);
        Debug.Log("Doethet");
    }
}
