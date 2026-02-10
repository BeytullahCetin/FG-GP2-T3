using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] List<QualitySettingsButton> qualityLevelButtonList;
        [SerializeField] List<FrameRateButton> frameRateButtonList;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] Transform modalTransform;
        [SerializeField] Button closeButton;

        [Header("Animation")]
        [SerializeField] float animationDuration = .5f;
        [SerializeField] Ease showEase = Ease.OutBack;
        [SerializeField] Ease hideEase = Ease.InBack;

        [Button]
        public void Show()
        {
            GameManager.Instance.PauseGame();
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            modalTransform.DOScale(1, animationDuration).SetUpdate(true);
            canvasGroup.DOFade(1, animationDuration).SetUpdate(true);
        }

        [Button]
        public void Hide()
        {
            GameManager.Instance.ResumeGame();
            modalTransform.DOScale(0, animationDuration).SetEase(hideEase);
            canvasGroup.DOFade(0, animationDuration);
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        void Start()
        {
            closeButton.onClick.AddListener(Hide);

            Application.targetFrameRate = PlayerPrefs.GetInt(SettingsManager.PP_FrameRate, 60);
            SetFrameRateButtons();
            UpdateFrameRateButtons();

            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(SettingsManager.PP_QualityIndex, 0), true);
            SetQualityButtons();
            UpdateQualityButtons();

            Hide();
        }

        void SetQualityButtons()
        {
            for (int i = 0; i < qualityLevelButtonList.Count; i++)
            {
                QualitySettingsButton qualitySettingsButton = qualityLevelButtonList[i];
                qualitySettingsButton.qualityIndex = i;
                qualitySettingsButton.qualityName = SettingsManager.Instance.QualitySettingsController.QualitySettings[i];
                qualitySettingsButton.QualitySettingsText.SetText(qualitySettingsButton.qualityName);

                qualitySettingsButton.Button.onClick.AddListener(() =>
                {
                    QualitySettings.SetQualityLevel(qualitySettingsButton.qualityIndex, true);
                    PlayerPrefs.SetInt(SettingsManager.PP_QualityIndex, qualitySettingsButton.qualityIndex);
                    UpdateQualityButtons();
                });
            }
        }

        void UpdateQualityButtons()
        {
            foreach (QualitySettingsButton qualitySettingsButton in qualityLevelButtonList)
            {
                qualitySettingsButton.Button.interactable = qualitySettingsButton.qualityIndex != QualitySettings.GetQualityLevel();
            }
        }

        void SetFrameRateButtons()
        {
            for (int i = 0; i < frameRateButtonList.Count; i++)
            {
                FrameRateButton frameRateButton = frameRateButtonList[i];
                frameRateButton.FrameRateText.SetText(SettingsManager.Instance.QualitySettingsController.SupportedFrameRates[i].ToString());
                frameRateButton.frameRate = SettingsManager.Instance.QualitySettingsController.SupportedFrameRates[i];

                frameRateButton.Button.onClick.AddListener(() =>
                {
                    Application.targetFrameRate = frameRateButton.frameRate;
                    PlayerPrefs.SetInt(SettingsManager.PP_FrameRate, frameRateButton.frameRate);
                    UpdateFrameRateButtons();
                });
            }
        }

        void UpdateFrameRateButtons()
        {
            foreach (FrameRateButton frameRateButton in frameRateButtonList)
            {
                frameRateButton.Button.interactable = frameRateButton.frameRate != Application.targetFrameRate;
            }
        }
    }
}
