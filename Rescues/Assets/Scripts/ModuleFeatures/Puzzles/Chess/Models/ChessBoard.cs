using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    public class ChessBoard: MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject[] _availableFigures;
        [SerializeField] private Transform _parentBoard;
        private Dictionary<ChessPuzzleFiguresTypes, GameObject> _availablePrefabsDictionary;
        private Cell[,] Board = new Cell[9, 9];
        private FigureCreationFactory _figureCreationFactory;
        private List<Figure> _figureStructs;
        private const int _indexOfMassive = 1;
        public event Action Loaded;
        public event Action FigurePlacedOnNewPosition;
        
        public ChessPuzzleData _chessPuzzleData;
        #endregion
        
        #region UnityMethods
        
        private void Start()
        {
            _availablePrefabsDictionary = new Dictionary<ChessPuzzleFiguresTypes, GameObject>();
            _figureStructs = new List<Figure>();
            MakeADictionary();
            BoardInitialization();
            SetNullableBoard();
            _figureCreationFactory = new FigureCreationFactory(
                _availablePrefabsDictionary,_parentBoard);
            Loaded.Invoke();
        }

        #endregion

        #region Methods
        
        // делаю велосипед.
        private void MakeADictionary()
        {
            var index = 1;
            foreach (var prefab in _availableFigures)
            {
                _availablePrefabsDictionary.Add((ChessPuzzleFiguresTypes)index,prefab);
                index++;
            }
        }
        
        public void SetNullableBoard()
        {
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                   Board[i,j].SetTypeOfCell(ChessPuzzleFiguresTypes.None);
                }
            }
               
            
        }
        
        private void BoardInitialization()
        {
            var realBoard = gameObject.GetComponentsInChildren<Cell>();
            for (int i = 1; i <= 8; i++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    var indexOfCell = (i - _indexOfMassive) * 8 + (j - _indexOfMassive);
                    Board[i,j] = realBoard[indexOfCell];
                    realBoard[indexOfCell].IndexX = (int)realBoard[indexOfCell].gameObject.transform.position.x;
                    realBoard[indexOfCell].IndexY = (int)realBoard[indexOfCell].gameObject.transform.position.y;
                }
            }
        }

        public void SetPuzzledFigures()
        {
            foreach (var figureStruct in _chessPuzzleData.BoardPosition)
            {
                Board[figureStruct.CurrentPositionX, figureStruct.CurrentPositionY].
                    SetTypeOfCell(figureStruct.IndexOfFigure);
                //_availableFigures[0].GetType();
                var currentFigure = _figureCreationFactory.CreateAFigure(figureStruct.IndexOfFigure,
                     new Vector2(figureStruct.CurrentPositionX, figureStruct.CurrentPositionY));
                currentFigure.OnPosition += newPosAlert;
                _figureStructs.Add(currentFigure);
            }
        }

        private void newPosAlert()
        {
            FigurePlacedOnNewPosition.Invoke();
        }

        #endregion
    }
}