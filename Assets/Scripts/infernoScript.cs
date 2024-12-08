using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infernoScript : MonoBehaviour
{
    private float timeCounter = 0;
    private void Start()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("enemy"))
            {
                MinionsDemonsMainManagement enemyScript = hitCollider.GetComponent<MinionsDemonsMainManagement>();
                        Debug.Log("Enemy detected in front: " + hitCollider.name);
                enemyScript.TakeDamage(10);
            }
        }
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        print(timeCounter);
        if(timeCounter >= 1.0)
        {
            timeCounter = 0f;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 5f);

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("enemy"))
                {
                    MinionsDemonsMainManagement enemyScript = hitCollider.GetComponent<MinionsDemonsMainManagement>();
                    Debug.Log("Enemy detected in front: " + hitCollider.name);
                    enemyScript.TakeDamage(2);
                }
            }
        }
        
    }
}
