using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    const string localeKey = "locale";

    void Start()
    {
        LoadLocale();
        dropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }
        dropdown.AddOptions(options);
        dropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(
            LocalizationSettings.SelectedLocale);
        dropdown.onValueChanged.AddListener(HandleLocaleSelected);
    }

    public static void HandleLocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        PlayerPrefs.SetString(localeKey, LocalizationSettings.SelectedLocale.Identifier.Code);
    }

    void LoadLocale()
    {
        // Load saved locale.
        if (PlayerPrefs.HasKey(localeKey))
        {
            string localeCode = PlayerPrefs.GetString(localeKey);
            Locale locale = LocalizationSettings.AvailableLocales.GetLocale(localeCode);
            if (locale != null)
            {
                LocalizationSettings.SelectedLocale = locale;
                return;
            }
        }
        // Fallback to system locale.
        Locale systemLocale = LocalizationSettings.AvailableLocales.GetLocale(Application.systemLanguage);
        if (systemLocale != null)
        {
            LocalizationSettings.SelectedLocale = systemLocale;
        }
    }
}
