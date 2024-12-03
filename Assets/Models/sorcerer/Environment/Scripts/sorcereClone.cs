using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorcererClone : MonoBehaviour
{
    [SerializeField] GameObject clone;
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

                }

            }
        }
    }

}
