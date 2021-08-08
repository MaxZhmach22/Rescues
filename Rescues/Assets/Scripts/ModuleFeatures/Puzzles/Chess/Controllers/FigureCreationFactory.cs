using System.Collections.Generic;
using ModuleFeatures.Puzzles.Chess.Interface;
using UnityEngine;

namespace Rescues
{
    public class FigureCreationFactory: IFigureFactory
    {
        #region Fields

        private Dictionary<ChessPuzzleFiguresTypes, GameObject> _availableGameObjectsDictionary;

        #endregion
        
        public FigureCreationFactory(Dictionary<ChessPuzzleFiguresTypes, GameObject>availableGameObjects)
        {
            _availableGameObjectsDictionary = availableGameObjects;
        }
        
        
        public void CreateAFigure(ChessPuzzleFiguresTypes figure,Vector2 pos)
        {
            Object.Instantiate(_availableGameObjectsDictionary[figure], pos,new Quaternion());
        }
    }
}