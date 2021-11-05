
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rescues
{
    public sealed class SaveLoadBaseBehaviour: MonoBehaviour, IPointerDownHandler
    {
        public InputField InputField;
        public event Action<string> InputFieldSelected;

        public void OnPointerDown(PointerEventData eventData)
        {
            InputFieldSelected?.Invoke(InputField.GetComponentsInChildren<Text>()[1].text);
        }
    }
}