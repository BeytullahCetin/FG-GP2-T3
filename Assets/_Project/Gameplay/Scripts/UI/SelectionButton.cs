using FormatableTextNS;
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
        [SerializeField] GameObject selected;

        public Button Button => button;
        public TMP_Text Title => title;
        public Image Image => image;
        public GameObject Selected => selected;

        public void Select()
        {
            selected.SetActive(true);
        }

        public void Deselect()
        {
            selected.SetActive(false);
        }
    }
}
