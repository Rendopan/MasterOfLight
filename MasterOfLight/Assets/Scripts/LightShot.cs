using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShot : MonoBehaviour
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
        transform.position += shootDir * speed * Time.deltaTime;
        if(transform.position.y < 0) 
        { 
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.LoseHitPoints(damage);
            Destroy(this.gameObject);
        }
    }
}
