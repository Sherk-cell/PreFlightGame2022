using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjGrab : MonoBehaviour
{
    public GameObject Crosshair;
    public PlayerCam playerCamera;


    private void Start()
    {

    }

    private void Update()
    {


        RaycastHit HitData;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out HitData))
        {
            if (HitData.transform.gameObject.tag == "GrappableObject")
            {
                Crosshair.SetActive(true);
            }
        }

        else
        {
            Crosshair.SetActive(false);
        }



    }





}
