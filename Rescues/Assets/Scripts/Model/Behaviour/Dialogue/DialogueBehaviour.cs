using UnityEngine;


namespace Rescues
{
    [RequireComponent(typeof(VIDE_Assign))]
    public class DialogueBehaviour: InteractableObjectBehavior
    {
        #region Fields

        public VIDE_Assign assignDialog;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            assignDialog = GetComponent<VIDE_Assign>();
        } 

        #endregion
    }
}
