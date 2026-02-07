using FormatableTextNS;
using UnityEngine;
using UnityEngine.UI;

namespace FG_GP2_T3
{
    public class SpeedUpButton : ScaleUpPanel
    {
        [SerializeField] Button button;
        [SerializeField] FormatableText currentSpeedText;

        void OnEnable()
        {
            GameSpeedController.OnGameSpeedSet += UpdateSpeedText;
        }

        void OnDisable()
        {
            GameSpeedController.OnGameSpeedSet -= UpdateSpeedText;
        }

        void Start()
        {
            button.onClick.AddListener(CycleGameSpeeds);
        }

        void CycleGameSpeeds()
        {
            GameManager.Instance.GameSpeedController.CycleGameSpeeds();
        }

        void UpdateSpeedText(float speed)
        {
            currentSpeedText.FillText(speed.ToString());
        }
    }
}
