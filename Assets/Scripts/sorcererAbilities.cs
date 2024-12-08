using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorcererAbilities : MonoBehaviour
{
    [SerializeField] GameObject fireball;
    [SerializeField] Transform fireballPosition;
    [SerializeField] float fireballSpeed = 10f;
    Rigidbody rb;
    [SerializeField] GameObject inferno;

    [SerializeField] GameObject sorcerer;

    [SerializeField] GameObject clone;

    private WandererMainManagement mainManagement;
    bool isBasicAbility = false;
    bool isDefensiveAbility = false;
    bool isWildCardAbility = false;
    bool isUltimateAbility = false;

    private float basicCooldown = 1f;
    private float defensiveCooldown = 10f;
    private float wildcardCooldown = 10f;
    private float ultimateCooldown = 15f;
    // for cooldown
    private Dictionary<string, float> lastUsedTime = new Dictionary<string, float>();

    [SerializeField] GameObject smoke;

    // Start is called before the first frame update
    void Start()
    {
        mainManagement = GetComponent<WandererMainManagement>();

        lastUsedTime["Basic"] = -basicCooldown;
        lastUsedTime["Defensive"] = -defensiveCooldown;
        lastUsedTime["Wildcard"] = -wildcardCooldown;
        lastUsedTime["Ultimate"] = -ultimateCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;

        //basic: fireball
        if (Input.GetMouseButtonDown(1) && !isDefensiveAbility && !isUltimateAbility && !isWildCardAbility)
        {
            if (currentTime >= lastUsedTime["Basic"] + basicCooldown)
            {
                isBasicAbility = true;
                lastUsedTime["Basic"] = currentTime;
                BasicAbility();
            }
            else
            {
                Debug.Log("Basic ability is on cooldown.");
            }
        }
        

        //wildcard: clone
        if (Input.GetKeyDown(KeyCode.Q) && mainManagement.getAbility2Unlock())
        {
            if (currentTime >= lastUsedTime["Wildcard"] + wildcardCooldown)
            {
                Debug.Log("wildcard");
                isWildCardAbility = true;
            }
            else
            {
                Debug.Log("Wildcard ability is on cooldown.");
            }
        }
        if (Input.GetMouseButtonDown(1) && isWildCardAbility)
        {
            lastUsedTime["Wildcard"] = currentTime;
            WildcardAbility();
        }
        

        //defensive: teleport
        if (Input.GetKeyDown(KeyCode.W) && mainManagement.getAbility1Unlock() && !isBasicAbility)
        {
            if (currentTime >= lastUsedTime["Defensive"] + defensiveCooldown)
            {
                Debug.Log("defensive");
                isDefensiveAbility = true;
            }
            else
            {
                Debug.Log("Defensive ability is on cooldown.");
            }

        }
        if (Input.GetMouseButtonDown(1) && isDefensiveAbility)
        {
            lastUsedTime["Defensive"] = currentTime;
            DefensiveAbility();
        }
        

        //ultimate: inferno
        if (Input.GetKeyDown(KeyCode.E) && mainManagement.getAbility3Unlock())
        {
            if (currentTime >= lastUsedTime["Ultimate"] + ultimateCooldown)
            {
                Debug.Log("ultimate");
                isUltimateAbility = true;
            }
            else
            {
                Debug.Log("Defensive ability is on cooldown.");
            }

        }
        if (Input.GetMouseButtonDown(1) && isUltimateAbility)
        {
            lastUsedTime["Ultimate"] = currentTime;
            UltimateAbility();
        }
        
    }



    //basic ability (fireball)
    void BasicAbility()
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

            GameObject spawn = Instantiate(fireball, fireballPosition.position, fireballPosition.rotation);

            Vector3 targetPos = (hitPos - transform.position).normalized;
            rb = spawn.GetComponent<Rigidbody>();
            rb.velocity = targetPos * fireballSpeed;
        }
        isBasicAbility = false;
    }

    //wildcard ability (clone)
    void WildcardAbility()
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
                GameObject smokeSpawn = Instantiate(smoke, hitPos, Quaternion.identity);
                Destroy(smokeSpawn, 3);

                StartCoroutine(DelayedCloneDamage(hitPos, 5f));
            }
        }
        isWildCardAbility = false;
    }

    //clone damage
    IEnumerator DelayedCloneDamage(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay); 


        Collider[] hitColliders = Physics.OverlapSphere(position, 5f);

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

    //defensive ability (teleport)
    void DefensiveAbility()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                GameObject targetHit = rayhit.transform.gameObject;
                Vector3 hitPos = rayhit.point;
                if (targetHit != null)
                {
                    hitPos = hitPos + Vector3.up * sorcerer.transform.localScale.y / 2;
                    transform.position = hitPos;
                }

            }
        isDefensiveAbility = false;
    }

    //ultimate ability (inferno)
    void UltimateAbility()
    {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;

            if (Physics.Raycast(ray, out rayhit))
            {
                Vector3 direction = (rayhit.point - transform.position).normalized;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);

                GameObject targetHit = rayhit.transform.gameObject;
                Vector3 hitPos = rayhit.point;
                if (targetHit != null)
                {
                    GetComponent<Animation>().CrossFade("idle_combat", 0.0f);
                    hitPos = hitPos + (Vector3.up * inferno.transform.localScale.y / 2) + (Vector3.right * 15);
                    GameObject spawn = Instantiate(inferno, hitPos, Quaternion.identity);

                    Destroy(spawn, 5);

                }

            }
            isUltimateAbility = false;
    }
}
