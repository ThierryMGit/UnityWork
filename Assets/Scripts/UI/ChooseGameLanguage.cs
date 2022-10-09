using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ChooseGameLanguage : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var dropDown = transform.GetComponent<TMPro.TMP_Dropdown>();

        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        var options = new List<TMPro.TMP_Dropdown.OptionData>();
        int selected = 0;
        for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            var locale = LocalizationSettings.AvailableLocales.Locales[i];
            if (LocalizationSettings.SelectedLocale == locale)
                selected = i;
            options.Add(new TMPro.TMP_Dropdown.OptionData(locale.name));
        }
        dropDown.options = options;

        dropDown.value = selected;
        dropDown.onValueChanged.AddListener(LocaleSelected);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void LocaleSelected(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
