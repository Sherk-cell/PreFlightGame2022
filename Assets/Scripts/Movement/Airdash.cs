using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airdash : MonoBehaviour
{
    public Rigidbody rb;
    public int amntOfDash;
    public PlayerMovementAdvanced pma;
    private void Update()//Het is beter dan update omdat dit in sync is met Physics system.
    {

        if (Input.GetKeyDown(KeyCode.E) && amntOfDash >= 1)//Als E wordt gedrukt dan doet het het volgende
        {
            rb.AddForce(Camera.main.transform.forward * 10, ForceMode.VelocityChange);
            amntOfDash -= 1;
            //Het maakt een explosie van force naar voren
        }
        
        if(pma.grounded == true)
        {
            amntOfDash = 1;
        }



    }
}
