using FormatableTextNS;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class TowerRerollButton : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] FormatableText text;

        public Button Button => button;
        public FormatableText Text => text;

        public void UpdateRerollButton(float cost)
        {
            bool isAffordable = CompostManager.Instance.CurrentCompostAmount >= cost;
            string costString = cost.ToString();

            if (isAffordable == false)
                costString = $"<color=red>{costString}</color>";

            text.FillText(costString);
            button.interactable = isAffordable;
        }
    }
}
