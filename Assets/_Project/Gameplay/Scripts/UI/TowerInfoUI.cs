using DG.Tweening;
using FormatableTextNS;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class TowerInfoUI : MonoBehaviour
    {
        [SerializeField] RectTransform panel;
        [SerializeField] RectTransform bottomPanel;
        [SerializeField] RectTransform mainPanel;
        [SerializeField] RectTransform detailPanel;

        [SerializeField] Button expandButton;
        [SerializeField] Button closeButton;

        [SerializeField] FormatableText towerInfoText;
        [SerializeField] FormatableText towerBasedText;

        [SerializeField] Color fusedTowerNameColor = Color.magenta;
        [SerializeField] Color fusedTowerStatColor = Color.green;

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }

        void Start()
        {
            closeButton.onClick.AddListener(Hide);
        }

        public void SetTowerInfo(TowerData towerData)
        {
            towerInfoText.FillText(
                towerData.TowerName, towerData.CrowdControlType.ToString(),
                "Damage", towerData.Damage.ToString(),
                "Range", towerData.Range.ToString(),
                "FireRate", towerData.FireRate.ToString(),
                "Fuse EffectInfo", "Lorem ipsum");

            towerBasedText.FillText(towerData.TowerName + "bla bla based!");
        }

        public void SetTowerFusionInfo(TowerData mainTower, TowerData secondaryTower)
        {
            string damageText = mainTower.Damage.ToString();
            string rangeText = mainTower.Range.ToString();
            string fireRateText = mainTower.FireRate.ToString();
            string fuseRate = secondaryTower.FusionStatValue.ToString();

            string statColorHex = ColorUtility.ToHtmlStringRGB(fusedTowerStatColor);
            string towerNameColorHex = ColorUtility.ToHtmlStringRGB(fusedTowerNameColor);

            switch (secondaryTower.FusionStatType)
            {
                case FusionStatType.DamageBoost:
                    damageText = $"<color=#{statColorHex}>{damageText}+{fuseRate}</color>";
                    break;

                case FusionStatType.RangeBoost:
                    rangeText = $"<color=#{statColorHex}>{rangeText}+{fuseRate}</color>";
                    break;

                case FusionStatType.FireRateBoost:
                    fireRateText = $"<color=#{statColorHex}>{fireRateText}+{fuseRate}</color>";
                    break;

            }

            towerInfoText.FillText(
                $"<color=#{towerNameColorHex}>(Evolved){mainTower.TowerName}</color>",
                mainTower.CrowdControlType.ToString(),
                "Damage", damageText,
                "Range", rangeText,
                "FireRate", fireRateText,
                "Fuse EffectInfo", "Lorem ipsum");


            towerBasedText.FillText(secondaryTower.TowerName + "bla bla based!");
        }

        [Button]
        public async void Show()
        {
            panel.DOAnchorPosY(0, 0.5f);
        }

        [Button]
        public async void Hide()
        {
            panel.DOLocalMoveY(0, 0.5f);
        }
    }
}
