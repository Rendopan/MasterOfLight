using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkShot : MonoBehaviour
{
    private Vector3 shootDir;
    private float speed = 2f;
    private int damage;

    public void Setup(Vector3 dir, int dmg) 
    {
        shootDir = dir;
        damage = dmg;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("A Dark Sphere is destroyed.");
        transform.position += shootDir * speed * Time.deltaTime;
        if(transform.position.y < 0) 
        {
            Debug.Log("A Dark Sphere is destroyed.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light" && other is SphereCollider)
        {
            StreetLamp streetLamp = other.gameObject.GetComponentInParent<StreetLamp>();
            streetLamp.LoseHitPoints(damage);
            Destroy(this.gameObject);
        }
    }
}
