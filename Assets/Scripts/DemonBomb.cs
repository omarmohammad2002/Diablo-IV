using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBomb : MonoBehaviour
{

    private CapsuleCollider bombCollider;
    private GameObject player;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bombCollider = GetComponent<CapsuleCollider>();
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && bombCollider.enabled)
        {
            player.GetComponent<WandererMainManagement>().DealDamage(15);
            Debug.Log("Player hit By Demon Bomb");
          
        }
    }
}
