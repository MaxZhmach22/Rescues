using System.Collections.Generic;
using UnityEngine;

namespace Rescues
{
    [CreateAssetMenu(fileName = "ChessPuzzle", menuName = "Data/Puzzles/ChessPuzzle")]
    public class ChessPuzzleData: ScriptableObject
    {
        #region Fields
        //нужна последовательность
        [Header("(Пример записи: 0,1,2 )")]
        [Header("Последовательность активных элементов")]
        [SerializeField]
        public string Sequence;
        // не особо хорошее решение. надо подумать как сделать лучше.
        // нужны ограничения по Х и У
        [Header("Элементы на доске")]
        public List<FigureStruct> BoardPosition;
        
        #endregion
    }
}