using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace Rescues
{
    public class DialogueUI : MonoBehaviour
    {
        #region Fields

        [Header("Npc")]
        public GameObject npcContainer;
        public TextMeshProUGUI npcLabel;
        public TextMeshProUGUI npcText;
        public Color npcLabelColor;
        public Image npcImage;
        public Color npcImageNormalColor;
        [Range(1, 10)]
        public int writeStep;
        [Range(1, 10)]
        public int writeSpeed;
        public Image npcBackGround;

        [Space]
        [Header("Player"), Space(10)]
        public GameObject playerContainer;
        public TextMeshProUGUI playerLabel;
        public PossibleAnswer[] playerTextChoices;
        public Color playerLabelColor;
        public Image playerImage;
        public Color playerImageNormalColor;
        public Image playerBackground;

        [Space]
        [Header("Other"), Space(10)]
        public GameObject dialogContainer;
        public Image background;
        public NodeSoundContainer nodeSoundContainer;

        #endregion
    }
}
