using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VIDE_Data;

namespace Rescues
{
    public class DialogueUIController : IInitializeController, ITearDownController
    {
        #region Fields

        private const string EXTRA_DATA_FOR_INVENTORY = "ExtraData";
        private readonly GameContext _context;
        private readonly Services _services;
        private DialogueUI _dialogueUI;
        private float _timeForWriteChar;
        private List<ITimeRemaining> _sequence;

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
            var dialogues = _context.GetTriggers(InteractableObjectType.Dialogue);
            foreach (var trigger in dialogues)
            {
                var dialogueBehaviour = trigger as InteractableObjectBehavior;
                dialogueBehaviour.OnFilterHandler += OnFilterHandler;
                dialogueBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                dialogueBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }

            _dialogueUI = Object.FindObjectOfType<DialogueUI>(true);
            _dialogueUI.destroyClip = Resources.Load<DestroyClip>
                ($"{AssetsPathGameObject.Object[GameObjectType.DialoguesComponents]}/DestroyClip");
            _dialogueUI.playerLabel.text = _context.character.Name;
            _dialogueUI.playerLabel.color = _dialogueUI.playerLabelColor;
            _dialogueUI.dialogContainer.SetActive(false);
            //Время на набор одного символа текста диалога, в данный момент технически не может превышать
            //Time.deltaTime, однако теоретически скорость можно увеличить
            _timeForWriteChar = _services.UnityTimeServices.DeltaTime();
            _sequence = new List<ITimeRemaining>();
            _context.dialogueUI = this;

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


        #region Methods

        public void Interact(VIDE_Assign assignDialog)
        {
            if (!VD.isActive)
            {
                Begin(assignDialog);
            }
            else
            {
                CallNext();
            }
        }

        private void Begin(VIDE_Assign dialogue)
        {
            VD.OnNodeChange += UpdateUI;
            VD.OnActionNode += ActionHandler;
            VD.OnEnd += End;

            VD.BeginDialogue(dialogue);
            _dialogueUI.dialogContainer.SetActive(true);
        }

        private void UpdateUI(VD.NodeData data)
        {
            _dialogueUI.playerContainer.SetActive(false);
            _dialogueUI.npcContainer.SetActive(false);
            SetSound(data);

            if (data.isPlayer)
            {
                SetPlayerName(data);
                SetSprite(data, _dialogueUI.playerImage, VD.assigned.defaultPlayerSprite, "SetPlayerSprite");
                SetPlayerChoices(data);

                _dialogueUI.playerContainer.SetActive(true);
            }
            else
            {
                _dialogueUI.npcText.text = "";
                SetNpcName(data);
                SetSprite(data, _dialogueUI.npcImage, VD.assigned.defaultNPCSprite, "SetNpcSprite");

                if (data.extraVars.ContainsKey("CheckItem"))
                {
                    ChangeStartNode(data.extraVars["CheckItem"].ToString());
                }

                if (data.audios[data.commentIndex] != null)
                {
                    _dialogueUI.dialogueSound.clip = data.audios[data.commentIndex];
                    _dialogueUI.dialogueSound.Play();
                }

                DrawText(data.comments[data.commentIndex], _timeForWriteChar);
                _dialogueUI.npcContainer.SetActive(true);
            }

            SetBackground(data);
        }

        private void CallNext()
        {
            if (TimeRemainingExtensions.SequentialTimeRemainings.Contains(_sequence))
            {
                CutTextAnimation();
                return;
            }

            VD.Next(); 
        }

        private void CutTextAnimation()
        {
            TimeRemainingExtensions.RemoveSequentialTimeRemaining(_sequence);
            _dialogueUI.npcText.text = VD.nodeData.comments[VD.nodeData.commentIndex];		
        }

        private void ChangeStartNode(string itemName)
        {
            if (_context.inventory.Contains(new ItemData()))//TODO empty shell, need parser from item name
            {
                VD.assigned.overrideStartNode = int.Parse(VD.nodeData.extraVars["Yes"].ToString());
            }
            else
            {
                VD.assigned.overrideStartNode = int.Parse(VD.nodeData.extraVars["No"].ToString());
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

        private void SetPlayerName(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("SetPlayerName"))
            {
                _dialogueUI.playerLabel.text = data.extraVars["SetPlayerName"].ToString();
            }
            else
            {
                _dialogueUI.playerLabel.text = _context.character.Name;
            }
        }

        private void SetNpcName(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("SetNpcName"))
            {
                _dialogueUI.npcLabel.text = data.extraVars["SetNpcName"].ToString();
            }
            else
            {
                _dialogueUI.npcLabel.text = VD.assigned.alias;
            }
        }

        private void SetSprite(VD.NodeData data, Image coreImage, Sprite deffaultSprite, string command)
        {
            if (data.sprites[data.commentIndex] != null && data.extraVars.ContainsKey(command))
            {
                if (data.commentIndex == (int)data.extraVars[command])
                {
                    coreImage.sprite = data.sprites[data.commentIndex];
                }
                else
                {
                    coreImage.sprite = deffaultSprite;
                }
            }
            else
            {
                coreImage.sprite = deffaultSprite;
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
                    var i1 = i;
                    _dialogueUI.playerTextChoices[i].AddListener(() => SetPlayerChoice(i1));
                    if (!EXTRA_DATA_FOR_INVENTORY.Equals(data.extraData[i]))
                    {
                        var items = _context.GetTriggers(InteractableObjectType.Item);
                        foreach (ItemBehaviour item in items)
                        {
                            if (item.ItemData.Name.ToLower() == data.extraData[i1].ToLower())
                            {
                                _dialogueUI.playerTextChoices[i].AddListener(() =>
                                _context.inventory.AddItem(item.ItemData));
                            }
                        }
                    }
                }
                else
                {
                    _dialogueUI.playerTextChoices[i].Disable();
                }
            }
        }

        private void SetPlayerChoice(int choice)
        {
            VD.nodeData.commentIndex = choice;
            VD.Next();
        }

        private void SetSound(VD.NodeData data)
        {
            if (data.extraVars.ContainsKey("PlayMusic"))
            {
                var destroyClip = Object.Instantiate(_dialogueUI.destroyClip);
                destroyClip.Initialization(Resources.Load<AudioClip>
                    ($"{AssetsPathGameObject.Object[GameObjectType.DialoguesComponents]}/" +
                    $"{VD.nodeData.extraVars["PlayMusic"]}"));
            }
        }

        private void End(VD.NodeData data)
        {
            VD.OnNodeChange -= UpdateUI;
            VD.OnActionNode -= ActionHandler;
            VD.OnEnd -= End;
            _dialogueUI.dialogContainer.SetActive(false);
            VD.EndDialogue();
        }

        private void DrawText(string text, float time)
        {
            _sequence.Clear();
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
            TimeRemainingExtensions.AddSequentialTimeRemaining(_sequence);
        }

        //for debug purposes
        private void ActionHandler(int nodeid)
        {
            // Debug.Log(nodeid);
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
                materialColor.g, materialColor.b, 0.5f), 1.0f);
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
