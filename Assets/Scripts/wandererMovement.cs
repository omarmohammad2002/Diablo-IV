using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class WandererMovement : MonoBehaviour
{
    private Camera camera;
    private NavMeshAgent agent;
    private Animator animator;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f; // Time in seconds to detect a double click
    private bool isDoubleClick = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    void Update()
    {

        // if (Input.GetMouseButton(0) &&!EventSystem.current.IsPointerOverGameObject())
        if (Input.GetMouseButton(0))

        {
            float currentTime = Time.time;
            float timeSinceLastClick = currentTime - lastClickTime;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                isDoubleClick = true;
                HandleDoubleClick(); // Handle double click immediately
            }
            else
            {
                isDoubleClick = false;
                StartCoroutine(HandleSingleClickAfterDelay());
            }

            lastClickTime = currentTime;
        }

        // Update animator parameters when destination is reached
        if (animator != null && agent.enabled)
        {
            if (agent.remainingDistance < agent.stoppingDistance + 1f)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", false);
            }
        }
    }

    private IEnumerator HandleSingleClickAfterDelay()
    {
        yield return new WaitForSeconds(doubleClickThreshold);

        // If not a double click, handle as a single click
        if (!isDoubleClick)
        {
            HandleSingleClick();
        }
    }

    private void HandleSingleClick()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);

            if (animator != null)
            {
                animator.SetBool("isRunning", false);
                animator.SetBool("isWalking", true);
                agent.speed = 7.5f; // Adjust speed for walking
            }
        }
    }

    private void HandleDoubleClick()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            agent.SetDestination(hit.point);

            if (animator != null)
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                agent.speed = 15f; // Adjust speed for running
            }
        }
    }
}
