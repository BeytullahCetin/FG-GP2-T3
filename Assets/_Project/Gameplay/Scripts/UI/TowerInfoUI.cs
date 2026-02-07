using DG.Tweening;
using FormatableTextNS;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class TowerInfoUI : MonoBehaviour
    {
        [SerializeField] VerticalLayoutGroup layoutGroup;
        [SerializeField] RectTransform mainLayoutElement;
        [SerializeField] RectTransform detailLayoutElement;
        [SerializeField] RectTransform detailPanelTransform;

        [SerializeField] Button expandButton;
        [SerializeField] Button shrinkButton;
        [SerializeField] Button closeButton;

        [SerializeField] FormatableText towerInfoText;
        [SerializeField] FormatableText towerDetailText;

        [SerializeField] Color towerNameColor = Color.red;
        [SerializeField] Color fusedTowerNameColor = Color.magenta;
        [SerializeField] Color fusedTowerStatColor = Color.green;

        [SerializeField] float showHideDuration = .5f;
        [SerializeField] float expandShrinkDuration = .5f;
        [SerializeField] Ease showEase = Ease.OutBack;
        [SerializeField] Ease hideEase = Ease.InBack;
        [SerializeField] Ease expandEase = Ease.OutBack;
        [SerializeField] Ease shrinkEase = Ease.InBack;

        private RectTransform layoutGroupTranform;
        private RectTransform closeButtonTransform;

        float LayoutHidePos => mainLayoutElement.sizeDelta.x + closeButtonTransform.sizeDelta.y + layoutGroup.spacing;
        float DetailShrinkPos => -detailLayoutElement.sizeDelta.y + closeButtonTransform.sizeDelta.y;

        void OnEnable()
        {
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState += Hide;
        }

        void OnDisable()
        {
            GameplayStateFlowEvents.OnEnteredTowerSelectionSubGameplayState -= Hide;
        }

        void Awake()
        {
            layoutGroupTranform = layoutGroup.GetComponent<RectTransform>();
            closeButtonTransform = closeButton.GetComponent<RectTransform>();
        }

        void Start()
        {
            closeButton.onClick.AddListener(Hide);
            expandButton.onClick.AddListener(Expand);
            shrinkButton.onClick.AddListener(Shrink);
        }

        public void SetTowerInfo(TowerData towerData)
        {
            string towerName = $"<color={towerNameColor.ToHex()}>{towerData.TowerName}</color>";
            string towerRole = $"<color={towerNameColor.ToHex()}>{towerData.Role}</color>";

            towerInfoText.FillText(towerName, towerRole,
                $"Damage: {towerData.DamageString}", $"Fire Rate: {towerData.FireRateString}",
                $"Range: {towerData.RangeString}", $"Attack Type: {towerData.AttackType}",
                $"Fuse Effect: {towerData.FusionStatType}",
                towerData.Description);
            FillTowerDetail(towerData);
        }

        public void SetTowerFusionInfo(TowerData mainTower, TowerData secondaryTower)
        {
            string towerName = $"<color={fusedTowerNameColor.ToHex()}>Evolved {mainTower.TowerName}</color>";
            string towerRole = $"<color={fusedTowerNameColor.ToHex()}>{mainTower.Role}</color>";

            string damageText = mainTower.DamageString;
            string rangeText = mainTower.RangeString;
            string fireRateText = mainTower.FireRateString;

            // string fuseRate = secondaryTower.FusionStatValue.ToString();
            switch (secondaryTower.FusionStatType)
            {
                case FusionStatType.DamageBoost:
                    damageText = $"<color={fusedTowerStatColor.ToHex()}>{damageText}+</color>";
                    break;

                case FusionStatType.RangeBoost:
                    rangeText = $"<color={fusedTowerStatColor.ToHex()}>{rangeText}+</color>";
                    break;

                case FusionStatType.FireRateBoost:
                    fireRateText = $"<color={fusedTowerStatColor.ToHex()}>{fireRateText}+</color>";
                    break;
            }

            towerInfoText.FillText(towerName, towerRole,
                $"Damage: {damageText}", $"FireRate: {fireRateText}",
                $"Range: {rangeText}", $"Attack Type: {mainTower.AttackType}",
                $"<color={fusedTowerStatColor.ToHex()}>{secondaryTower.FusionStatType}+</color>",
                mainTower.Description);
            FillTowerDetail(mainTower);
        }

        private void FillTowerDetail(TowerData towerData)
        {
            towerDetailText.FillText(towerData.ReferanceDescription, towerData.RealLifeEquivalentDescription);
        }

        [Button]
        public void Show()
        {
            layoutGroupTranform.DOAnchorPosY(0, showHideDuration).SetEase(showEase);
        }

        [Button]
        public void Hide()
        {
            Shrink();
            layoutGroupTranform.DOAnchorPosY(-LayoutHidePos, showHideDuration).SetEase(hideEase);
        }

        [Button]
        public void Expand()
        {
            shrinkButton.gameObject.SetActive(true);
            expandButton.gameObject.SetActive(false);
            detailPanelTransform.DOAnchorPosY(0, expandShrinkDuration).SetEase(expandEase);
        }

        [Button]
        public void Shrink()
        {
            expandButton.gameObject.SetActive(true);
            shrinkButton.gameObject.SetActive(false);
            detailPanelTransform.DOAnchorPosY(DetailShrinkPos, expandShrinkDuration).SetEase(shrinkEase);
        }
    }
}
