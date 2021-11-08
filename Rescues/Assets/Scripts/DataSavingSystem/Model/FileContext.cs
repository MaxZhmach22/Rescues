using System.IO;

namespace Rescues
{
    public sealed class FileContext
    {
        #region Fields

        private string CreationTime { get; set; }
        public string FileName { get; private set; }
        private const string _dateFormat = "yyyy/MM/dd HH:mm:ss";

        #endregion

        
        #region ClassLifeCycles

        public FileContext(FileInfo fileInfo)
                {
                    CreationTime = fileInfo.CreationTimeUtc.ToString(_dateFormat);
                    FileName = Path.GetFileNameWithoutExtension(fileInfo.Name);
                }

        #endregion
        
    }
}