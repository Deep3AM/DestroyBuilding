using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHandler : MonoBehaviour
{
    [SerializeField] private Player player;
    private void LateUpdate()
    {
        TouchObject();
    }

    private void TouchObject()
    {
        if (!GameManager.Instance.canRaycastGameObject)
            return;
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            int attack = player.Shoot();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Building>() != null)
                {
                    hit.transform.gameObject.GetComponent<Building>().OnDamaged(attack);
                }
            }
        }
#endif
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            int attack = player.Shoot();
            Ray ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponent<Building>() != null)
                {
                    hit.transform.gameObject.GetComponent<Building>().OnDamaged(attack);
                }
            }
        }
    }
}
