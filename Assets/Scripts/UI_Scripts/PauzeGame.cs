using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauzeGame : MonoBehaviour
{
    public GameObject canvas;
    public GameObject maintab;
    public GameObject settingstab;
    public bool toggler = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        canvas.SetActive(toggler);


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            toggler = !toggler;
            
        }

        if(toggler == true)
        {
            Time.timeScale = 0f;
           
        }
        if(toggler == false)
        {

            Time.timeScale = 1f;
            settingstab.SetActive(false);
        }
    }
}
