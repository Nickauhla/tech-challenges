using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public class SelectionManager : MonoBehaviour
    {
        private GameObject m_selectedObject;
        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 mousePos = Mouse.current.position.ReadValue();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hitInfo))
                {
                    GameObject hitObject = hitInfo.collider.gameObject;
                    if (!m_selectedObject && hitObject?.tag == "Furniture")
                    {
                        m_selectedObject = hitObject;
                        m_selectedObject?.SendMessage("OnSelection", true);
                    }
                    else if (hitObject?.tag == "Ground" && m_selectedObject?.tag == "Furniture")
                    {
                        m_selectedObject?.SendMessage("OnSelection", false);
                        m_selectedObject = null;
                    }
                }
            }
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (m_selectedObject?.tag == "Furniture")
                {
                    m_selectedObject?.SendMessage("CancelEditMode");
                    m_selectedObject = null;
                }
            }
        }
    }
}