using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Rescues
{
    public sealed class InitializeSaveLoadController : IInitializeController
    {
        #region Fields

        private readonly GameContext _context;
        private readonly PhysicalServices _physicServices;
        
        private GameSavingSerializer _gameSavingSerializer;

        #endregion

        #region ClassLifeCycles

        public InitializeSaveLoadController(GameContext context, Services services)
        {
            _context = context;
            _physicServices = services.PhysicalServices;
            _context.WorldGameData = new WorldGameData();
            _gameSavingSerializer = new GameSavingSerializer();
        }

        #endregion

        #region IInitializeController

        public void Initialize()
        {
            _context.saveLoadBehaviour = Object.FindObjectOfType<SaveLoadBehaviour>();
            _context.saveLoadBehaviour.ReEnable += UpdateListOfSaves;
            _context.gameMenu.CalledSaveLoad += _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.BackAction += _context.saveLoadBehaviour.SwitchState;
            _context.saveLoadBehaviour.Saving += Saving;
            _context.saveLoadBehaviour.Loading += Loading;
            _context.saveLoadBehaviour.gameObject.SetActive(false);
        }
        
        #endregion

        #region Methods

        private void Loading(string obj)
        {
            _gameSavingSerializer.Load(obj);
        }

        private void Saving(string name)
        {
            _gameSavingSerializer.Save(_context.WorldGameData,name);
        }

        private void UpdateListOfSaves()
        {
            _context.saveLoadBehaviour.FileContexts = _gameSavingSerializer.GetAllSaves();
        }

        #endregion
    }
}