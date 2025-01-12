using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using TMPro;

public class LanguageSwitcher : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        dropdown.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }
        dropdown.AddOptions(options);
        dropdown.value = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
        dropdown.onValueChanged.AddListener(LocaleSelected);
    }

    public static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
