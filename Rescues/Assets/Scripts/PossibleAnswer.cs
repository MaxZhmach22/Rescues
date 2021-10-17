using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DefaultNamespace
{
    internal sealed class PossibleAnswer : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private Button _button;

        public string Text
        {
            set => _text.text = value;
        }

        private void Awake()
        {
            _button = GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Select()
        {
            _button.Select();
        }

        public void RemoveAllListeners()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void AddListener(Action action)
        {
            _button.onClick.AddListener(action.Invoke);
        }
    }
}
