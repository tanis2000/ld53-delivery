using GameBase.SceneChanger;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace App
{
    public class NameInput : MonoBehaviour
    {
        public TMP_InputField Field;

        private void Start()
        {
            Field.onValueChanged.AddListener(ToUpper);
            Invoke(nameof(FocusInput), 0.6f);
        }

        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.KeypadEnter) || UnityEngine.Input.GetKeyDown(KeyCode.Return))
            {
                Save();
            }
        }

        private void FocusInput()
        {
            EventSystem.current.SetSelectedGameObject(Field.gameObject);
            Field.OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void ToUpper(string value)
        {
            Field.text = value;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(Field.text)) return;
            PlayerPrefs.SetString("PlayerName", Field.text);
            SceneChanger.Instance.ChangeScene("Main");
        }
    }
}