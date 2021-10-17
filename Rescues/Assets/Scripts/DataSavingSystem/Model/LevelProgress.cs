using System.Collections.Generic;

namespace Rescues
{
    public sealed class LevelProgress
    {
        //public List<NPCData> NPCsPositions; тут надо подумать как лучше сохранять данные об npc
        public List<ItemData> ItemBehaviours;
        public List<QuestListData> QuestListData;
        public List<PuzzleListData> PuzzleListData;
    }
}