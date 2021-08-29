using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseInputsController : MonoBehaviour
{
    private MouseInputs mouseInputs = null;
    [SerializeField]
    private LayerMask activeLayer;

    private void Awake()
    {
        mouseInputs = new MouseInputs();
    }

    private void OnEnable()
    {
        mouseInputs.Enable();

        //click the left mouse button to interact with other objects in the scene
        mouseInputs.ActionObjects.LeftClick.performed += AssessClickedObject;
    }

    private void AssessClickedObject(InputAction.CallbackContext obj)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector3.zero, Mathf.Infinity, activeLayer);

        if(hit.collider == null) { return; }

        bool objectExists = hit.collider.gameObject.TryGetComponent(out ItemClickable item);
        
        if (objectExists)
        {
            item.OnClick();
        }
    }

    private void OnDisable()
    {
        mouseInputs.Disable();
    }
}
