using System.IO;

namespace Rescues
{
    public static class Serialization
    {
        #region Fields

        public const string SAVING_PATH = "SavingPath";
        public const string DEFEND_EXTENSION = ".def";
        public static string path =Path.GetFullPath(
            Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\"));

        #endregion
    }
}