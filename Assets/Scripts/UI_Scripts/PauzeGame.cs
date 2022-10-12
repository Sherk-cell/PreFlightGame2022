using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzeGame : MonoBehaviour
{
    public GameObject canvas;
    public GameObject maintab;
    public GameObject settingstab;
    public bool toggler = false;
    public GameObject Crosshair;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        canvas.SetActive(toggler);


        if (Input.GetKeyDown(KeyCode.P))
        {
            toggler = !toggler;
            
        }

        if(toggler == true)
        {
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Crosshair.SetActive(false);
        }
        if(toggler == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            settingstab.SetActive(false);
            Crosshair.SetActive(true);
        }
    }
}
