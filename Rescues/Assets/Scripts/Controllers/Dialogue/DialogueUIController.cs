using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using VIDE_Data;


namespace Rescues
{
    public sealed class DialogueUIController : IInitializeController, ITearDownController, IExecuteController
    {
        #region Fields

        private const string EXTRA_DATA_DEFAULT = "ExtraData";
        private const float DIMMING_FACTOR = 0.7f;
        private readonly GameContext _context;
        private readonly Services _services;
        private DialogueUI _dialogueUI;
        private float _timeForWriteChar;
        private List<ITimeRemaining> _sequence;
        private List<IInteractable> _items;

        #endregion


        #region ClassLifeCycles

        public DialogueUIController(GameContext context, Services services)
        {
            _context = context;
            _services = services;
        }

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _items = _context.GetTriggers(InteractableObjectType.Item);
            var dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (var dialogue in dialogues)
            {
                var dialogueBehaviour = dialogue as InteractableObjectBehavior;
                dialogueBehaviour.OnFilterHandler += OnFilterHandler;
                dialogueBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                dialogueBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }

            _dialogueUI = Object.FindObjectOfType<DialogueUI>(true);
            _dialogueUI.dialogContainer.SetActive(false);
            _dialogueUI.npcImage.color = _dialogueUI.npcImageNormalColor;
            _dialogueUI.playerImage.color = _dialogueUI.playerImageNormalColor;
            //Возможно расширение возможности и добавление команды в ExtraVars для дополнительного вызова
            SetNameColor();
            //Время на набор одного символа текста диалога, в данный момент технически не может превышать
            //Time.deltaTime, однако теоретически скорость можно увеличить
            _timeForWriteChar = _services.UnityTimeServices.DeltaTime();
            _sequence = new List<ITimeRemaining>();
            _context.dialogueUIController = this;

            VD.LoadDialogues();
        }

        #endregion


        #region ITearDownController

        public void TearDown()
        {
            var dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (var trigger in dialogues)
            {
                var dialogueBehaviour = trigger as InteractableObjectBehavior;
                dialogueBehaviour.OnFilterHandler -= OnFilterHandler;
                dialogueBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                dialogueBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion


        #region IExecuteController

        public void Execute()
        {
            if (VD.isActive && !VD.nodeData.isPlayer && _dialogueUI.npcText.text != "" && Input.GetButtonUp("Fire1"))
            {
                CallNext();
            }

            if (VD.isActive && VD.nodeData.isPlayer)
            {
                if (Input.GetKeyUp(KeyCode.Alpha1))
                {
                    SetPlayerChoice(0);
                }

                if (Input.GetKeyUp(KeyCode.Alpha2))
                {
                    SetPlayerChoice(1);
                }

                if (Input.GetKeyUp(KeyCode.Alpha3))
                {
                    SetPlayerChoice(2);
                }

                if (Input.GetKeyUp(KeyCode.Alpha4))
                {
                    SetPlayerChoice(3);
                }

                if (Input.GetKeyUp(KeyCode.Alpha5))
                {
                    SetPlayerChoice(4);
                }
            }
        }

        #endregion


        #region Methods

        public void Begin(VIDE_Assign dialogue)
        {
            _dialogueUI.playerImage.sprite = dialogue.defaultPlayerSprite;
            _dialogueUI.npcImage.sprite = dialogue.defaultNPCSprite;
            _dialogueUI.playerLabel.text = _context.character.Name;
            _dialogueUI.npcLabel.text = dialogue.alias;

            VD.OnNodeChange += UpdateUI;
            VD.OnNodeChange += SetName;
            VD.OnNodeChange += PlayNodeSound;
            VD.OnNodeChange += GivePlayerItem;
            VD.OnNodeChange += CheckItem;
            VD.OnNodeChange += SetBackground;
            VD.OnEnd += End;

            VD.BeginDialogue(dialogue);
            _dialogueUI.dialogContainer.SetActive(true);
            _dialogueUI.playerContainer.SetActive(true);
            _dialogueUI.npcContainer.SetActive(true);
        }

        public void End(VD.NodeData data)
        {
            VD.OnNodeChange -= UpdateUI;
            VD.OnNodeChange -= SetName;
            VD.OnNodeChange -= PlayNodeSound;
            VD.OnNodeChange -= GivePlayerItem;
            VD.OnNodeChange -= CheckItem;
            VD.OnNodeChange -= SetBackground;
            VD.OnEnd -= End;

            CutTextAnimation();
            _dialogueUI.dialogContainer.SetActive(false);
            VD.EndDialogue();
        }

        private void UpdateUI(VD.NodeData data)
        {
            if (data.isPlayer)
            {
                if (_dialogueUI.npcImage.color == _dialogueUI.npcImageNormalColor)
                {
                    _dialogueUI.npcImage.color *= DIMMING_FACTOR;
                    _dialogueUI.playerImage.color = _dialogueUI.playerImageNormalColor; 
                }
                SetPlayerChoices(data);
            }
            else
            {
                if (_dialogueUI.playerImage.color == _dialogueUI.playerImageNormalColor)
                {
                    _dialogueUI.playerImage.color *= DIMMING_FACTOR; 
                    _dialogueUI.npcImage.color = _dialogueUI.npcImageNormalColor;
                }

                foreach (var choise in _dialogueUI.playerTextChoices)
                {
                    choise.Disable();
                }

                DrawText(data.comments[data.commentIndex], _timeForWriteChar);
            }
        }

        private void CallNext()
        {
            CutTextAnimation();
            VD.Next();
        }

        private bool CutTextAnimation()
        {
            if (TimeRemainingExtensions.SequentialTimeRemainings.sequentialTimeRemainings.Contains(_sequence))
            {
                TimeRemainingExtensions.RemoveSequentialTimeRemaining(_sequence);
                _dialogueUI.npcText.text = VD.nodeData.comments[VD.nodeData.commentIndex];

                return true;
            }

            return false;
        }

        private void CheckItem(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("CheckItem"))
            {
                if (data.extraVars.ContainsKey("Yes"))
                {
                    foreach (var itemSlot in _context.inventory.itemSlots)
                    {
                        if (itemSlot.Item?.Name.ToLower() == data.extraVars["CheckItem"].ToString().ToLower())
                        {
                            VD.assigned.overrideStartNode = (int)VD.nodeData.extraVars["Yes"];
                            return;
                        }
                    }
                }

                if (data.extraVars.ContainsKey("No"))
                {
                    VD.assigned.overrideStartNode = (int)VD.nodeData.extraVars["No"];
                }
            }
        }

        private void SetBackground(VD.NodeData data)
        {
            var dataSprite = data.sprite;
            if (dataSprite)
            {
                _dialogueUI.background.sprite = dataSprite;
            }
        }

        private void SetNameColor()
        {
            _dialogueUI.playerLabel.color = _dialogueUI.playerLabelColor;
            _dialogueUI.npcLabel.color = _dialogueUI.npcLabelColor;
        }

        private void SetName(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("SetNpcName"))
            {
                _dialogueUI.npcLabel.text = data.extraVars["SetNpcName"].ToString();
            }

            if (data.extraVars.ContainsKey("SetPlayerName"))
            {
                _dialogueUI.playerLabel.text = data.extraVars["SetPlayerName"].ToString();
            }
        }

