using PathCreation;
using System;
using UnityEngine;


namespace Rescues
{
    public sealed class CharacterModel
    {
        #region Fields

        public Action AfterSceneLoad = () => { };

        private float _distance;
        private float _speed;
        private float _hidingDuration;
        private int _direction;

        private DragonBones.UnityArmatureComponent _characterArmature;
        private DragonBones.Animation _characterAnimation;

        private CapsuleCollider2D _playerCollider;
        private Rigidbody2D _playerRigidbody2D;

        private HidingPlaceBehaviour _hidingPlaceBehaviour;
        private CurveWay _curveWay;
        
        #endregion


        #region Properties

        public Transform Transform { get; }
        public AudioSource PlayerSound { get; }

        public Timer AnimationPlayTimer { get; }
        public Gate Gate { get; private set; }
        public InteractableObjectBehavior InteractableItem { get; private set; }
        public State PlayerState { get; private set; }


        #endregion


        #region ClassLifeCycle

        public CharacterModel(Transform transform, PlayerData playerData)
        {
            _speed = playerData.Speed;

            _characterArmature = transform.GetComponentInChildren<DragonBones.UnityArmatureComponent>();
            _characterAnimation = _characterArmature.animation;

            _playerCollider = transform.GetComponentInChildren<CapsuleCollider2D>();
            _playerRigidbody2D = transform.GetComponentInChildren<Rigidbody2D>();
            AnimationPlayTimer = new Timer();
            PlayerSound = transform.GetComponentInChildren<AudioSource>();
            Transform = transform;
        }

        #endregion


        #region StateMachine     

        public void StateIdle()
        {
            PlayerState = State.Idle;
            if (_characterAnimation.lastAnimationName != "Idle")
            {
                _characterAnimation.FadeIn("Idle"); 
            }
        }

        public void StateHideAnimation(HidingPlaceBehaviour hidingPlaceBehaviour)
        {
            if (hidingPlaceBehaviour != null)
            {
                _hidingPlaceBehaviour = hidingPlaceBehaviour;
            }
            PlayerState = State.HideAnimation;                                           
            StartHiding();
        }

        public void StateHiding()
        {                            
            if (Hide())
            {
                PlayerState = State.Hiding;                             
            }
            else
            {
                StateIdle();                
            }
        }

        public void StatePickUpAnimation(ItemBehaviour itemBehaviour)
        {
            InteractableItem = itemBehaviour;
            PlayerState = State.PickUpAnimation;
            AnimationPlayTimer.StartTimer(itemBehaviour.PickUpTime);           
        }

        public void StateCraftTrapAnimation(TrapBehaviour trapBehaviour)
        {
            InteractableItem = trapBehaviour;
            PlayerState = State.CraftTrapAnimation;
            AnimationPlayTimer.StartTimer(trapBehaviour.TrapInfo.BaseTrapData.CraftingTime);
        }

        public void StateTeleporting(Gate gate)
        {
            PlayerState = State.GoByGateWay;
            Gate = gate;
            AnimationPlayTimer.StartTimer(gate.LocalTransferTime);
        }

        public void StateMoving(int direction)
        {
            switch (PlayerState)
            {
                case State.Hiding:
                    {                       
                        return;
                    }
                case State.HideAnimation:
                    {                        
                        return;
                    }
                case State.PickUpAnimation:
                    {
                        return;
                    }
                case State.CraftTrapAnimation:
                    {
                        return;
                    }
                case State.GoByGateWay:
                    {
                        return;
                    }
            }

            if (direction != 0 && _characterAnimation.lastAnimationName != "Walking")
            {
                _characterAnimation.FadeIn("Walking"); 
            }

            PlayerState = State.Moving;
            _direction = direction;
        }
 

        public void StateHandler()
        {
            switch (PlayerState)
            {
                case State.Moving:
                    Move();
                    break;

                case State.GoByGateWay:
                    Gate.GoByGateWay();
                    break;
            }
        }

        #endregion


        #region Methods

        public void LocateCharacter(CurveWay curveWay)
        {
            _curveWay = curveWay;
            Transform.position = curveWay.GetStartPointPosition;
            CorrectDistance();
            if (AfterSceneLoad.GetInvocationList().Length <= 1)
            {
                AfterSceneLoad = null;
                AfterSceneLoad += SetScale;
                AfterSceneLoad += StateHandler;
                AfterSceneLoad += AnimationPlayTimer.UpdateTimer;
            }
        }

        private void SetScale()
        {
            Transform.localScale = _curveWay.GetScale(Transform.position);
        }

        private void StartHiding()
        {
            PlayerSound.clip = _hidingPlaceBehaviour.HidingPlaceData.HidingSound;
            _hidingDuration = _hidingPlaceBehaviour.HidingPlaceData.AnimationDuration;         
            PlayAnimationWithTimer();
            if (_playerRigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                _characterArmature.enabled = false; //чтобы спрайт выключался сразу, когда идет процесс пряток
            }
            else
            {
                _hidingPlaceBehaviour.HidedSprite.enabled = false; //чтобы спрайт хайдинг плейс бехевора выключался сразу, когда персонаж начинает вылезать
            }
        }

        private bool Hide()
        {
            bool isHided;
            _playerCollider.enabled = !_playerCollider.enabled;          
            if (_playerRigidbody2D.bodyType == RigidbodyType2D.Dynamic)
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Static;
                isHided = true;
                _hidingPlaceBehaviour.HidedSprite.enabled = true; //чтобы спрайт хайдинг плейс бехевора включался только тогда, когда персонаж спрятался
            }
            else
            {
                _playerRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                isHided = false;
                _characterArmature.enabled = true; //чтобы спрайт выключался только тогда, когда персонаж уже вылез
            }

            return isHided;
        }

        private void PlayAnimationWithTimer()
        {
            if (PlayerSound.clip != null)
            {
                PlayerSound.Play();
            }          
            AnimationPlayTimer.StartTimer(_hidingDuration);
        }

        private void CorrectDistance()
        {
            _distance = 0;
            //расчет сколько нам понадобится чтобы дойти до точки от 0(начало кривой).
            _distance = _curveWay.LeftmostPoint.x<0 ?
                _curveWay.StartCharacterPosition.x - _curveWay.LeftmostPoint.x:
                _curveWay.StartCharacterPosition.x + _curveWay.LeftmostPoint.x;
        }
        
        private void Move()
        {
            _distance += _direction * _speed * Time.deltaTime;
            Transform.position = _curveWay.PathCreator.path.GetPointAtDistance(_distance, EndOfPathInstruction.Stop);
            if (_direction == 0)
            {
                StateIdle();               
            }

            if (_direction > 0 && _characterArmature._armature.flipX)
            {
                Flip();               
            }
            else if (_direction < 0 && !_characterArmature._armature.flipX)
            {
                Flip();            
            }
        }
      
        private void Flip()
        {
            _characterArmature._armature.flipX = !_characterArmature._armature.flipX;
        }

        #endregion

       
    }
}
