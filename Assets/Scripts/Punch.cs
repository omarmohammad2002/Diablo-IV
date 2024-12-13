using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Punch : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider punchCollider;
    private GameObject player;
    WandererMainManagement WandererMainManagement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        punchCollider = GetComponent<BoxCollider>();
        WandererMainManagement = player.GetComponent<WandererMainManagement>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && punchCollider.enabled)
        {
            player.GetComponent<WandererMainManagement>().DealDamage(5);
            Debug.Log("Player hit By Minion");

        }
    }
}
