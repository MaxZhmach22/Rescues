using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

namespace Rescues
{
    public sealed class SaveLoadBehaviour: BaseUi
    {
        #region Fields
        
        [SerializeField] private GameObject _scrollView;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _saveLoadButton;
        private List<SaveLoadBaseBehaviour> _listOfInputField=new List<SaveLoadBaseBehaviour>();
        private SaveLoadingPanelFactory _saveLoadingPanelFactory = new SaveLoadingPanelFactory();
        private string _selectedInputInfo;
        
        [NonSerialized] public bool SaveOrLoad=true;
        public IEnumerable<FileContext> FileContexts;
        public event Action BackAction;
        public event Action ReEnable;
        public event Action<string> Saving;
        public event Action<string> Loading;

        #endregion

        #region UnityMethods
        private void OnEnable()
        {
            _backButton.onClick.AddListener(BackButtonClick);

            ReEnable?.Invoke();
            if (FileContexts!=null)
            {
                //loading data
                foreach (var fileName in FileContexts)
                {
                    var newOne = _saveLoadingPanelFactory.Create("SaveLoadingBase", _scrollView.transform);
                    _listOfInputField.Add(newOne.gameObject.GetComponentInChildren<SaveLoadBaseBehaviour>());
                    _listOfInputField.Last().InputField.text = fileName.FileName.Split('/','\\').Last();
                    _listOfInputField.Last().InputField.readOnly = true;
                    _listOfInputField.Last().InputFieldSelected += SelectedInput;
                }
                if (SaveOrLoad)
                {
                    _saveLoadButton.onClick.AddListener(SaveButtonClick);
                    _listOfInputField.Add(
                        _saveLoadingPanelFactory.Create("SaveLoadingBase", _scrollView.transform)
                            .gameObject.GetComponentInChildren<SaveLoadBaseBehaviour>());
                    _listOfInputField.Last().InputField.readOnly = false;
                    _listOfInputField.Last().InputFieldSelected += SelectedInput;
                }
                else
                    _saveLoadButton.onClick.AddListener(LoadButtonClick);
                
                SetSaveLoad(SaveOrLoad);
            }
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(BackButtonClick);
            _saveLoadButton.onClick?.RemoveAllListeners();
            foreach (var behaviour in _listOfInputField)
            {
                Destroy(behaviour.transform.parent.gameObject);
            }
            _listOfInputField.Clear();
        }

        #endregion

        #region Methods
        
        private void SelectedInput(string obj)
        {
            _selectedInputInfo = obj;
        }
        public override void Show()
        {
            gameObject.SetActive(true);
            ShowUI.Invoke();
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
            HideUI.Invoke();
        }
        
        public void SwitchState()
        {
            if (gameObject.activeSelf)
                Hide();
            else
                Show();
        }
        public void SwitchState(bool state)
        {
            if (gameObject.activeSelf)
                Hide();
            else
                Show();
        }
        public void SetSaveLoad(bool state)
        {
            SaveOrLoad = state;
            if(state)
                _saveLoadButton.GetComponentInChildren<Text>().text = "Save";
            else
                _saveLoadButton.GetComponentInChildren<Text>().text = "Load";
        }
        private void BackButtonClick()
        {
            BackAction?.Invoke();
        }

        private void LoadButtonClick()
        {
            Loading?.Invoke(_selectedInputInfo);
        }

        private void SaveButtonClick()
        {
            Saving?.Invoke(_selectedInputInfo);
        }
        #endregion
    }
}