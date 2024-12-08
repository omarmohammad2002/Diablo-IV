/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorcererClone : MonoBehaviour
{
    [SerializeField] GameObject clone;
    [SerializeField] GameObject smoke;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                GameObject targetHit = rayhit.transform.gameObject;
                Vector3 hitPos = rayhit.point;
                
                if (targetHit != null)
                {
                    hitPos = hitPos + Vector3.up * clone.transform.localScale.y / 2;
                    GameObject spawn = Instantiate(clone, hitPos, Quaternion.identity);
                   
                    Destroy(spawn, 5);
                    StartCoroutine(DelayedCloneDamage(hitPos, 5f));
                    GameObject smokeSpawn = Instantiate(smoke, hitPos, Quaternion.identity);
                    Destroy(smokeSpawn, 3);

                }
                
            }
        }
    }

    IEnumerator DelayedCloneDamage(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        // Apply damage at the position where the clone was spawned
        Collider[] hitColliders = Physics.OverlapSphere(position, 5f);

        foreach (Collider hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("enemy"))
            {
                MinionsDemonsMainManagement enemyScript = hitCollider.GetComponent<MinionsDemonsMainManagement>();
                Debug.Log("Enemy detected in range: " + hitCollider.name);
                enemyScript.TakeDamage(10);
            }
        }
    }

}
*/