using System;
using System.Collections;
using UnityEngine;

static class Helper
{
    public static string GetUntilOrEmpty(this string text, string stopAt = "-")
    {
        if (!string.IsNullOrWhiteSpace(text))
        {
            int charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

            if (charLocation > 0)
            {
                return text.Substring(0, charLocation);
            }
        }

        return string.Empty;
    }

    private static IEnumerator ShowText(TMPro.TextMeshProUGUI _textMeshProText, string text, float fadeInDuration = 0.2f)
    {
        var step = 0f;
        var initialColor = new Color(_textMeshProText.color.r, _textMeshProText.color.g, _textMeshProText.color.b, 0);
        var finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1);

        _textMeshProText.color = initialColor;
        _textMeshProText.text = text;

        while (step < 1f)
        {
            step += Time.deltaTime / fadeInDuration;

            _textMeshProText.color = Color.Lerp(initialColor, finalColor, step);

            yield return 0;
        }
    }

    private static IEnumerator EraseText(TMPro.TextMeshProUGUI _textMeshProText, float fadeOutDuration = 0.2f)
    {
        var step = 0f;
        var initialColor = new Color(_textMeshProText.color.r, _textMeshProText.color.g, _textMeshProText.color.b, 1);
        var finalColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0);

        _textMeshProText.color = initialColor;

        while (step < 1f)
        {
            step += Time.deltaTime / fadeOutDuration;

            _textMeshProText.color = Color.Lerp(initialColor, finalColor, step);

            yield return 0;
        }
    }

    public static IEnumerator DisplayText(this TMPro.TextMeshProUGUI _textMeshProText, string text, float fadeInDuration, float duration, float fadeOutDuration)
    {
        yield return ShowText(_textMeshProText, text, fadeInDuration);
        yield return new WaitForSeconds(duration);
        yield return EraseText(_textMeshProText, fadeOutDuration);
    }
}