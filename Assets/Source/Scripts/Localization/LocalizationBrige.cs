using Assets.SimpleLocalization.Scripts;
using UnityEngine;
using YG;

namespace LastTrain.Localization
{
    public class LocalizationBrige : MonoBehaviour
    {
        private const string _nameLanguageRu = "Russian";
        private const string _nameLanguageEn = "English";
        private const string _nameLanguageTr = "Turkish";
        private const string _codelanguageRu = "ru";
        private const string _codelanguageEn = "en";
        private const string _codelanguageTr = "tr";

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
            string initialLanguageCode = !string.IsNullOrEmpty(YG2.lang) ? YG2.lang : _codelanguageEn;
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
                LocalizationManager.Language = _nameLanguageEn;
            }
        }

        private string NormalizeLanguageCode(string languageCode)
        {
            return languageCode switch
            {
                _codelanguageRu => _nameLanguageRu,
                _codelanguageEn => _nameLanguageEn,
                _codelanguageTr => _nameLanguageTr,
                _ => _nameLanguageEn
            };
        }
    }
}
