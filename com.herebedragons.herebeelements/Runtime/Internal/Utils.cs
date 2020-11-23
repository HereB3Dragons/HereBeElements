using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Internal
{
    public static class Utils
    {
        internal const string EMPTY = "";
        
        private static Regex JSON_KEY_CLEANER = new Regex("\\n\\s*");
        
        internal static IEnumerator LoadContent<T>(AssetReference assetRef, Action<T> setter, Action<float> percentageSetter = null)
        {
            AsyncOperationHandle<T> loadProcess = assetRef.LoadAssetAsync<T>();
            if (percentageSetter != null)
                percentageSetter(loadProcess.PercentComplete);
            yield return loadProcess;
                switch (loadProcess.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        setter(loadProcess.Result);
                        break;
                    default:
                        Debug.Log(loadProcess.OperationException);
                        break;
                }
            
        }
        
        internal static void LoadAsset<T>(AssetReference assetRef, Action<T> setter)
        {
            AsyncOperationHandle<T> loadProcess = assetRef.LoadAssetAsync<T>();

            loadProcess.Completed += h =>
            {
                switch (h.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                        setter(h.Result);
                        break;
                    default:
                        Debug.LogError(h.OperationException);
                        break;
                }
          
            };
        }
        
        internal static string ToJson<T>(T data)
        {
            MemoryStream stream = new MemoryStream();

            DataContractJsonSerializer serialiser = new DataContractJsonSerializer(
                data.GetType(),
                new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true
                });

            serialiser.WriteObject(stream, data);

            return Encoding.UTF8.GetString(stream.ToArray());
        }
        
        internal static Dictionary<string, Dictionary<string, string>> FromJson(string data)
        {
            Dictionary<string, object> temp = JsonHelper.ParseJSON(data);

            return temp.ToDictionary(t => JSON_KEY_CLEANER.Replace(t.Key, EMPTY),
                t =>
            {
                Dictionary<string, string> temp2 = new Dictionary<string, string>(); 
                Dictionary<string, object> inner = (Dictionary<string, object>)t.Value;
                foreach (string key in inner.Keys)
                    temp2.Add(JSON_KEY_CLEANER.Replace(key, EMPTY), inner[key] as string);
                return temp2;

            });            
        }

    }
}