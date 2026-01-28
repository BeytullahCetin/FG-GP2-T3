using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class SelectionButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMP_Text title;
        [SerializeField] Image image;

        public Button Button => button;
        public TMP_Text Title => title;
        public Image Image => image;
    }
}
