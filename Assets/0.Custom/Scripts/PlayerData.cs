using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace _0.Custom.Scripts
{
    public static class PlayerData
    {
        public static Action onChangeCoin;

        public static float MusicVolume
        {
            get => PlayerPrefs.GetFloat("MusicVolume", 1f);
            set => PlayerPrefs.SetFloat("MusicVolume", value);
        }

        public static float SfxVolume
        {
            get => PlayerPrefs.GetFloat("SfxVolume", 1f);
            set => PlayerPrefs.SetFloat("SfxVolume", value);
        }
        public static int PlayerCar
        {
            get => PlayerPrefs.GetInt("PlayerCar", 0);
            set => PlayerPrefs.SetInt("PlayerCar", value);
        }
        public static int currentLevel
        {
            get => PlayerPrefs.GetInt("currentLevel", 0);
            set => PlayerPrefs.SetInt("currentLevel", value);
        }

        public static List<int> ListCar
        {
            get => GetList<int>("listPlayerCar", new List<int>{0} );
            set => SetList<int>("listPlayerCar", value );
        }
        public static int currentGold
        {
            get => PlayerPrefs.GetInt("currentGold", 0);
            set
            {
                
                PlayerPrefs.SetInt("currentGold", value);
                onChangeCoin?.Invoke();
            }
        }

        public static bool ShouldShowDailyReward()
        {
            int today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int lastDate = PlayerPrefs.GetInt("LastRewardDate", 0);
            Debug.Log(lastDate != today);
            return lastDate != today;
        } 
        public static void ConfirmToday()
        {
            int today = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            PlayerPrefs.SetInt("LastRewardDate", today);
        } 
        private static void SetBool(string name, bool value)
        {
            PlayerPrefs.SetInt(name, value ? 1 : 0);
            PlayerPrefs.SetInt($"{name}", value ? 1 : 0);
        }

        private static bool GetBool(string name, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(name, defaultValue ? 1 : 0) == 1;
        }

        // ---- INT ----
        private static void SetInt(string name, int value)
        {
            PlayerPrefs.SetInt(name, value);
        }

        private static int GetInt(string name, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(name, defaultValue);
        }
        
        private static void SetList<T>(string key, IList<T> list)
        {
            var json = JsonConvert.SerializeObject(list ?? new List<T>());
            PlayerPrefs.SetString(key, json);
        }

        private static List<T> GetList<T>(string key, List<T> defaultValue = null)
        {
            if (!PlayerPrefs.HasKey(key))
            {
                var def = defaultValue ?? new List<T>();
                PlayerPrefs.SetString(key, JsonConvert.SerializeObject(def));
                return def;
            }

            var json = PlayerPrefs.GetString(key);
            try
            {
                return JsonConvert.DeserializeObject<List<T>>(json) ?? (defaultValue ?? new List<T>());
            }
            catch
            {
                return defaultValue ?? new List<T>();
            }
        }
        
    }
}