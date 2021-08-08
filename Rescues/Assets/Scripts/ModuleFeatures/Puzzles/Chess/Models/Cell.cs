using System;
using UnityEngine;

namespace Rescues
{
    public class Cell: MonoBehaviour
    {
        #region Flieds

        private ChessPuzzleFiguresTypes _cellType;

        #endregion
        
        #region UnityMethods
        
        private void OnTriggerEnter2D(Collider2D other)
        {
           
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        public ChessPuzzleFiguresTypes GetTypeOfCell()
        {
            return ChessPuzzleFiguresTypes.None;
        }

        public void SetTypeOfCell(ChessPuzzleFiguresTypes type)
        {
            _cellType = type;
        }
        
        #endregion
        
        
    }
}