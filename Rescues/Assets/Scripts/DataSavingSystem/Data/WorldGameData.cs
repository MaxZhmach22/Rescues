using System;
using UnityEngine;


namespace Controllers.DataSavingSystem.Model
{
    [Serializable]
    public sealed class WorldGameData
    {
        private string _playerPosition;
        //public PlayersInventory PlayersInventory;
        public PlayersProgress PlayersProgress;
        public byte[] Serialize()
        {
            var result = new byte[sizeof(int) + sizeof(int) + sizeof(byte) * 2 +
                                  sizeof(byte) * Content.Length];
            
            var offset = 0;
            offset += ByteConverter.AddToStream(Version, result, offset);
            offset += ByteConverter.AddToStream(AccountId, result, offset);
            offset += ByteConverter.AddToStream(X, result, offset);
            offset += ByteConverter.AddToStream(Y, result, offset);
            foreach (var c in Content)
            {
                offset += ByteConverter.AddToStream((byte) c, result, offset);
            }

            return result;
        }

        public static WorldGameData Deserialize(byte[] data)
        {
            var offset = 0;

            offset += ByteConverter.ReturnFromStream(data, offset, out int version);
            if (version != Serialization.VERSION)
            {
                return null;
            }

            var result = new WorldGameData();

            offset += ByteConverter.ReturnFromStream(data, offset, out result.AccountId);
            offset += ByteConverter.ReturnFromStream(data, offset, out result.X);
            offset += ByteConverter.ReturnFromStream(data, offset, out result.Y);
            offset += ByteConverter.ReturnFromStream(data, offset, data.Length - offset, out byte[] content);

            result.Content = new GameTileContentType[content.Length];
            for (var i = 0; i < content.Length; i++)
            {
                result.Content[i] = (GameTileContentType) content[i];
            }

            return result;
        }

        public void ConvertVector3ToString(Vector3 vector3)
        {
            _playerPosition = vector3.x + "," + vector3.y + "," + vector3.z;
        }
    }
}
    
    