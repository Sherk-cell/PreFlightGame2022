using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public float hP;

    public bool invince = false;

    public GameObject block3Hp;
    public GameObject block2Hp;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        Debug.Log(hP);



        switch (hP)
        {
            case 2:
                block3Hp.SetActive(true);
                break;
            case 1:
                block2Hp.SetActive(true);
                break;
            
        }

    }







    public IEnumerator Invisframes()
    {
        invince = true;
        Debug.Log(invince);
        yield return new WaitForSeconds(10f);
        Debug.Log(invince);
        invince = false;
    }

    public void TakeDmg(float amount)
    {
        if(invince == false)
        {
            hP -= amount;
            StartCoroutine("Invisframes");
        }
        
        
    }

}
