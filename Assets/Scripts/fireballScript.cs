using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireballScript : MonoBehaviour
{
    [SerializeField] AudioClip explode;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Minion"))
        {

            MinionsMainManagement minionScript = other.GetComponent<MinionsMainManagement>();
            if (minionScript != null) 
            {
                print("damage 5");
                minionScript.TakeDamage(5);
            }
            GameObject tempAudioSource = new GameObject("TempAudio");
            AudioSource tempSource = tempAudioSource.AddComponent<AudioSource>();
            tempSource.clip = explode;
            tempSource.Play();
            Destroy(gameObject);
            
        }

        if (other.CompareTag("Demon"))
        {

            DemonsMainManagement demonScript = other.GetComponent<DemonsMainManagement>();
            if (demonScript != null)
            {
                print("damage 5");
                demonScript.TakeDamage(5);
            }
            GameObject tempAudioSource = new GameObject("TempAudio");
            AudioSource tempSource = tempAudioSource.AddComponent<AudioSource>();
            tempSource.clip = explode;
            tempSource.Play();
            Destroy(gameObject);
        }

        if (other.CompareTag("Boss"))
        {

            BossMainManagement bossScript = other.GetComponent<BossMainManagement>();
            if (bossScript != null)
            {
                print("damage 5");
                bossScript.TakeDamage(5);
            }
            GameObject tempAudioSource = new GameObject("TempAudio");
            AudioSource tempSource = tempAudioSource.AddComponent<AudioSource>();
            tempSource.clip = explode;
            tempSource.Play();
            Destroy(gameObject);
        }

        if (other.CompareTag("Untagged"))
        {
            GameObject tempAudioSource = new GameObject("TempAudio");
            AudioSource tempSource = tempAudioSource.AddComponent<AudioSource>();
            tempSource.clip = explode;
            tempSource.Play();
            Destroy(gameObject);
        }
        
    }

}
