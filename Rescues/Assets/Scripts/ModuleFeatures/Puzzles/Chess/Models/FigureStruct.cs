using System;

namespace Rescues
{
    [Serializable]
    public class FigureStruct
    {
        public ChessPuzzleFiguresTypes IndexOfFigure;
        public int CurrentPositionX;
        public int CurrentPositionY;
        public int EndPositionX;
        public int EndPositionY;
    }
}