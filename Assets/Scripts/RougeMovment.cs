using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class RougeMovment : MonoBehaviour
{
    private Camera camera;
    [SerializeField] private float stoppingThreshold = 1.7f; // Allowable distance to stop earlier
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
            // Set the destination of the NavMeshAgent
            agent.SetDestination(hit.point);
        }
    }

    void Update()
    {
        HandleEarlyStopping();
        FaceTarget();
        UpdateMovementAnimations();
    }

    void HandleEarlyStopping()
    {
        if (agent.pathPending) return; // Wait for the path to be calculated
        // Check if the agent is within the stopping threshold
        if (agent.remainingDistance <= stoppingThreshold && agent.remainingDistance > 0f)
        {
            agent.ResetPath(); // Stop the agent
        }
    }

    void FaceTarget()
    {
        // Only rotate towards the target if the agent is moving
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            Vector3 direction = (agent.destination - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
        }
    }

    void UpdateMovementAnimations()
    {
        if (agent.velocity.magnitude > 0.1f) // Slight threshold to detect motion
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
