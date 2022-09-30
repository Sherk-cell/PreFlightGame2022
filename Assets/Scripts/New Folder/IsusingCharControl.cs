using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsusingCharControl : MonoBehaviour
{
    public PlayerMovementAdvanced PMA;
    public GameObject obj;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            obj.SetActive(false);
            PMA.IsusingCharController = false;
            PMA.CharacterController.enabled = false;
            PMA.Stats = PlayerMovementAdvanced.States.Normal;
            
        }

        
    }
}
