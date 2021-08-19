using System;
using UnityEngine;

namespace Rescues
{
    [Serializable]
    public class FigureStruct
    {
        [Range(-1,32)]
        public int UnicSequenceID = -1;
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