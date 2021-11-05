using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rescues
{
    public sealed class SaveLoadBaseBehaviour: MonoBehaviour, IPointerDownHandler
    {
        public InputField InputField;
        //public event Action<string> InputFieldSelected;
        public InputField.SubmitEvent se = new InputField.SubmitEvent();
        public void OnPointerDown(PointerEventData eventData)
        {
            var input = gameObject.GetComponent<InputField>();
            input.onEndEdit = se;
            //InputFieldSelected?.Invoke(InputField);
        }//.GetComponentsInChildren<Text>()[1].text
    }
}