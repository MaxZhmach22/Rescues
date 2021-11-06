using UnityEngine;


namespace Rescues
{
    [RequireComponent(typeof(ItemData))]
    public sealed class ItemBehaviour: InteractableObjectBehavior
    {
        #region Fields
        
        public ItemData ItemData;
        public float PickUpTime = 0.5f;

        #endregion
    }
}