        private void SetSprite(VD.NodeData data, int choise)
        {
            if (data.creferences[choise].sprites != null && data.creferences[choise].extraData != EXTRA_DATA_DEFAULT)
            {
                if (data.creferences[choise].extraData == "SetNpcSprite")
                {
                    _dialogueUI.npcImage.sprite = data.creferences[choise].sprites;
                }

                if (data.creferences[choise].extraData == "SetPlayerSprite")
                {
                    _dialogueUI.playerImage.sprite = data.creferences[choise].sprites;
                }
            }
        }

        private void SetPlayerChoices(VD.NodeData data)
        {
            for (int i = 0; i < _dialogueUI.playerTextChoices.Length; i++)
            {
                _dialogueUI.playerTextChoices[i].RemoveAllListeners();
                if (i < data.comments.Length)
                {
                    _dialogueUI.playerTextChoices[i].Enable();
                    _dialogueUI.playerTextChoices[i].Text = data.comments[i];
                    var temp = i;
                    _dialogueUI.playerTextChoices[i].AddListener(() => SetPlayerChoice(temp));
                    _dialogueUI.playerTextChoices[i].AddListener(() => SetSprite(data, temp));
                }
                else
                {
                    _dialogueUI.playerTextChoices[i].Disable();
                }
            }
        }

        private void GivePlayerItem(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("GiveItem"))
            {
                foreach (ItemBehaviour item in _items)
                {
                    if (item.ItemData?.Name.ToLower() == data.extraVars["GiveItem"].ToString().ToLower())
                    {
                        item.gameObject.SetActive(false);
                        _context.inventory.AddItem(item.ItemData);
                    }
                }
            }
        }

        private void SetPlayerChoice(int choice)
        {
            if (choice < VD.nodeData.comments.Length && choice >= 0)
            {
                VD.nodeData.commentIndex = choice;
                VD.Next();
            }
        }

        private void PlayNodeSound(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("PlayMusic"))
            {
                var path = ($"{AssetsPathGameObject.Object[GameObjectType.DialoguesComponents]}" +
                    $"{VD.nodeData.extraVars["PlayMusic"]}");
                _dialogueUI.nodeSoundContainer.Initialization(Resources.Load<AudioClip>(path));

            }
        }

        private void DrawText(string text, float time)
        {
            _sequence.Clear();
            _dialogueUI.npcText.text = "";
            for (int i = 0; i < text.Length; i++)
            {
                float lastHeight = _dialogueUI.npcText.preferredHeight;
                if (_dialogueUI.npcText.preferredHeight > lastHeight)
                {
                    _dialogueUI.npcText.text += System.Environment.NewLine;
                }

                var tempLetter = text[i];
                _sequence.Add(new TimeRemaining(() =>
                {
                    _dialogueUI.npcText.text += tempLetter;
                },
                time));
            }
            _sequence.Add(new TimeRemaining(() => { VD.Next(); }, time));
            TimeRemainingExtensions.AddSequentialTimeRemaining(_sequence);
        }

        #endregion


        #region InteractableObjectMethods

        private bool OnFilterHandler(Collider2D obj)
        {
            return obj.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
            var materialColor = enteredObject.GameObject.GetComponent<SpriteRenderer>().color;
            enteredObject.GameObject.GetComponent<SpriteRenderer>().DOColor(new Color(materialColor.r,
                materialColor.g * 1.2f, materialColor.b, 1f), 1.0f);
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
            var materialColor = enteredObject.GameObject.GetComponent<SpriteRenderer>().color;
            enteredObject.GameObject.GetComponent<SpriteRenderer>().DOColor(new Color(materialColor.r,
                materialColor.g, materialColor.b, 1.0f), 1.0f);
        }

        #endregion
    }
}
