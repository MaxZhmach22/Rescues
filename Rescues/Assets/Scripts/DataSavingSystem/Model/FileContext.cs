using System.IO;

namespace Controllers.DataSavingSystem.Model
{
    public sealed class FileContext
    {
        public string CreationTime { get; private set; }
        public string FileName { get; private set; }
        private const string _dateFormat = "yyyy/MM/dd HH:mm:ss";

        public FileContext(FileInfo fileInfo)
        {
            CreationTime = fileInfo.CreationTimeUtc.ToString(_dateFormat);
            FileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
        }
    }
}