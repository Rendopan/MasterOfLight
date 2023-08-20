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
        // A Dark Sphere is destroyed."
        transform.position += shootDir * speed * Time.deltaTime;
        if(transform.position.y < 0 || transform.position.y > 120f) 
        {
            // A Dark Sphere is destroyed.
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Light" && other is SphereCollider)
        {
            StreetLamp streetLamp = other.gameObject.GetComponentInParent<StreetLamp>();
            if(streetLamp != null) 
            {
                streetLamp.LoseHitPoints(damage);
            }
            POI poi = other.gameObject.GetComponentInParent<POI>();
            if (poi != null)
            {
                poi.LoseHitPoints(damage);
            }

            Destroy(this.gameObject);
        }
    }
}
