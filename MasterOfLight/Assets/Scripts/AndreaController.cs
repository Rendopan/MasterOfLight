using UnityEngine;
using UnityEngine.AI;

public class AndreaController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent character;
    public Animator characterAnimator;
    public GameObject targetDest;



    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint))
            {
                targetDest.transform.position = hitPoint.point;
                character.SetDestination(hitPoint.point);
            }
        }

        if (character.velocity != Vector3.zero)
        {
            characterAnimator.SetBool("isWalking", true);
        }
        else if (character.velocity == Vector3.zero)
        {
            characterAnimator.SetBool("isWalking", false);
        }
    }

}
