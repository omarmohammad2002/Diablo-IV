using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        print("ArrowScript OnTriggerEnter");
        if (other.CompareTag("Minion"))
        {

            MinionsMainManagement minionScript = other.GetComponent<MinionsMainManagement>();
            if (minionScript != null)
            {
                print("damage 5");
                minionScript.TakeDamage(5);
            }

        }

        if (other.CompareTag("Demon"))
        {

            DemonsMainManagement demonScript = other.GetComponent<DemonsMainManagement>();
            if (demonScript != null)
            {
                print("damage 5");
                demonScript.TakeDamage(5);
            }

        }

        if (other.CompareTag("Boss"))
        {
            print("Boss");
            BossMainManagement bossScript = other.GetComponent<BossMainManagement>();
            if (bossScript != null)
            {
                print("damage 5");
                bossScript.TakeDamage(5);
            }

        }

        Destroy(gameObject);
    }

}
