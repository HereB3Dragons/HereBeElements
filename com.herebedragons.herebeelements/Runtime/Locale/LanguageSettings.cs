using System;
using System.Collections.Generic;

namespace HereBeElements.Locale
{
        [Serializable]
        public sealed class LanguageSettings
        {
            public string defaultLocale = "en_US";
            public AvailableLocale[] availableLocales = new AvailableLocale[1];
            public Dictionary<string, string> languageCache = new Dictionary<string, string>();
        }
}