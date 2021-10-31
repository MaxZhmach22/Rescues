using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Rescues
{
    public sealed class GameSavingSerializer
    {
        private string path =Path.GetFullPath(
            Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\"));
        public IEnumerable<FileContext> GetAllSaves()
        {
            var path = $"{this.path}/{Serialization.SAVING_PATH}";
            if (Directory.Exists(path) == false)
            {
                yield break;
            }       
            foreach (var file in Directory.GetFiles(path))
            {
                if(Path.HasExtension(file) == false || Path.GetExtension(file) != Serialization.DEFEND_EXTENSION)
                    continue;
                yield return new FileContext(new FileInfo(file));
            }
        }
        
        public void Delete(string fileName)
        {
            
            var savePath = $"{path}/{Serialization.SAVING_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
            if(File.Exists(savePath))
                File.Delete(savePath);
        }
        
        public void Save(WorldGameData data, string fileName)
        {
            var formatter = new BinaryFormatter();
            Directory.CreateDirectory(path+"/"+$"{Serialization.SAVING_PATH}");
            var savePath = $"{path}/{Serialization.SAVING_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
            var savePath2 = $"{path}/{Serialization.SAVING_PATH}/{fileName}.ttt";
            File.WriteAllBytes(savePath, data.Serialize());
            using (FileStream stream = File.Create(savePath2))
                formatter.Serialize(stream, data);
            //UnityEditor.EditorUtility.RevealInFinder(savePath);
        }
        
        public WorldGameData Load(string fileName)
        {
            var savePath = $"{path}/{Serialization.SAVING_PATH}/{fileName}{Serialization.DEFEND_EXTENSION}";
            if (File.Exists(savePath) == false)
                return null;
            return WorldGameData.Deserialize(File.ReadAllBytes(savePath));
        }
        
    }
}