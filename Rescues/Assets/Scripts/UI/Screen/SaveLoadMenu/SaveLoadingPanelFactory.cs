using UnityEngine;

namespace Rescues
{
    public sealed class SaveLoadingPanelFactory
    {
        public GameObject Create(string whatUICreating, Transform parent)
        {
            return (GameObject) Object.Instantiate(Resources.Load("Prefabs/UI/"+whatUICreating),parent);
        }
    }
}