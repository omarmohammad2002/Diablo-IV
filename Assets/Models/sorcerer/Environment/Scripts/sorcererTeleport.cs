using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sorcererTeleport : MonoBehaviour
{
    [SerializeField] GameObject sorcerer;
    void Update()
    {
        if (Input.GetMouseButton(0))
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
        }
    }

}
