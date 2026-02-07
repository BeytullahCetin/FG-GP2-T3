using DG.Tweening;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace FormatableTextNS
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FormatableText : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmp_text;
        public TMP_Text TMP_Text => tmp_text;

        [SerializeField] string replacePrefix = "#";
        [ResizableTextArea][SerializeField] string initialText;

        public void FillTextWith(string textToFill, params string[] parameters)
        {
            initialText = textToFill;
            FillText(parameters);
        }

        public void FillText(params string[] parameters)
        {
            string newText = ObtainNewText(parameters);
            if (newText != null)
                TMP_Text.SetText(newText);
        }

        public Tween DoFillText(float duration = 1f, Ease ease = Ease.Linear, params string[] parameters)
        {
            string newText = ObtainNewText(parameters);
            Tween textTween = null;
            if (newText != null)
            {
                TMP_Text.SetText("");
                textTween = TMP_Text.DOText(newText, duration, true)
                    .SetEase(ease);
            }

            return textTween;
        }

        public void SetText(string text)
        {
            TMP_Text.SetText(text);
        }

        private string ObtainNewText(params string[] parameters)
        {
            string newText = initialText;

            if (string.IsNullOrEmpty(replacePrefix) || parameters == null)
            {
                return null;
            }

            for (int i = parameters.Length; i > 0; i--)
            {
                newText = newText.Replace($"{replacePrefix}{i}", parameters[i - 1]);
            }

            return newText;
        }

        [Button]
        public void ResetToInitialText()
        {
            TMP_Text.SetText(initialText);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (TMP_Text != null)
            {
                TMP_Text.SetText(initialText);
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

        private void Reset()
        {
            tmp_text = GetComponent<TMP_Text>();
            UnityEditor.EditorUtility.SetDirty(this);
            TMP_Text.SetText(initialText);
        }
#endif
    }
}