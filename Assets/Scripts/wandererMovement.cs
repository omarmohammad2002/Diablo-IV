using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WandererMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private NavMeshAgent agent;
    private Animator animator;

    private float clickTime = 0f;
    private float doubleClickThreshold = 0.3f; // Time in seconds to detect a double click
    private bool isDoubleClick = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - clickTime;
            clickTime = Time.time;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                isDoubleClick = true;
            }
            else
            {
                isDoubleClick = false;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);

                if (animator != null)
                {
                    if (isDoubleClick)
                    {
                        animator.SetBool("isRunning", true);
                        animator.SetBool("isWalking", false);
                        agent.speed = 15f; // Adjust speed for running
                    }
                    else
                    {
                        animator.SetBool("isRunning", false);
                        animator.SetBool("isWalking", true);
                        agent.speed = 7.5f; // Adjust speed for walking
                    }
                }
            }
        }

        // Update animator parameters
        if (animator != null && agent.enabled)
        {
            if (agent.remainingDistance < agent.stoppingDistance + 1f)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
            }
        }
    }
}
