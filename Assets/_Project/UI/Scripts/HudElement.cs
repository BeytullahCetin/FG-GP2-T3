using FormatableTextNS;
using TMPro;
using UnityEngine;

namespace FG_GP2_T3
{
    public class HudElement : MonoBehaviour
    {
        [SerializeField] FormatableText contentText;
        public FormatableText ContentText => contentText;
    }
}
