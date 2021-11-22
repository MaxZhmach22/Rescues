using System;


namespace Rescues
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAudioInterfacesAttribute : Attribute
    {
        #region Fields

        public readonly string AssetName;

        #endregion


        #region ClassLifeCycles

        public InjectAudioInterfacesAttribute(string assetName = null) =>
            AssetName = assetName; 

        #endregion
    }
}