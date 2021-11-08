using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;


namespace Rescues
{
    [Serializable]
    public sealed class WorldGameData
    {
        #region Fields

        private static string _playerPosition;
        private static PlayersProgress _playersProgress;
        private static List<LevelProgress> _levelsProgress;
        public Action RestartLevel = delegate { Debug.Log("Invoke restart"); };
        
        #endregion

        
        #region ClassLifeCycles

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

        private void AddNewLevelInfoToLevelsProgress(LevelProgress levelProgress)
        {
            _levelsProgress.Add(levelProgress);
        }

        #region Items

        public void AddInLevelProgressItem(int levelsIndex, ItemListData itemListData)
        {
            _levelsProgress[levelsIndex].ItemBehaviours.Add(itemListData);
        }

        public void SaveInfoInLevelProgressItem(int levelsIndex, string name, ItemListData itemListData)
        {
            var index = _levelsProgress[levelsIndex].ItemBehaviours.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].ItemBehaviours.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressItem(int levelsIndex, ItemListData itemListData)
        {
            _levelsProgress[levelsIndex].ItemBehaviours.Remove(itemListData);
        }


        #endregion

        
        #region Quests

        public void AddInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _levelsProgress[levelsIndex].QuestListData.Add(itemListData);
        }

        public void SaveInfoInLevelProgressQuest(int levelsIndex, string name, QuestListData itemListData)
        {
            var index = _levelsProgress[levelsIndex].QuestListData.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].QuestListData.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressQuest(int levelsIndex, QuestListData itemListData)
        {
            _levelsProgress[levelsIndex].QuestListData.Remove(itemListData);
        }


        #endregion

        
        #region Puzzles

        public void AddInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _levelsProgress[levelsIndex].PuzzleListData.Add(itemListData);
        }

        public void SaveInfoInLevelProgressPuzzle(int levelsIndex, string name, PuzzleListData itemListData)
        {
            var index = _levelsProgress[levelsIndex].PuzzleListData.FindIndex(s => s.Name == name);
            _levelsProgress[levelsIndex].PuzzleListData.Insert(index, itemListData);
        }

        public void DeleteInfoInLevelProgressPuzzle(int levelsIndex, PuzzleListData itemListData)
        {
            _levelsProgress[levelsIndex].PuzzleListData.Remove(itemListData);
        }
        
        #endregion

        public byte[] Serialize()
        {

            IEnumerable<byte> position = Encoding.ASCII.GetBytes(_playerPosition);
            var playerProgress = ByteConverter.AddToStreamPlayersProgress(_playersProgress);
            var levels = ByteConverter.AddToStreamLevelProgress(_levelsProgress);
            IEnumerable<byte> total = Encoding.ASCII.GetBytes("[").Concat(position).Concat(Encoding.ASCII.GetBytes("]"))
                .Concat(Encoding.ASCII.GetBytes("[")).Concat(playerProgress).Concat(Encoding.ASCII.GetBytes("]"))
                .Concat(levels);

            byte[] result = total.ToArray();
            return result;
        }

        public static void Deserialize(byte[] data)
        {
            var dataString = Encoding.ASCII.GetString(data);
            ByteConverter.DataReader(dataString, out _playerPosition, out _playersProgress, out _levelsProgress);
        }

        public bool LookForLevelByNameBool(string name)
        {
            foreach (var levelProgress in _levelsProgress)
                if (levelProgress.LevelsName == name)
                    return true;
            return false;
        }

        public int LookForLevelByNameInt(string name)
        {
            int counter = 0;
            foreach (var levelProgress in _levelsProgress)
            {
                if (levelProgress.LevelsName == name)
                {
                    return counter;
                }

                counter++;
            }

            return -1;
        }

        public void AddNewLocation(Location location,IGate gate)
        {
            AddNewLevelInfoToLevelsProgress(new LevelProgress()
            {
                LevelsName = location.name,
                LastGate = gate
            });
            int correctIndex = _levelsProgress.Count - 1;
            foreach (Transform transform in location._items.transform)
                AddInLevelProgressItem(correctIndex, new ItemListData()
                {
                    Name = transform.name,
                    //TODO: Need implementation
                    ItemCondition = (ItemCondition) 1
                });
            foreach (Transform transform in location._puzzles.transform)
                AddInLevelProgressPuzzle(correctIndex, new PuzzleListData()
                {
                    Name = transform.name,
                    //TODO: Need implementation
                    PuzzleCondition = (PuzzleCondition) 1
                });
        }

        public void OpenCurrentLocation(Location location)
        {
            var locationIndex = LookForLevelByNameInt(location.name);
            foreach (var item in _levelsProgress[locationIndex].ItemBehaviours)
            {
                if ((item.ItemCondition == (ItemCondition)0)||(item.ItemCondition==(ItemCondition)2))
                    foreach (Transform realItem in location._items)
                        if (item.Name==realItem.gameObject.name)
                            Object.Destroy(realItem.gameObject);
            }
            foreach (var puzzle in _levelsProgress[locationIndex].PuzzleListData)
            {
                if ((puzzle.PuzzleCondition == (PuzzleCondition)1))
                    foreach (Transform realPuzzle in location._puzzles)
                        if (puzzle.Name == realPuzzle.gameObject.name)
                            Object.Destroy(realPuzzle.gameObject);
            }
        }

        public IGate GetLastGate()
        {
            return _levelsProgress[_playersProgress.PlayerCurrentPositionInProgress].LastGate;
        }

        public string ConvertVector3ToString(Vector3 vector3)
        {
            return vector3.x + "," + vector3.y + "," + vector3.z;
        }

        #endregion
    }
}
    
    