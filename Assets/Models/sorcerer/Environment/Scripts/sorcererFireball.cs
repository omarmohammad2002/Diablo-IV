using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class sorcererFireball : MonoBehaviour
{
    [SerializeField] GameObject fireball;
    [SerializeField] Transform fireballPosition;
    [SerializeField] float fireballSpeed = 10f;
    Rigidbody rb;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                Vector3 direction = (rayhit.point - transform.position).normalized;
                direction.y = 0; 
                transform.rotation = Quaternion.LookRotation(direction);

                GetComponent<Animation>().CrossFade("attack_short_001", 0.0f);
                GetComponent<Animation>().CrossFadeQueued("idle_combat");

                Vector3 hitPos = rayhit.point;

                GameObject spawn = Instantiate(fireball,fireballPosition.position, fireballPosition.rotation);
                
                Vector3 targetPos = (hitPos - transform.position).normalized;
                rb = spawn.GetComponent<Rigidbody>();
                rb.velocity = targetPos * fireballSpeed;

            }
        }
            
    }
    
}
