using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    float dist;
    public float range;
    public Transform head, barrel;
    public GameObject _projectile;
    public float speed;
    public float fireRate, nextFire;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(target.position, transform.position);
        if(dist <= range)
        {
            head.LookAt(target);
            if(Time.time >= nextFire)
            {
                nextFire = Time.time + 1f / fireRate;
                Shoot();

            }
        }
    }

    void OndrawGizmosselected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }
    void Shoot()
    {
        GameObject Clone = Instantiate(_projectile, barrel.position, head.rotation);
        Clone.GetComponent<Rigidbody>().AddForce(head.forward * speed);
       // Destroy(Clone, 10);
    }
}
