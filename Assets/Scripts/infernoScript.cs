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
            if (hitCollider.CompareTag("Minion"))
            {
                MinionsMainManagement minionScript = hitCollider.GetComponent<MinionsMainManagement>();
                minionScript.TakeDamage(10);
            }

            if (hitCollider.CompareTag("Demon"))
            {
                DemonsMainManagement demonScript = hitCollider.GetComponent<DemonsMainManagement>();
                demonScript.TakeDamage(10);
            }

            if (hitCollider.CompareTag("Boss"))
            {
                BossMainManagement bossScript = hitCollider.GetComponent<BossMainManagement>();
                bossScript.TakeDamage(10);
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
                if (hitCollider.CompareTag("Minion"))
                {
                    MinionsMainManagement minionScript = hitCollider.GetComponent<MinionsMainManagement>();
                    minionScript.TakeDamage(2);
                }

                if (hitCollider.CompareTag("Demon"))
                {
                    DemonsMainManagement demonScript = hitCollider.GetComponent<DemonsMainManagement>();
                    demonScript.TakeDamage(2);
                }

                if (hitCollider.CompareTag("Boss"))
                {
                    BossMainManagement bossScript = hitCollider.GetComponent<BossMainManagement>();
                    bossScript.TakeDamage(2);
                }
            }
        }
        
    }
}
