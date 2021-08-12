using System;
using Rescues;
using UnityEngine;

namespace Rescues
{
    public class ChessPuzzle: Puzzle
    {
        #region Fileds

        [SerializeField] private ChessPuzzleData _chessPuzzleData;
        private ChessBoard _chessBoard;
        private string _playersSequence;
        private bool _isPlayerRight;
        #endregion
        
        
        #region  Propeties

        public ChessBoard ChessBoard => _chessBoard;
        
        
        #endregion


        #region UnityMethods

        private void Start()
        {
            _chessBoard = gameObject.GetComponentInChildren<ChessBoard>();
            _chessBoard._chessPuzzleData = _chessPuzzleData;
            _chessBoard.Loaded += BoardLoading;
            _chessBoard.FigurePlacedOnNewPosition += LookingAtSequence;
        }
        private void OnDestroy()
        {
            _chessBoard.Loaded -= BoardLoading;
            _chessBoard.FigurePlacedOnNewPosition -= LookingAtSequence;
        }

        private void OnEnable()
        {
           
            // if (_startPositions.Count == 0)
            // {
            //     _wirePoints = GetComponentsInChildren<WirePoint>().ToList();
            //     foreach (var wirePoint in _wirePoints)
            //     {
            //         _startPositions.Add(wirePoint.GetHashCode(), wirePoint.transform.localPosition);
            //     }
            // }
        }

        #endregion

        #region Methods

        private void BoardLoading()
        {
            Debug.Log(_chessBoard);
            _chessBoard.SetPuzzledFigures();
        }

        private void LookingAtSequence()
        {
            Debug.Log("Смотрю на последовательность(нет)");
        }

        #endregion
        
    }
}