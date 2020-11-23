using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using HereBeElements.Internal;
using UnityEngine;


namespace HereBeElements.Locale
{
    [Serializable]
    public class Locale<T> where T : Enum
    {
        private static AvailableLocale DefaultLocale;
        private const string UNDEFINED = "UNDEFINED";
        private static CultureInfo _localeInfo;
        private static Dictionary<string, string> _messages;

        private static HashSet<AvailableLocale> _langs;
        
        private static readonly Regex LOCALE_TO_CULTURE = new Regex("([a-z]{2})_([A-Z]{2})");

        private static Locale<T> _instance;

        public delegate void ChangeLocaleHandler();
        public static ChangeLocaleHandler ChangeLanguageEvent;
        
        public static void RegisterLanguages(AvailableLocale[] languages)
        {
            if (languages == null)
                return;
            foreach (AvailableLocale availableLanguage in languages)
                RegisterLocale(availableLanguage);
        }

        public static Locale<T> Instance()
        {
            return _instance;
        }
        
        protected Locale()
        {
        }

        public static Locale<T> Init(string lang, Dictionary<string, string> cache = null)
        {
            AvailableLocale l= StringToLocale(lang); 
            if (_instance is null)
                _instance = new Locale<T>(l);

            if (cache != null)
                _messages = cache;
            else if (_messages is null)
                _messages = new Dictionary<string, string>();
            
            SetLocale(l);
            return _instance;
        }

        public static void RegisterLocale(AvailableLocale n)
        {
            if (_langs == null)
                _langs = new HashSet<AvailableLocale>();
            _langs.Add(n);
        }

        private static AvailableLocale StringToLocale(string lang)
        {
            if (_langs == null)
                throw new ArgumentException("no langs registered yet");
            if(_langs.Any(l => l.lang.Equals(lang)))
                return _langs.First(l => l.lang.Equals(lang));
            throw new ArgumentException("could not find lang " + lang + ". Did you register it?");
        }
        
        private Locale(AvailableLocale defaultLocale)
        {
            DefaultLocale = defaultLocale;
            if (_instance != null)
                throw new Exception("there can be only one");
            _instance = this;
        }

        public static string GetText(T key, params string[] replacements)
        {
            if (replacements.Length != 0)
                return String.Format(GetOrUndefined(key.ToString()), replacements);
            return GetOrUndefined(key.ToString());
        }

        public static string FormatNumber(long number)
        {
            if (_localeInfo != null)
                return number.ToString(_localeInfo);
            return number.ToString();
        }
        
        private static void SetLocale(AvailableLocale localeConst)
        {
            string locale = GetCultureString(localeConst);
            _localeInfo = new CultureInfo(locale);
            
            Utils.LoadAsset<TextAsset>(localeConst.jsonFile, SetLocale);
        }
        
        private static void SetLocale(TextAsset localeTextAsset)
        {
            if (localeTextAsset is null) return;

            Dictionary<string, Dictionary<string, string>> dictFromJson = Utils.FromJson(localeTextAsset.text);
            if (dictFromJson == null || dictFromJson.Count == 0) return;
            
            _messages.Clear();
            foreach (var keyValuePair in dictFromJson)
                foreach (var valuePair in keyValuePair.Value)
                    _messages.Add(keyValuePair.Key + "_" + valuePair.Key, valuePair.Value);
            
            if (ChangeLanguageEvent != null)
                ChangeLanguageEvent.Invoke();
        }

        private static string GetCultureString(AvailableLocale locale)
        {
            return LOCALE_TO_CULTURE.Replace(locale.lang, "$1-$2");
        }


        private static string GetOrUndefined(string key)
        {
            if (_messages is null || !_messages.ContainsKey(key))
                return UNDEFINED;
            return _messages[key];
        }
    }
}