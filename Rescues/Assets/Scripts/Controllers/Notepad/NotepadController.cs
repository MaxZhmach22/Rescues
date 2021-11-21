using UnityEngine;


namespace Rescues
{
    public sealed class NotepadController : IInitializeController, ITearDownController, IExecuteController
    {
        #region Fields
        private readonly GameContext _context;
        private NotepadBehaviour _notepadBehaviour;
        #endregion

        #region ClassLifeCycles
        public NotepadController(GameContext context)
        {
            _context = context;
        }

        #endregion

        #region IInitializeController

        public void Initialize()
        {           
            _notepadBehaviour = Object.FindObjectOfType<NotepadBehaviour>(true);
            _notepadBehaviour.Initialize();
            _context.notepad = _notepadBehaviour;

            var notepadTriggers = _context.GetTriggers(InteractableObjectType.NotepadTrigger);
            foreach (var trigger in notepadTriggers)
            {
                var notepadTriggerBehaviour = trigger as InteractableObjectBehavior;
                notepadTriggerBehaviour.OnFilterHandler += OnFilterHandler;
                notepadTriggerBehaviour.OnTriggerEnterHandler += OnTriggerEnterHandler;
                notepadTriggerBehaviour.OnTriggerExitHandler += OnTriggerExitHandler;
            }
        }

        #endregion

        #region ITearDownController

        public void TearDown()
        {
            var notepadTriggers = _context.GetTriggers(InteractableObjectType.NotepadTrigger);
            foreach (var trigger in notepadTriggers)
            {
                var notepadTriggerBehaviour = trigger as InteractableObjectBehavior;
                notepadTriggerBehaviour.OnFilterHandler -= OnFilterHandler;
                notepadTriggerBehaviour.OnTriggerEnterHandler -= OnTriggerEnterHandler;
                notepadTriggerBehaviour.OnTriggerExitHandler -= OnTriggerExitHandler;
            }
        }

        #endregion

        #region IExecuteController

        public void Execute()
        {
            if (_notepadBehaviour.isActiveAndEnabled)
                _notepadBehaviour.ShowPageButtons();
        }

        #endregion

        #region Methods

        private bool OnFilterHandler(Collider2D obj)
        {
            return obj.CompareTag(TagManager.PLAYER);
        }

        private void OnTriggerEnterHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = true;
        }

        private void OnTriggerExitHandler(ITrigger enteredObject)
        {
            enteredObject.IsInteractable = false;
        }

        #endregion
    }
}
