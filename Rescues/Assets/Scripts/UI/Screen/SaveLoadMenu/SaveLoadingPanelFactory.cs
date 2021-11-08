using UnityEngine;

namespace Rescues
{
    public sealed class SaveLoadingPanelFactory
    {
        #region Methods

        public GameObject Create(string whatUICreating, Transform parent)
        {
            return (GameObject) Object.Instantiate(Resources.Load("Prefabs/UI/Saving/"+whatUICreating),parent);
        }

        #endregion
    }
}