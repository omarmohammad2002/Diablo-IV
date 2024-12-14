using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    private CustomActions input;
    private NavMeshAgent agent;
    private Animator animator;
    private float lookRotationSpeed = 8f;

    void Awake()
    {
        input = new CustomActions();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
        FaceTarget();
        SetAnimations();
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

    void SetAnimations()
    {
        if (agent.velocity.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }
    }
}
