using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rescues
{
    public static class ByteConverter
    {
        public static IEnumerable<byte> AddToIntStream(int source)
        {
            var newString = Convert.ToString(source);
            IEnumerable<byte> destination = Encoding.ASCII.GetBytes(newString);
            return destination;
        }

        public static IEnumerable<byte> AddToStreamPlayersProgress(PlayersProgress playersProgress)
        {
            IEnumerable<byte> playersProgressBytes = AddToIntStream(playersProgress.PlayerCurrentPositionInProgress);
            playersProgressBytes = playersProgressBytes.Concat(Encoding.ASCII.GetBytes("/"));
            return playersProgressBytes;
        }

        public static IEnumerable<byte> AddToStreamLevelProgress(List<LevelProgress> levelProgresses)
        {
            // Сначала хранятся количество данных
            // Затем уже вещественные
            IEnumerable<byte> AllLevelsBytes = Encoding.ASCII.GetBytes("");
            foreach (var levelProgress in levelProgresses)
            {
                IEnumerable<byte> LevelBytes = Encoding.ASCII.GetBytes(levelProgress.LevelsName);
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes("|"));
                foreach (var item in levelProgress.ItemBehaviours)
                {
                    ConvertInputs(item.Name, (int) item.ItemCondition, LevelBytes, out LevelBytes);
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"\"));
                }
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"|"));
                foreach (var puzzle in levelProgress.PuzzleListData)
                {
                    ConvertInputs(puzzle.Name, (int) puzzle.PuzzleCondition, LevelBytes, out LevelBytes);
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"\"));
                }
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"|"));
                foreach (var quest in levelProgress.QuestListData)
                {
                    ConvertInputs(quest.Name, (int) quest.QuestCondition, LevelBytes, out LevelBytes);
                    LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"\"));
                }
                LevelBytes = LevelBytes.Concat(Encoding.ASCII.GetBytes(@"#"));
                AllLevelsBytes = AllLevelsBytes.Concat(LevelBytes);
            }
            return AllLevelsBytes;
        }
        private static void ConvertInputs(string name, int condition,IEnumerable<byte> mass,out IEnumerable<byte> LevelBytes)
        {
            var Name = Encoding.ASCII.GetBytes(name);
            var Condition = AddToIntStream(condition);
            LevelBytes = mass.Concat(Name).Concat(Encoding.ASCII.GetBytes("-")).Concat(Condition);
        }
        public static int ReturnFromStream(byte[] source, int offset, out byte destination)
        {
            destination = source[offset];
            return 1;
        }

        public static int ReturnFromStream(byte[] source, int offset, out int destination)
        {
            destination = 0;
            destination |= source[offset] << 24;
            destination |= source[offset + 1] << 16;
            destination |= source[offset + 2] << 8;
            destination |= source[offset + 3];
            return 4;
        }

        public static List<LevelProgress> ReturnFromStreamLevelProgress()
        {
            List<LevelProgress> levelProgresses = new List<LevelProgress>();
            return levelProgresses;
        }
        //
        // private static string _playerPosition;
        // private static PlayersProgress _playersProgress;
        // private static List<LevelProgress> _levelsProgress;
        public static void DataReader(string dataString,
            out string playerPosition,
            out PlayersProgress playersProgress,
            out List<LevelProgress> levelsProgress)
        {
            string[] data = dataString.Split('/');
            playerPosition = data[0];
            playersProgress = new PlayersProgress(){PlayerCurrentPositionInProgress = Convert.ToInt32(data[1])};
            levelsProgress = new List<LevelProgress>();
            string[] levels = data[2].Split('#');
            levels = levels.Reverse().Skip(1).Reverse().ToArray();//hmm...expencive......
            int counter = 0;
            foreach (var level in levels)
            {
                levelsProgress.Add(new LevelProgress());
                var levelInside = level.Split('|');
                levelsProgress[counter].LevelsName = levelInside[0];
                levelInside = levelInside.Skip(1).ToArray();
                levelsProgress[counter].ItemBehaviours = new List<ItemListData>();
                levelsProgress[counter].PuzzleListData = new List<PuzzleListData>();
                levelsProgress[counter].QuestListData = new List<QuestListData>();
                var items = levelInside[0].Split('\\');
                items = items.Reverse().Skip(1).Reverse().ToArray();
                foreach (var part in items) 
                {
                    
                    ConvertInputs(out var name,out var condition,part);
                    levelsProgress[counter].ItemBehaviours.Add(new ItemListData() 
                        {Name = name, ItemCondition = (ItemCondition)condition});
                }
                var puzzles = levelInside[1].Split('\\');
                puzzles = puzzles.Reverse().Skip(1).Reverse().ToArray();
                foreach (var part in puzzles) 
                {
                    ConvertInputs(out var name,out var condition,part);
                    levelsProgress[counter].PuzzleListData.Add(new PuzzleListData() 
                        {Name = name, PuzzleCondition = (PuzzleCondition)condition});
                }
                var quests = levelInside[2].Split('\\');
                quests = quests.Reverse().Skip(1).Reverse().ToArray();
                foreach (var part in quests) 
                {
                    ConvertInputs(out var name,out var condition,part);
                    levelsProgress[counter].QuestListData.Add(new QuestListData() 
                        {Name = name, QuestCondition = (QuestCondition)condition});
                }
                counter++;
            }
        }
        private static void ConvertInputs(out string name,out int condition, string part)
        {
            var splitedPart = part.Split('-');
            name = splitedPart[0];
            condition = Convert.ToInt32(splitedPart[1]);
        }
    }
}