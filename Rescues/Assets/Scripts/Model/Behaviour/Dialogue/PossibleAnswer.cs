using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Rescues
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [RequireComponent(typeof(Button))]
    public sealed class PossibleAnswer : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _button;

        #endregion


        #region Properties

        public string Text
        {
            set => _text.text = value;
        }

        #endregion


        #region Methods

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

        #endregion
    }
}
