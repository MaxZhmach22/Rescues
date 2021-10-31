using System.Collections.Generic;

namespace Rescues
{
    public sealed class LevelProgress
    {
        //public List<NPCData> NPCsPositions; тут надо подумать как лучше сохранять данные об npc
        public string LevelsName;
        public List<ItemListData> ItemBehaviours = new List<ItemListData>();
        public List<QuestListData> QuestListData = new List<QuestListData>();
        public List<PuzzleListData> PuzzleListData = new List<PuzzleListData>();
    }
}