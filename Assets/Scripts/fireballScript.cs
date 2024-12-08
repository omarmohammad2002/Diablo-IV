using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {

            MinionsDemonsMainManagement enemyScript = other.GetComponent<MinionsDemonsMainManagement>();
            if (enemyScript != null) 
            {
                    print("damage 5");
                    enemyScript.TakeDamage(5);
            }
            
        }
        Destroy(gameObject);
    }

}
