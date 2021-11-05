using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Rescues
{
    [Serializable]
    public sealed class WorldGameData
    {
        #region Fields

        private static string _playerPosition;
        private static PlayersProgress _playersProgress;
        private static List<LevelProgress> _levelsProgress;

        public WorldGameData()
        {
            _playerPosition = null;
            _playersProgress = new PlayersProgress();
            _levelsProgress = new List<LevelProgress>();
        }

        #endregion
        
        #region Methods

        public void SavePlayersPosition(Vector3 playersPosition)
        {
            _playerPosition = ConvertVector3ToString(playersPosition);
        }

        public void SavePlayersProgress(int currentLevel)
        {
            _playersProgress.PlayerCurrentPositionInProgress = currentLevel;
        }

        public void AddNewLevelInfoToLevelsProgress(LevelProgress levelProgress)
        {
            _levelsProgress.Add(levelProgress);
        }

        #region Items

        public void AddInLevelProgressItem(int levelsIndex, ItemListData itemListData)
        {
            _levelsProgress[levelsIndex].ItemBehaviours.Add(itemListData);
        }

        public void SaveInfoInLevelProgressItem(int levelsIndex,string name, ItemListData itemListData )
        {
            var index = _levelsProgress[levelsIndex].ItemBehaviours.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].ItemBehaviours.Insert(index,itemListData);
        }
        public void DeleteInfoInLevelProgressItem(int levelsIndex, ItemListData itemListData )
        {
            _levelsProgress[levelsIndex].ItemBehaviours.Remove(itemListData);
        }
        
        
        #endregion

        #region Quests

        public void AddInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _levelsProgress[levelsIndex].QuestListData.Add(itemListData);
        }
        public void SaveInfoInLevelProgressQuest(int levelsIndex,string name, QuestListData itemListData )
        {
            var index = _levelsProgress[levelsIndex].QuestListData.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].QuestListData.Insert(index,itemListData);
        }
        public void DeleteInfoInLevelProgressQuest(int levelsIndex, QuestListData itemListData )
        {
            _levelsProgress[levelsIndex].QuestListData.Remove(itemListData);
        }
        

        #endregion

        #region Puzzles

        public void AddInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _levelsProgress[levelsIndex].PuzzleListData.Add(itemListData);
        }
        public void SaveInfoInLevelProgressPuzzle(int levelsIndex,string name, PuzzleListData itemListData )
        {
            var index = _levelsProgress[levelsIndex].PuzzleListData.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].PuzzleListData.Insert(index,itemListData);
        }
        public void DeleteInfoInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData )
        {
            _levelsProgress[levelsIndex].PuzzleListData.Remove(itemListData);
        }
        
        
        #endregion
        public byte[] Serialize()
        {
            
            IEnumerable<byte> position = Encoding.ASCII.GetBytes(_playerPosition);
            var playerProgress =  ByteConverter.AddToStreamPlayersProgress(_playersProgress);
            var levels = ByteConverter.AddToStreamLevelProgress(_levelsProgress);
            IEnumerable<byte> total = Encoding.ASCII.GetBytes("[").Concat(position).Concat(Encoding.ASCII.GetBytes("]")).
                Concat(Encoding.ASCII.GetBytes("[")).Concat(playerProgress).Concat(Encoding.ASCII.GetBytes("]")).
                Concat(levels);
            
            byte[] result = total.ToArray();
            return result;
        }

        public static WorldGameData Deserialize(byte[] data)
        {
            var dataString =Encoding.ASCII.GetString(data);
            var result = new WorldGameData();
            ByteConverter.DataReader(dataString,out _playerPosition,out _playersProgress,out _levelsProgress);
            return result;
        }

        public string ConvertVector3ToString(Vector3 vector3)
        {
            return vector3.x + "," + vector3.y + "," + vector3.z;
        }

        #endregion
      
    }
}
    
    