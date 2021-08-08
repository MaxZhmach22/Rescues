using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    [CreateAssetMenu(fileName = "ChessPuzzle", menuName = "Data/Puzzles/ChessPuzzle")]
    public class ChessPuzzleData: ScriptableObject
    {
        #region Fields
        //нужна последовательность
        
        // не особо хорошее решение. надо подумать как сделать лучше.
        // нужны ограничения по Х и У
        public List<FigureStruct> BoardPosition;
        
        #endregion
    }
}