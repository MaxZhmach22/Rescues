using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using VIDE_Data;
using UnityEngine.UI;
 
internal sealed class UIManager : MonoBehaviour
{
    private const string EXTRA_DATA_FOR_INVENTORY = "ExtraData";
    [Header("Npc")]
    public GameObject _containerNpc;
    public Image _imageNpc;
    public Image _imageNpcIcon;
    public Color _enableColorNpc;
    public Color _disableColorNpc;
    public TextMeshProUGUI _textNpc;
    
    [Header("Player"), Space(10)]
    public GameObject _containerPlayer;
    public TextMeshProUGUI _playerName;
    public Color _playerNameColor;
    public PossibleAnswer[] _textChoices;
    
    [Header("Other"), Space(10)]
    public Image _background;
    public AudioSource _audioSource;
    
    [Header("Test"), Space(10)]
    public KeyCode _continueButton = KeyCode.Space;
    
    private Player _player;
    private VIDE_Assign _videAssign;
    private DestroyClip _destroyClip;

    private void Start()
    {
        _videAssign = GetComponent<VIDE_Assign>();
        _player = FindObjectOfType<Player>();
        _destroyClip = Resources.Load<DestroyClip>("Other/DestroyClip");
        _playerName.text = _player.PlayerName;
        _playerName.color = _playerNameColor;
        _containerPlayer.SetActive(false);
        _containerNpc.SetActive(false);
        _imageNpc.gameObject.SetActive(false);
        VD.LoadDialogues();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_continueButton))
        {
            if (!VD.isActive)
            {
                Begin();
            }
            else
            {
                VD.Next();
            }
        }
    }

    private void Begin()
    {
        VD.OnNodeChange += UpdateUI;
        VD.OnActionNode += ActionHandler;
        VD.OnEnd += End;
        VD.BeginDialogue(_videAssign);
    }

    private void UpdateUI(VD.NodeData data)
    {
        SetPlayerName();
        SetSound();
        
        _containerPlayer.SetActive(false);
        if (data.isPlayer)
        {
            _containerPlayer.SetActive(true);
            _imageNpc.color = _disableColorNpc;
            for (int i = 0; i < _textChoices.Length; i++)
            {
                _textChoices[i].RemoveAllListeners();
                if (i < data.comments.Length)
                {
                    _textChoices[i].Enable();
                    _textChoices[i].Text = data.comments[i];
                    var i1 = i;
                    _textChoices[i].AddListener(() => SetPlayerChoice(i1));
                    if (!EXTRA_DATA_FOR_INVENTORY.Equals(data.extraData[i]))
                    {
                        _textChoices[i].AddListener(() => _player.AddItemInventory(data.extraData[i1]));
                    }
                }
                else
                {
                    _textChoices[i].Disable();
                }
            }
            _textChoices[0].Select();
        }
        else
        {
            _containerNpc.SetActive(true);
            _imageNpc.gameObject.SetActive(true);
            _textNpc.text = data.comments[data.commentIndex];
            if (CheckInventory())
            {
                _textNpc.text += $" {VD.nodeData.extraVars["CheckItem"]}?";
            }
            var dataSpriteNpc = data.sprites[data.commentIndex]; 
            _imageNpc.gameObject.SetActive(dataSpriteNpc);
            if (dataSpriteNpc)
            {
                _imageNpc.sprite = dataSpriteNpc;
                _imageNpc.color = _enableColorNpc;
            }

            var imageNpcIcon = Resources.Load<Sprite>($"Art/Icon/{data.tag}");
            _imageNpcIcon.sprite = imageNpcIcon;
            if (data.audios[data.commentIndex] != null)
            {
                _audioSource.clip = data.audios[data.commentIndex];
            }

            _audioSource.Play();
        }
        
        SetBackground(data);
    }

    private void SetBackground(VD.NodeData data)
    {
        var dataSprite = data.sprite;
        if (dataSprite)
        {
            _background.sprite = dataSprite;
        }
    }

    private void SetPlayerName()
    {
        if (VD.nodeData.extraVars.ContainsKey("Name"))
        {
            _playerName.text = VD.nodeData.extraVars["Name"].ToString();
        }
        else
        {
            _playerName.text = _player.PlayerName;
        }
    }

    private void SetSound()
    {
        if (VD.nodeData.extraVars.ContainsKey("PlayMusic"))
        {
            var destroyClip = Instantiate(_destroyClip);
            destroyClip.Initialization(
                Resources.Load<AudioClip>($"Sound/{VD.nodeData.extraVars["PlayMusic"]}"));
        }
    }

    private void End(VD.NodeData data)
    {
        _containerNpc.SetActive(false);
        _containerPlayer.SetActive(false);
        VD.OnNodeChange -= UpdateUI;
        VD.OnActionNode -= ActionHandler;
        VD.OnEnd -= End;
        VD.EndDialogue();
    }

    private void OnDisable()
    {
        if (_containerNpc != null)
            End(null);
    }
 
    private void SetPlayerChoice(int choice)
    {
        VD.nodeData.commentIndex = choice; 
        VD.Next();
    }

    public bool CheckInventory()
    {
        if (VD.nodeData.extraVars.ContainsKey("CheckItem"))
        {
            if (_player.Inventory.Contains(VD.nodeData.extraVars["CheckItem"].ToString()))
            {
                VD.assigned.overrideStartNode = Int32.Parse(VD.nodeData.extraVars["Yes"].ToString());
            }
            else
            {
                VD.assigned.overrideStartNode = Int32.Parse(VD.nodeData.extraVars["No"].ToString());
            }
            // VD.Next();
            return true;
        }
        
        return false;
    }

    private void ActionHandler(int nodeid)
    {
        // Debug.Log(nodeid);
    }
}