using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using YG;

namespace LastTrain.Localization
{
    public class LocalizationBrige : MonoBehaviour
    {
        private const string NameLanguageRu = "Russian";
        private const string NameLanguageEn = "English";
        private const string NameLanguageTr = "Turkish";
        private const string CodelanguageRu = "ru";
        private const string CodelanguageEn = "en";
        private const string CodelanguageTr = "tr";

        private void Start()
        {
            LocalizationManager.Read();
            YG2.onSwitchLang += OnLanguageChanged;
            YG2.onCorrectLang += OnLanguageChanged;
            InitialLanguage();
        }

        private void OnDestroy()
        {
            YG2.onSwitchLang -= OnLanguageChanged;
            YG2.onCorrectLang -= OnLanguageChanged;
        }

        private void InitialLanguage()
        {
            string initialLanguageCode = !string.IsNullOrEmpty(YG2.lang) ? YG2.lang : CodelanguageEn;
            OnLanguageChanged(initialLanguageCode);
        }

        private void OnLanguageChanged(string languageCode)
        {
            string normalizedLanguage = NormalizeLanguageCode(languageCode);

            if (LocalizationManager.Dictionary.ContainsKey(normalizedLanguage))
            {
                LocalizationManager.Language = normalizedLanguage;
            }
            else
            {
                LocalizationManager.Language = NameLanguageEn;
            }
        }

        private string NormalizeLanguageCode(string languageCode)
        {
            return languageCode switch
            {
                CodelanguageRu => NameLanguageRu,
                CodelanguageEn => NameLanguageEn,
                CodelanguageTr => NameLanguageTr,
                _ => NameLanguageEn
            };
        }
    }
}
