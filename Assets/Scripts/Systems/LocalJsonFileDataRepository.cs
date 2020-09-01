using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using UnityEngine;

namespace Roomba.Systems
{
    public class LocalJsonFileDataRepository : IDataRepository
    {
        private readonly GlobalSettings _settings;
        private string _path;

        private Dictionary<string, object> _database;
        private bool _saving;

        private Queue<Action> _saveQueue;
        private bool _dirty;
        private string _dir;

        public LocalJsonFileDataRepository(GlobalSettings settings)
        {
            _saveQueue = new Queue<Action>();

            _settings = settings;

#if UNITY_EDITOR
            _dir = Application.dataPath + "/../" + _settings.dataFolder;
#else
            _dir = Application.persistentDataPath + "/" +  _settings.dataFolder;
#endif
            _path = _dir + "/configurations.json";
        }

        public void Save(string key, object value)
        {
            if (_database == null)
            {
                LoadDb();
            }

            if (_database.ContainsKey(key))
            {
                if (_database[key] == value)
                    return;

                _database[key] = value;
                
                _dirty = true;
                SaveDb();
                return;
            }

            _database.Add(key, value);
            _dirty = true;
            SaveDb();
        }

        private void LoadDb()
        {
            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
            }
            
            if (!File.Exists(_path))
            {
                _database = new Dictionary<string, object>();
                SaveDb();
                return;
            }

            var json = File.ReadAllText(_path);
            _database = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        }

        private void SaveDb(Action callback = null)
        {
            if (_database == null)
            {
                callback?.Invoke();
                return;
            }

            if (_saving)
            {
                _saveQueue.Enqueue(() => SaveDb(callback));
                return;
            }

            if (!_dirty)
                callback?.Invoke();
            
            _saving = true;
            Debug.Log("Saving " +_path);
            
            
            var trd = new Thread(() =>
            {
                _dirty = false;
                var json = JsonConvert.SerializeObject(_database);

                File.WriteAllText(_path, json);

                callback?.Invoke();
                _saving = false;

                if (_saveQueue.Any())
                {
                    _saveQueue.Dequeue()();
                }
            });

            trd.Start();
        }

        public T Load<T>(string key)
        {
            if (!_database.ContainsKey(key))
            {
                return default;
            }

            return (T) _database[key];
        }
    }
}