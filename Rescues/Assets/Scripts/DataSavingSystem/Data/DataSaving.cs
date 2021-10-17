using System;

namespace Rescues
{
    public sealed class DataSaving
    {
        public string Name { get; private set; }
        public string Time { get; private set; }
        
        public Action<string> Selected { get; private set; }
        //public Action<> Deleted {get; private set;} 
        
    }
}