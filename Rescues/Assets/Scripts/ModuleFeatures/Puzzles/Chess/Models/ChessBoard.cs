using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    public class ChessBoard: MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameObject[] _availableFigures;
        private Dictionary<ChessPuzzleFiguresTypes, GameObject> _availablePrefabsDictionary;
        private Cell[,] Board = new Cell[8, 8];
        private FigureCreationFactory _figureCreationFactory;
        public event Action Loaded;
        
        public ChessPuzzleData _chessPuzzleData;
        #endregion
        
        #region UnityMethods
        
        public void Start()
        {
            _availablePrefabsDictionary = new Dictionary<ChessPuzzleFiguresTypes, GameObject>();
            MakeADictionary();
            BoardInitialization();
            SetNullableBoard();
            _figureCreationFactory = new FigureCreationFactory(
                _availablePrefabsDictionary);
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
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                   Board[i,j].SetTypeOfCell(ChessPuzzleFiguresTypes.None);
                }
            }
               
            
        }
        
        private void BoardInitialization()
        {
            var realBoard = gameObject.GetComponentsInChildren<Cell>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Board[i,j] = realBoard[i*8+j];
                }
            }
        }

        public void SetPuzzledFigures()
        {
            foreach (var figureStruct in _chessPuzzleData.BoardPosition)
            {
                Board[figureStruct.CurrentPositionX, figureStruct.CurrentPositionY].
                    SetTypeOfCell(figureStruct.IndexOfFigure);
                _availableFigures[0].GetType();
                _figureCreationFactory.CreateAFigure(figureStruct.IndexOfFigure,
                     new Vector2(figureStruct.CurrentPositionX, figureStruct.CurrentPositionY));
            }
        }
        
        #endregion
    }
}