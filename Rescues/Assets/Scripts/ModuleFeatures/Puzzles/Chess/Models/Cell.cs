using System;
using UnityEngine;

namespace Rescues
{
    public class Cell: MonoBehaviour
    {
        #region Flieds

        private ChessPuzzleFiguresTypes _cellType = ChessPuzzleFiguresTypes.None;
        private int _idOfFigurePlacedOnCell = 0;
        private Collider2D _currentCollider;
        private Figure _currentFigure;
        
        //небольшой костыль. хотя в теории норм решение,но...ну такое,но хз как заменить нормально.
        private Figure _ifBlockedFigure;
        
        public int IndexX;
        public int IndexY;
        #endregion

        #region UnityMethods
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //нет отключения игрока.
            if (!other.CompareTag("Player"))
            {
                if (_cellType == ChessPuzzleFiguresTypes.None&&
                    (_idOfFigurePlacedOnCell==0||_idOfFigurePlacedOnCell==other.gameObject.GetInstanceID()))
                {
                    _currentFigure = other.gameObject.GetComponent<Figure>();
                    _currentFigure.SetFigureCurrentPosition(IndexX,IndexY);
                    _currentFigure.OnPosition += PlaceAFigureInCell;
                    _currentCollider = other;
                    _idOfFigurePlacedOnCell = other.gameObject.GetInstanceID();
                }
                else
                {
                    _ifBlockedFigure = other.gameObject.GetComponent<Figure>();
                    _ifBlockedFigure.OnPosition += Back;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                if (_idOfFigurePlacedOnCell==0||_idOfFigurePlacedOnCell==other.gameObject.GetInstanceID())
                {
                    _cellType = ChessPuzzleFiguresTypes.None;
                    _idOfFigurePlacedOnCell = 0;
                    other.gameObject.GetComponent<Figure>().OnPosition -= PlaceAFigureInCell;
                }
            }
        }

        #endregion

        #region Methods

        public ChessPuzzleFiguresTypes GetTypeOfCell()
        {
            return _cellType;
        }
        
        public void SetTypeOfCell(ChessPuzzleFiguresTypes type)
        {
            _cellType = type;
        }

        private void Back()
        {
            Debug.Log(_ifBlockedFigure.GetFigureCurrentPosition());
            _ifBlockedFigure.gameObject.transform.position =
                _ifBlockedFigure.GetFigureCurrentPosition();
            _ifBlockedFigure.OnPosition -= Back;
        }
        private void PlaceAFigureInCell()
        {
            var gm = _currentCollider.gameObject;
            gm.transform.position = this.gameObject.transform.position;
            SetTypeOfCell(_currentFigure.GetType());
        }
        #endregion
        
        
    }
}