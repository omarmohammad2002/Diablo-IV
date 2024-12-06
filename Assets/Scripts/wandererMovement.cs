using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class wandererMovement : MonoBehaviour
{
    [SerializeField] Camera camera;
    private NavMeshAgent agent;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                animator.SetBool("isRunning", true);
            }
        }

        // Update animator parameter
        if (agent.remainingDistance > agent.stoppingDistance + 1)
        {
            
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }
}