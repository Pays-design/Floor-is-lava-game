using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Assets.Resources.Scripts
{
    public class GlobalSaverLoader : MonoBehaviour
    {
        private readonly string s_nameOfFile = "data.bin";
        private Dictionary<string, object> m_dataDictionary = new Dictionary<string, object>();

        public event Action OnSave;
        public event Action OnLoad;

        private static GlobalSaverLoader s_instance;

        public static GlobalSaverLoader GetInstance() 
        {
            if (s_instance == null) 
            {
                GameObject g = new GameObject();
                s_instance = g.AddComponent<GlobalSaverLoader>();
            }
            return s_instance;
        }

        private void SaveDataInABinaryFile()
        {
            m_dataDictionary = new Dictionary<string, object>();
            OnSave?.Invoke();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(GetDataFilePath(), FileMode.OpenOrCreate, FileAccess.Write))
            {
                try
                {
                    binaryFormatter.Serialize(fileStream, m_dataDictionary);
                }
                catch (System.Runtime.Serialization.SerializationException exception)
                {
                    Debug.LogError(exception.Message);
                    throw;
                }
                finally
                {
                    fileStream.Close();
                }
            }
        }

        private void GetDataFromABinaryFile() 
        {
            m_dataDictionary = new Dictionary<string, object>();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(GetDataFilePath(), FileMode.Open, FileAccess.Read))
            {
                try
                {
                    m_dataDictionary = (Dictionary<string, object>) binaryFormatter.Deserialize(fileStream);
                }
                catch (System.Runtime.Serialization.SerializationException exception)
                {
                    Debug.LogError(exception.Message);
                    throw;
                }
                finally
                {
                    fileStream.Close();
                }
                OnLoad?.Invoke();
            }
        }

        private string GetDataFilePath()
        {
            return Application.persistentDataPath + $"/{s_nameOfFile}";
        }

        public TData TryGetData<TData>() where TData : class 
        {
            string nameOfType = typeof(TData).Name;
            if (m_dataDictionary.ContainsKey(nameOfType))
                return (TData) m_dataDictionary[nameOfType];
            return null;
        }

        public void AddDataToSerialize<TData>(TData data) => m_dataDictionary.Add(typeof(TData).Name, data);

        #region MonoBehaviour
        private void OnApplicationQuit() => SaveDataInABinaryFile();

        private void Start()
        {
            FindObjectOfType<ReloadLevelButton>().OnReloadLevel += SaveDataInABinaryFile;
            if (File.Exists(GetDataFilePath()))
                GetDataFromABinaryFile();
        }
        #endregion

    }

}
