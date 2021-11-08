using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rescues
{
    public static class ByteConverter
    {
        #region Methods
        
        private static IEnumerable<byte> AddToIntStream(int source)
        {
            var newString = Convert.ToString(source);
            IEnumerable<byte> destination = Encoding.ASCII.GetBytes(newString);
            return destination;
        }

        public static IEnumerable<byte> AddToStreamPlayersProgress(PlayersProgress playersProgress)
        {
            IEnumerable<byte> playersProgressBytes = AddToIntStream(playersProgress.PlayerCurrentPositionInProgress);
            return playersProgressBytes;
        }

        public static IEnumerable<byte> AddToStreamLevelProgress(List<LevelProgress> levelProgresses)
        {
            int[] counts = new int[3];
            int levelCounter = 0;
            IEnumerable<byte> AllLevelsBytes = Encoding.ASCII.GetBytes("");
            foreach (var levelProgress in levelProgresses)
            {
                counts[0] = 0;
                counts[1] = 0;
                counts[2] = 0;
                IEnumerable<byte> LevelBytes = Encoding.ASCII.GetBytes("[").Concat(Encoding.ASCII.GetBytes("[")).
                    Concat(Encoding.ASCII.GetBytes(levelProgress.LevelsName)).Concat(Encoding.ASCII.GetBytes("]"));
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("[")).
                    Concat(Encoding.ASCII.GetBytes(levelProgress.LastGate.GoToLevelName+",")).
                    Concat(Encoding.ASCII.GetBytes(levelProgress.LastGate.GoToLocationName+",")).
                    Concat(Encoding.ASCII.GetBytes(levelProgress.LastGate.GoToGateId+"")).
                    Concat(Encoding.ASCII.GetBytes("]"));
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("["));
                foreach (var item in levelProgress.ItemBehaviours)
                {
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("["));
                    ConvertInputs(item.Name, (int) item.ItemCondition, LevelBytes, out LevelBytes);
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"]"));
                    counts[0]++;
                }
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("]"));
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"["));
                foreach (var puzzle in levelProgress.PuzzleListData)
                {
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("["));
                    ConvertInputs(puzzle.Name, (int) puzzle.PuzzleCondition, LevelBytes, out LevelBytes);
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"]"));
                    counts[1]++;
                }
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("]"));
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"["));
                foreach (var quest in levelProgress.QuestListData)
                {
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("["));
                    ConvertInputs(quest.Name, (int) quest.QuestCondition, LevelBytes, out LevelBytes);
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"]"));
                    counts[2]++;
                }
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("]"));
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"]"));
                AllLevelsBytes = AllLevelsBytes.
                    Concat(Encoding.ASCII.GetBytes("[")).
                    Concat(Encoding.ASCII.GetBytes($"{counts[0]},{counts[1]},{counts[2]}").
                    Concat(Encoding.ASCII.GetBytes("]")).
                    Concat(LevelBytes));
                levelCounter++;
            }
            return Encoding.ASCII.GetBytes("["). 
                Concat(Encoding.ASCII.GetBytes(levelCounter.ToString())).
                Concat(Encoding.ASCII.GetBytes("]")).
                Concat(AllLevelsBytes);
        }
        private static void ConvertInputs(string name, int condition,IEnumerable<byte> mass,out IEnumerable<byte> LevelBytes)
        {
            var Name = Encoding.ASCII.GetBytes(name);
            var Condition = AddToIntStream(condition);
            LevelBytes = mass.Concat(Name).Concat(Encoding.ASCII.GetBytes("~")).Concat(Condition);
        }
        public static void DataReader(string dataString,
            out string playerPosition,
            out PlayersProgress playersProgress,
            out List<LevelProgress> levelsProgress)
        {
            string[] separatingStrings = { "[","]" };
            string[] data = dataString.Split(separatingStrings,StringSplitOptions.RemoveEmptyEntries);
            playerPosition = data[0];
            playersProgress = new PlayersProgress(){PlayerCurrentPositionInProgress = Convert.ToInt32(data[1])};
            levelsProgress = new List<LevelProgress>();
            int countOfLevelsInfo = Convert.ToInt32(data[2]);
            int offsetIndex = 3;
            int elemCounter = 0;
            for (int i = 0; i < countOfLevelsInfo; i++)
            {
                char[] separatorChars = {','};
                string[] levelCounters = data[elemCounter+offsetIndex].Split(separatorChars, StringSplitOptions.RemoveEmptyEntries);
                elemCounter++;
                levelsProgress.Add(new LevelProgress());
                levelsProgress[i].LevelsName = data[elemCounter+offsetIndex];
                elemCounter++;
                string[] levelLastGate = data[elemCounter+offsetIndex].Split(separatorChars, StringSplitOptions.RemoveEmptyEntries);
                levelsProgress[i].LastGate = 
                    GateDataMock.GetMock(levelLastGate[0], levelLastGate[1], Convert.ToInt32(levelLastGate[2]));
                elemCounter++;
                levelsProgress[i].ItemBehaviours = new List<ItemListData>();
                levelsProgress[i].PuzzleListData = new List<PuzzleListData>();
                levelsProgress[i].QuestListData = new List<QuestListData>();
                for (int j = 0; j < Convert.ToInt32(levelCounters[0]); j++)
                {
                    ConvertInputs(out var name,out var condition,data[elemCounter+offsetIndex]);
                    levelsProgress[i].ItemBehaviours.Add(new ItemListData() 
                        {Name = name, ItemCondition = (ItemCondition)condition});
                    elemCounter++;
                }
                for (int j = 0; j < Convert.ToInt32(levelCounters[1]); j++)
                {
                    ConvertInputs(out var name,out var condition,data[elemCounter+offsetIndex]);
                    levelsProgress[i].PuzzleListData.Add(new PuzzleListData() 
                        {Name = name, PuzzleCondition = (PuzzleCondition)condition});
                    elemCounter++;
                }
                for (int j = 0; j < Convert.ToInt32(levelCounters[2]); j++)
                {
                    ConvertInputs(out var name,out var condition,data[elemCounter+offsetIndex]);
                    levelsProgress[i].QuestListData.Add(new QuestListData() 
                        {Name = name, QuestCondition = (QuestCondition)condition});
                    elemCounter++; 
                }
            }
        }
        private static void ConvertInputs(out string name,out int condition, string part)
        {
            var splitPart = part.Split('~');
            name = splitPart[0];
            condition = Convert.ToInt32(splitPart[1]);
        }

        #endregion
    }
}