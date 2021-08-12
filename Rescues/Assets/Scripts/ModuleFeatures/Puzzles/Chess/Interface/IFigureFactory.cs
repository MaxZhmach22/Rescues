using Rescues;
using UnityEngine;

namespace ModuleFeatures.Puzzles.Chess.Interface
{
    public interface IFigureFactory
    {
        Figure CreateAFigure(ChessPuzzleFiguresTypes figure,Vector2 pos);
    }
}