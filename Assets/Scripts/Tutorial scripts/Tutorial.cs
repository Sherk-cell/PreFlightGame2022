using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public bool wkeycheck;
    public bool akeycheck;
    public bool skeycheck;
    public bool dkeycheck;
    public bool spacekeycheck;
    public bool sprintchecked = false;
    public bool slidechecked = false;
    public bool wall1check;
    public bool wall2check;


    public bool seconedone;
    public bool sectwodone;







    public GameObject wall1;
    public GameObject wall2;




    public GameObject wasdtext;
    public GameObject sprinttext;
    public GameObject slidetext;
    public GameObject Donetext;

    private KeyCode[] InputList = new KeyCode[]
  {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.Space,

  };


    private void Update()
    {



        foreach (KeyCode key in InputList)
        {
            if (Input.GetKeyDown(key))
                switch (key)
                {
                    case KeyCode.W:
                        wkeycheck = true;
                        break;
                    case KeyCode.A:
                        akeycheck = true;
                        break;
                    case KeyCode.S:
                        skeycheck = true;
                        break;
                    case KeyCode.D:
                        dkeycheck = true;
                        break;
                    case KeyCode.Space:
                        spacekeycheck = true;
                        break;


                }
        }

        if (wkeycheck && akeycheck && skeycheck && dkeycheck && spacekeycheck == true && seconedone == false)
        {
            wasdtext.SetActive(false);
            sprinttext.SetActive(true);


            wall1check = true;

            wall1.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && wall1 == true && seconedone == false)
        {
            seconedone = true;


            wall2check = true;
            wall2.SetActive(false);
            sprinttext.SetActive(false);



            sprintchecked = true;
            wall1check = false;
            slidetext.SetActive(true);


        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && wall2 == true && sprintchecked == true)
        {
            sectwodone = true;


            slidetext.SetActive(false);
            Donetext.SetActive(true);
            print("Nice");
        }



        

    }


    private void OnTriggerEnter(Collider other)
    {
        if(gameObject.tag == "Player")
        {
            Debug.Log("wor");
            SceneManager.LoadScene("Level1");


        }
    }



}
