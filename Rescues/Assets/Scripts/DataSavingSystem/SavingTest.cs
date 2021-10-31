using UnityEngine;

namespace Rescues
{
    public class SavingTest : MonoBehaviour
    {
        public SavingTest()
        {
            GameSavingSerializer GameSavingSerializer = new GameSavingSerializer();
           // WorldGameData worldGameData = new WorldGameData();
            
            // worldGameData.SavePlayersPosition(new Vector3(0,1,0));
            // worldGameData.SavePlayersProgress(1);
            // worldGameData.AddNewLevelInfoToLevelsProgress(new LevelProgress(){LevelsName = "Hotel"});
            // worldGameData.AddInLevelProgressItem(0,new ItemListData(){Name = "Pivo",ItemCondition = (ItemCondition)1});
            // worldGameData.AddInLevelProgressPuzzle(0,new PuzzleListData(){Name = "Chess", PuzzleCondition = (PuzzleCondition)1});
            // worldGameData.AddInLevelProgressPuzzle(0,new PuzzleListData(){Name = "Chess", PuzzleCondition = (PuzzleCondition)1});
            // worldGameData.AddInLevelProgressPuzzle(0,new PuzzleListData(){Name = "Chess", PuzzleCondition = (PuzzleCondition)1});
            // worldGameData.AddInLevelProgressQuest(0,new QuestListData(){Name = "Don't smoking weeds", QuestCondition = (QuestCondition)0});
            // worldGameData.AddNewLevelInfoToLevelsProgress(new LevelProgress(){LevelsName = "Reseption"});
            // worldGameData.AddInLevelProgressItem(1,new ItemListData(){Name = "Pivos",ItemCondition = (ItemCondition)1});
            // worldGameData.AddInLevelProgressPuzzle(1,new PuzzleListData(){Name = "ChessSS", PuzzleCondition = (PuzzleCondition)1});
            // worldGameData.AddInLevelProgressQuest(1,new QuestListData(){Name = "Don't smoking weedsSS", QuestCondition = (QuestCondition)0});
            //
            // GameSavingSerializer.Save(worldGameData,"Test");
            //
             var newWorldGameData = GameSavingSerializer.Load("Test");
            // Debug.Log("End of Save-Load");
        }
    }
}