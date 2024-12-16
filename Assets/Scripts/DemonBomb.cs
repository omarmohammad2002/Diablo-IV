using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBomb : MonoBehaviour
{

    private CapsuleCollider bombCollider;
    private GameObject player;
    private AudioSource AudioSource;
    public AudioClip explosionSound;
 




    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bombCollider = GetComponent<CapsuleCollider>();
        AudioSource = GetComponent<AudioSource>();
        
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && bombCollider.enabled)
        {
            player.GetComponent<WandererMainManagement>().DealDamage(15);
            AudioSource.PlayOneShot(explosionSound);
            // Debug.Log("Player hit By Demon Bomb");
          
        }
    }
}
