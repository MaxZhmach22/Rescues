using System;
using UnityEngine;

namespace Rescues
{
    public class Figure: MonoBehaviour
    {
        #region Fields

        [SerializeField] private FigureStruct _figureStruct;

        private Vector2 _currentPositon;
        private bool _isMoving = false;
        public event Action OnPosition;

        #endregion

        #region UnityMethods

        private void OnMouseDown()
        {
            _isMoving = true;
        }

        private void OnMouseDrag()
        {
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MoveFigure(cursorPosition);
        }

        private void OnMouseUp()
        {
            _isMoving = false;
            OnPosition.Invoke();
        }
        
        #endregion

        #region Methods

        public void MoveFigure(Vector2 newPosition)
        {
            transform.position = newPosition;
        }

        public ChessPuzzleFiguresTypes GetType()
        {
            return _figureStruct.IndexOfFigure;
        }

        public void SetFigureCurrentPosition(
            int CurrentPositionX,
            int CurrentPositionY)
        {
            _figureStruct.CurrentPositionX = CurrentPositionX;
            _figureStruct.CurrentPositionY = CurrentPositionY;
        }

        public Vector2 GetFigureCurrentPosition()
        {
            return new Vector2(_figureStruct.CurrentPositionX, _figureStruct.CurrentPositionY);
        }
        
        public void SetFigureStartInfo(
         int CurrentPositionX,
         int CurrentPositionY
        )
        {
            _figureStruct.CurrentPositionX = CurrentPositionX;
            _figureStruct.CurrentPositionY = CurrentPositionY;
            _figureStruct.EndPositionX = CurrentPositionX;
            _figureStruct.EndPositionY = CurrentPositionY;
        }
        #endregion

    }
}