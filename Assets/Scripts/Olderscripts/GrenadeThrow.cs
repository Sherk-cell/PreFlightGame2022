using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrow : MonoBehaviour
{
    public int damage;
    //public bool destroyOnHit;

    [Header("Explosive Projectile")]
    public bool isExplosive;
    public float explosionRadius;
    public float explosionForce;
    public int explosionDamage;
    public GameObject explosionEffect;

    private Rigidbody rb;

    private bool targetHit;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // make sure only to stick to the first target you hit
        if (targetHit)
        {
            return;
        }

        else
            targetHit = true;

        // check if you hit an enemy
       // if (collision.gameObject.GetComponent<Enemy>() != null)
        {
           // Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            //enemy.TakeDamage(damage);

            // destroy projectile after kill
            Destroy(gameObject);

            // destroy projectile
            /*if (!isExplosive) //destroyOnHit
                Invoke(nameof(DestroyProjectile), 0.1f);*/
        }

        // explode projectile if it's explosive
        if (isExplosive)
        {
            Explode();
            return;
        }
    }
    private void Explode()
    {
        // spawn explosion effect (if assigned)
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // find all the objects that are inside the explosion range
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, explosionRadius);

        // loop through all of the found objects and apply damage and explosion force
        for (int i = 0; i < objectsInRange.Length; i++)
        {
            if (objectsInRange[i].gameObject == gameObject)
            {
                // don't break or return please, thanks
            }
            else
            {
                // check if object is enemy, if so deal explosionDamage
             //   if (objectsInRange[i].GetComponent<Enemy>() != null)
               //     objectsInRange[i].GetComponent<Enemy>().TakeDamage(explosionDamage);

                // check if object has a rigidbody
                if (objectsInRange[i].GetComponent<Rigidbody>() != null)
                {
                    // custom explosionForce
                    Vector3 objectPos = objectsInRange[i].transform.position;

                    // calculate force direction
                    Vector3 forceDirection = (objectPos - transform.position).normalized;

                    // apply force to object in range
                    objectsInRange[i].GetComponent<Rigidbody>().AddForceAtPosition(forceDirection * explosionForce + Vector3.up * explosionForce, transform.position + new Vector3(0, -0.5f, 0), ForceMode.Impulse);
                    if(objectsInRange[i].gameObject.tag == "Player")
                    {
                        PlayerHealth playerHP = objectsInRange[i].transform.GetComponent<PlayerHealth>();

                        if(playerHP != null)
                        {
                            Debug.Log("Hitted Player");
                            playerHP.TakeDmg(1);
                        }
                        
                        
                    }
                    Debug.Log("Kabooom " + objectsInRange[i].name);



                }
            }
        }

        // destroy projectile with 0.1 seconds delay
        //Invoke(nameof(DestroyProjectile), 0.1f);
    }

    /*private void DestroyProjectile()
    {
        Destroy(gameObject);
    }*/

    // just graphics stuff
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
