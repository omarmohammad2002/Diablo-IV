using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Camera camera;
    private CustomActions input;
    private NavMeshAgent agent;
    private Animator animator;
    private float lookRotationSpeed = 8f;

    private float lastClickTime = 0f;
    private float doubleClickThreshold = 0.3f; // Time in seconds for double-click detection

    void Awake()
    {
        input = new CustomActions();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }

    void OnEnable()
    {
        input.Enable();
        input.Main.Move.performed += ctx => HandleMouseClick();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void HandleMouseClick()
    {
        float currentTime = Time.time;
        if (currentTime - lastClickTime <= doubleClickThreshold)
        {
            // Detected a double click
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            agent.speed = 16f; // Set a higher speed for running
        }
        else
        {
            // Single click
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", true);
            agent.speed = 8f; // Default walking speed
        }

        lastClickTime = currentTime;

        // Cast a ray from the screen point where the mouse clicked
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 100))
        {
            // Check if the NavMeshAgent is on a valid NavMesh
            if (agent.isOnNavMesh)
            {
                // Set the destination of the NavMeshAgent
                agent.SetDestination(hit.point);
            }
         
        }
    }

    void Update()
    {
        if (agent.isOnNavMesh) // Ensure the agent is on a valid NavMesh before performing any operations
        {
            FaceTarget();
            UpdateMovementAnimations();
        }
       
    }

    void FaceTarget()
    {
        // Only rotate towards the target if the agent is moving and on a valid NavMesh
        if (agent.isOnNavMesh && agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }

    void UpdateMovementAnimations()
    {
        if (agent.isOnNavMesh && agent.velocity.magnitude > 0.1f) // Slight threshold to detect motion
        {
            if (animator.GetBool("isRunning"))
            {
                animator.SetBool("isWalking", false);
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }
}
