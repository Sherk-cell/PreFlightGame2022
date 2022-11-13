using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwing : MonoBehaviour
{[Header("References")]
    public Transform cam;
    public Transform attackPoint;
    public GameObject objectToThrow;

    public GameObject secondObjectToThrow;

    [Header("Settings")]
    public int totalThrows;
    public float throwCooldown;

    public int totalGrenades;
    public float grenadeCooldown;

    [Header("Throwing")]
    public KeyCode throwKey = KeyCode.Mouse0;
    public KeyCode GrenadeKey = KeyCode.Mouse1;
    public float throwForce;
    public float throwUpwardForce;

    public float throwGrenadeForce;
    public float throwUpwardNadeForce;

    bool readyToThrow;
    bool readyToGrenade;

    private void Start()
    {
        readyToThrow = true;

        readyToGrenade = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(throwKey) && readyToThrow && totalThrows > 0)
        {
            Throw();
        }

        if (Input.GetKeyDown(GrenadeKey) && readyToGrenade && totalGrenades > 0)
        {
            GrenadeThrow();
        }
    }

    private void GrenadeThrow()
    {
        readyToGrenade = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(secondObjectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        Vector3 forceDirection = cam.transform.forward;

        // add force
        Vector3 forceToAdd = forceDirection * throwGrenadeForce + transform.up * throwUpwardNadeForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalGrenades--;

        // implement throwCooldown
        Invoke(nameof(ResetGrenadeThrow), grenadeCooldown);
    }
    private void Throw()
    {
        readyToThrow = false;

        // instantiate object to throw
        GameObject projectile = Instantiate(objectToThrow, attackPoint.position, cam.rotation);

        // get rigidbody component
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

        // calculate direction
        Vector3 forceDirection = cam.transform.forward;

        //RaycastHit hit;

        /*if(Physics.Raycast(cam.position, cam.forward, out hit, 500f))
        {
            forceDirection = (hit.point - attackPoint.position).normalized;
        }*/

        // add force
        Vector3 forceToAdd = forceDirection * throwForce + transform.up * throwUpwardForce;

        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        totalThrows--;

        // implement throwCooldown
        Invoke(nameof(ResetThrow), throwCooldown);
    }

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void ResetGrenadeThrow()
    {
        readyToGrenade = true;
    }
}