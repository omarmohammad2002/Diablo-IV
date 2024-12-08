using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorcererInferno : MonoBehaviour
{
    [SerializeField] GameObject inferno;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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
        }
    }
}
