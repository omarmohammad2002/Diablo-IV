using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    public float basicCooldown = 1f;
    public float defensiveCooldown = 10f;
    public float wildcardCooldown = 10f;
    public float ultimateCooldown = 15f;
    // for cooldown
    private Dictionary<string, float> lastUsedTime = new Dictionary<string, float>();

    private AudioSource audioSource;
    [SerializeField] AudioClip fireballThrow;
    [SerializeField] AudioClip explode;
    [SerializeField] AudioClip cloneAudio;
    [SerializeField] AudioClip infernoAudio;

    [SerializeField] GameObject smoke;
    private Animator animator;

    [SerializeField] private Slider defensiveCooldownSlider;
    [SerializeField] private Slider wildcardCooldownSlider;
    [SerializeField] private Slider ultimateCooldownSlider;


    // Start is called before the first frame update
    void Start()
    {
        mainManagement = GetComponent<WandererMainManagement>();


        lastUsedTime["Basic"] = -basicCooldown;
        lastUsedTime["Defensive"] = -defensiveCooldown;
        lastUsedTime["Wildcard"] = -wildcardCooldown;
        lastUsedTime["Ultimate"] = -ultimateCooldown;

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();

        // Initialize slider values
        if (defensiveCooldownSlider != null)
    {
        defensiveCooldownSlider.maxValue = defensiveCooldown;
        defensiveCooldownSlider.value = defensiveCooldown;
        defensiveCooldownSlider.gameObject.SetActive(false); // Hide initially
    }
    if (wildcardCooldownSlider != null)
    {
        wildcardCooldownSlider.maxValue = wildcardCooldown;
        wildcardCooldownSlider.value = wildcardCooldown;
        wildcardCooldownSlider.gameObject.SetActive(false); // Hide initially
    }
    if (ultimateCooldownSlider != null)
    {
        ultimateCooldownSlider.maxValue = ultimateCooldown;
        ultimateCooldownSlider.value = ultimateCooldown;
        ultimateCooldownSlider.gameObject.SetActive(false); // Hide initially
    }
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = Time.time;
        // Update sliders to reflect remaining cooldown times
        UpdateCooldownSliders(currentTime);

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

    void UpdateCooldownSliders(float currentTime)
{
    if (defensiveCooldownSlider != null)
    {
        float remainingDefensiveCooldown = Mathf.Max(0, defensiveCooldown - (currentTime - lastUsedTime["Defensive"]));
        defensiveCooldownSlider.value = remainingDefensiveCooldown;
        defensiveCooldownSlider.gameObject.SetActive(remainingDefensiveCooldown > 0); // Show slider during cooldown
    }

    if (wildcardCooldownSlider != null)
    {
        float remainingWildcardCooldown = Mathf.Max(0, wildcardCooldown - (currentTime - lastUsedTime["Wildcard"]));
        wildcardCooldownSlider.value = remainingWildcardCooldown;
        wildcardCooldownSlider.gameObject.SetActive(remainingWildcardCooldown > 0); // Show slider during cooldown
    }

    if (ultimateCooldownSlider != null)
    {
        float remainingUltimateCooldown = Mathf.Max(0, ultimateCooldown - (currentTime - lastUsedTime["Ultimate"]));
        ultimateCooldownSlider.value = remainingUltimateCooldown;
        ultimateCooldownSlider.gameObject.SetActive(remainingUltimateCooldown > 0); // Show slider during cooldown
    }
}



    //basic ability (fireball)
    void BasicAbility()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayhit;

        if (Physics.Raycast(ray, out rayhit))
        {
            GameObject targetHit = rayhit.transform.gameObject;
            if (targetHit.CompareTag("Minion") || targetHit.CompareTag("Demon") || targetHit.CompareTag("Boss"))
            {
                Vector3 direction = (rayhit.point - transform.position).normalized;
                direction.y = 0;
                transform.rotation = Quaternion.LookRotation(direction);

                animator.SetTrigger("Fireball");

                GameObject spawn = Instantiate(fireball, fireballPosition.position, fireballPosition.rotation);
                Debug.Log(spawn);

                Vector3 targetPos = (targetHit.transform.position - transform.position).normalized;
                rb = spawn.GetComponent<Rigidbody>();
                rb.velocity = targetPos * fireballSpeed;

                audioSource.PlayOneShot(fireballThrow);
                isBasicAbility = false; 
            }
            else
            {
                Debug.Log("taregt should be minion or demon or boss");
            }
        }

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
                hitPos.y = 0;
                GameObject spawn = Instantiate(clone, hitPos, Quaternion.identity);
                AudioSource.PlayClipAtPoint(cloneAudio, hitPos);

                UpdateMinionsPlayerReference(spawn);

                StartCoroutine(PlaySoundAndDestroy(spawn, 5f, hitPos));

                StartCoroutine(ResetTagAfterDelay(4.9f));

                StartCoroutine(SpawnSmokeAfterDelay(hitPos, 5f));

                StartCoroutine(DelayedCloneDamage(hitPos, 5f));
            }
        }
        isWildCardAbility = false;
    }

    //extra methods for clone
    void UpdateMinionsPlayerReference(GameObject newPlayer)
    {
        // Find all GameObjects with the tag "Minion"
        GameObject[] minionGameObjects = GameObject.FindGameObjectsWithTag("Minion");
        GameObject[] demonGameObjects = GameObject.FindGameObjectsWithTag("Demon");

        foreach (GameObject minion in minionGameObjects)
        {
            // Get the MinionChasing instance from the GameObject
            MinionsChasingPlayer minionChasing = minion.GetComponent<MinionsChasingPlayer>();
            if (minionChasing != null)
            {
                // Update the player reference
                minionChasing.player = newPlayer;
            }
        }

        foreach (GameObject demon in demonGameObjects)
        {
            // Get the MinionChasing instance from the GameObject
            DemonsChasingPlayer demonChasing = demon.GetComponent<DemonsChasingPlayer>();
            if (demonChasing != null)
            {
                // Update the player reference
                demonChasing.player = newPlayer;
            }
        }
    }
    IEnumerator ResetTagAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset the player back to the original game object
        GameObject[] minionGameObjects = GameObject.FindGameObjectsWithTag("Minion");
        GameObject[] demonGameObjects = GameObject.FindGameObjectsWithTag("Demon");

        foreach (GameObject minion in minionGameObjects)
        {
            // Get the MinionChasing instance from the GameObject
            MinionsChasingPlayer minionChasing = minion.GetComponent<MinionsChasingPlayer>();
            if (minionChasing != null)
            {
                // Update the player reference
                minionChasing.player = gameObject;
            }
        }

        foreach (GameObject demon in demonGameObjects)
        {
            // Get the MinionChasing instance from the GameObject
            DemonsChasingPlayer demonChasing = demon.GetComponent<DemonsChasingPlayer>();
            if (demonChasing != null)
            {
                // Update the player reference
                demonChasing.player = gameObject;
            }
        }
    }
    IEnumerator SpawnSmokeAfterDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject smokeSpawn = Instantiate(smoke, position, Quaternion.identity);
        Destroy(smokeSpawn, 3); 
    }
    private IEnumerator PlaySoundAndDestroy(GameObject spawn, float delay, Vector3 hitPos)
    {
        // Wait for the delay
        yield return new WaitForSeconds(delay - 0.1f);

        // Play the sound at the spawn's position
        AudioSource.PlayClipAtPoint(explode, spawn.transform.position);

        // Destroy the spawn object
        Destroy(spawn);
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
                NavMeshAgent agent = GetComponent<NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                }

                transform.position = hitPos;

                if (agent != null)
                {
                    agent.enabled = true;
                }
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
                animator.SetTrigger("Inferno");

                hitPos = hitPos + (Vector3.up * inferno.transform.localScale.y / 2) + (Vector3.right * 15);
                GameObject spawn = Instantiate(inferno, hitPos, Quaternion.identity);
                AudioSource.PlayClipAtPoint(infernoAudio, hitPos);

                Destroy(spawn, 5);

            }

            }
        isUltimateAbility = false;
    }

    /*void DisableAgent()
    {
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
            Debug.Log("NavMeshAgent disabled");
        }
    }*/
}