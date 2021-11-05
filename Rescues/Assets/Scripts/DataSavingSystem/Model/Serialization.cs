using System.IO;

namespace Rescues
{
    public class Serialization
    {
        public const int VERSION = 1;
        public const string SAVING_PATH = "SavingPath";
        public const string DEFEND_EXTENSION = ".def";
        public static string path =Path.GetFullPath(
            Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), @"..\..\"));
        public enum ErrorCode
        {
            FileNotFound,
            VersionInvalid,
            Unknown
        }
    }
}