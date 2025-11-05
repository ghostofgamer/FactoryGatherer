using System.Collections.Generic;
using System.IO;
using FactoryContent;
using SOContent;
using UnityEngine;

namespace SaveContent
{
    public class SaveSystem : MonoBehaviour
    {
        public static GameData Data = new GameData();

        private const string PlayerPrefsKey = "GameData";

        public static void SaveFactory(Factory factory)
        {
            var existing = Data.factories.Find(f => f.ID == factory.ID);

            if (existing != null)
            {
                existing.ID = factory.ID;
                existing.Position = factory.transform.position;
                existing.StoredAmount = factory.GetStoredAmount();
                existing.ResourceType = factory.Data.Produces.ResourceName;
            }
            else
            {
                Data.factories.Add(new FactoryData
                {
                    ID = factory.ID,
                    Position = factory.transform.position,
                    ResourceType = factory.Data.Produces.ResourceName,
                    StoredAmount = factory.GetStoredAmount()
                });
            }
            
            SaveToPlayerPrefs();
        }

        public static FactoryData GetFactory(int id)
        {
            return Data.factories.Find(f => f.ID == id);
        }
        
        public static void SaveResources(Dictionary<ResourceData, int> resources)
        {
            Data.resources.Clear();
            
            foreach (var kv in resources)
                Data.resources.Add(new ResourceSaveData(kv.Key.ResourceName, kv.Value));
            
            SaveToPlayerPrefs();
        }

        public static void SaveToPlayerPrefs()
        {
            string json = JsonUtility.ToJson(Data, true);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
            Debug.Log("[GameSave] GameData сохранён!");
        }

        public static void LoadFromPlayerPrefs()
        {
            if (!PlayerPrefs.HasKey(PlayerPrefsKey))
                return;

            string json = PlayerPrefs.GetString(PlayerPrefsKey);
            Data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("[GameSave] GameData загружен!");
        }


        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "save.json");

        public static void Save(GameData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
#if UNITY_EDITOR
            Debug.Log($"[SaveSystem] Game saved to: {SavePath}");
#endif
        }

        public static GameData Load()
        {
            if (!File.Exists(SavePath))
            {
#if UNITY_EDITOR
                Debug.LogWarning("[SaveSystem] Save file not found. Creating new GameData.");
#endif
                return new GameData();
            }

            string json = File.ReadAllText(SavePath);
            GameData data = JsonUtility.FromJson<GameData>(json);
            return data;
        }

        public static void DeleteSave()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
#if UNITY_EDITOR
            Debug.Log("[SaveSystem] Save file deleted.");
#endif
        }
    }
}