using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalToBossLevel : MonoBehaviour
{
    // Start is called before the first frame update
    private WandererMainManagement mainManagement;

    void Start()
    {
        mainManagement = GetComponent<WandererMainManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
           if (mainManagement.getRuneFragments() == 3)
            {
                mainManagement.useRuneFragment();
                mainManagement.useRuneFragment();
                mainManagement.useRuneFragment();
                //load the boss level with the same player attributes
            }
        }
    }

}
