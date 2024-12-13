using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    // Start is called before the first frame update
    private BoxCollider swordCollider;
    private GameObject player;
    WandererMainManagement WandererMainManagement;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        swordCollider = GetComponent<BoxCollider>();
        WandererMainManagement = player.GetComponent<WandererMainManagement>();
       
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && swordCollider.enabled)
        {
            player.GetComponent<WandererMainManagement>().DealDamage(10);
            Debug.Log("Player hit By Demon");
          
        }
    }
}
