using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lazer : MonoBehaviour
{

    private void Start()
    {
       
    }

    public void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        PlayerHealth playerhp = GetComponent<PlayerHealth>();

        if (other.gameObject.tag == "Player")
        {

            playerhp.TakeDmg(1);
            Debug.Log("Player gone through");
        }
      



    }
}
