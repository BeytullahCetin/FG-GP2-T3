using FormatableTextNS;
using TMPro;
using UnityEngine;

namespace FG_GP2_T3
{
    public class CompostManager : MonoBehaviour
    {
        public static CompostManager Instance;

        [Header("Compost Settings")]
        public int StartingCompostAmount = 10;

        public int CurrentCompostAmount { get; private set; }

        [Header("UI Reference")]
        [SerializeField] private FormatableText _CompostText;


        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        void Start()
        {
            CurrentCompostAmount = StartingCompostAmount;
            UpdateCompostUI();
        }

        public void AddCompost(int amount)
        {
            CurrentCompostAmount += amount;
            UpdateCompostUI();
        }

        public bool UseCompost(int amount)
        {
            if (CurrentCompostAmount >= amount)
            {
                CurrentCompostAmount -= amount;
                UpdateCompostUI();
                return true;
            }
            Debug.LogWarning("Not enough compost to use!");
            return false;
        }
        public void UpdateCompostUI()
        {
            if (_CompostText != null)
            {
                _CompostText.FillText(CurrentCompostAmount.ToString());
            }
        }
    }
}
