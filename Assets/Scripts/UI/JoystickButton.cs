using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class JoystickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private UnityEvent repeatEvent;
    public bool isPointerDown = false;

    private void Start()
    {
        isPointerDown = false;
    }

    private void Update()
    {
        if (isPointerDown)
        {
            repeatEvent.Invoke();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        if (GameManager.Instance.canRaycastGameObject)
            GameManager.Instance.canRaycastGameObject = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPointerDown = false;
        if (!GameManager.Instance.canRaycastGameObject)
            GameManager.Instance.canRaycastGameObject = true;
    }
}
