using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(-6.5f, 12f, 4f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Camera postion should be equal to player position
        this.transform.position = player.transform.position + offset;
    }
}
