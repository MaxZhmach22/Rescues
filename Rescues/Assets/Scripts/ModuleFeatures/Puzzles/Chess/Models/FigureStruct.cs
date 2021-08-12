using System;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public class FigureStruct
    {
        public ChessPuzzleFiguresTypes IndexOfFigure;
        [Range(1, 8)]
        public int CurrentPositionX;
        [Range(1, 8)]
        public int CurrentPositionY;
        [Range(1, 8)]
        public int EndPositionX;
        [Range(1, 8)]
        public int EndPositionY;
    }
}