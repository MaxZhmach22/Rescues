using UnityEngine;


namespace Rescues
{
    public sealed class InitializeGameMenuController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly PhysicsService _physicServices; 

        #endregion


        #region ClassLifeCycles

        public InitializeGameMenuController(GameContext context, Services services)
        {
            _context = context;
            _physicServices = services.PhysicsService;
        } 

        #endregion


        #region IInitializeController

        public void Initialize()
        {
            _context.gameMenu = Object.FindObjectOfType<GameMenuBehaviour>();
            _context.gameMenu.ShowUI += _physicServices.PauseSwitch;
            _context.gameMenu.HideUI += _physicServices.PauseSwitch;
            _context.gameMenu.gameObject.SetActive(false);
        } 

        #endregion
    }
}
