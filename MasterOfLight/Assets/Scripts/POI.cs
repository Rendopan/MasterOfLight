using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Andrea")
        {
            if (TaleManager.Tale.CurrentStroke == 0)
            {
                TaleManager.Tale.SetStroke("I cannot remember but I was here before…");
                TaleManager.Tale.CurrentStroke++;
            }
        }
    }
}
