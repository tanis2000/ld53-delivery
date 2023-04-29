using System;
using System.Collections;
using App.Input;
using GameBase.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace App.UI
{
    public class MainMenuController : MonoBehaviour, GameInput.IMenuActions
    {
        public GameObject FirstButton;

        private GameInput gameInput;
        private GameInput.IMenuActions menuActionsImplementation;

        private void OnEnable()
        {
            StartCoroutine(SelectFirstButton());
            gameInput = new GameInput();
            gameInput.Menu.SetCallbacks(this);
            gameInput.Menu.Enable();
        }

        private void OnDisable()
        {
            gameInput.Menu.Disable();
            gameInput.Menu.RemoveCallbacks(this);
        }

        private IEnumerator SelectFirstButton()
        {
            yield return new WaitForSeconds(2);
            EventSystem.current.SetSelectedGameObject(FirstButton);
            var style = FirstButton.GetComponent<ButtonStyle>();
            style.Select();
            var btn = FirstButton.GetComponent<Button>();
            btn.Select();
            yield return null;
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            Debug.Log(context.performed);
            if (context.performed)
            {
                var btn = FirstButton.GetComponent<Button>();
            }
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            Debug.Log(context.performed);
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            Debug.Log(context.performed);
        }
    }
}