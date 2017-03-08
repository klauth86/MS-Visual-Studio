using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleCacheStore
{
    public enum STORAGE : byte { MEMORY, DISK };

    public class MyCache:IDisposable
    {
        public delegate void ItemRemoved(string key, object value);
        public event ItemRemoved onRemove = delegate { };

        protected struct ObjectData
        {
            internal object obj;
            internal Type type;
            internal STORAGE storage;
            internal DateTime isAddedAt;
            internal DateTime isActualTill;
        };

        private Dictionary<string, ObjectData> _data;
        private object _locker;

        MyCache()
        {
            _data = new Dictionary<string, ObjectData>();
            _locker = new object();
        }

        public static MyCache _instance;
        public static MyCache Instance=>_instance ?? (_instance = new MyCache());

        public KeyValuePair<Type, object> Get(string key)
        {
            lock (_locker)
            {
                if (_data.ContainsKey(key) == false)                
                    throw new KeyNotFoundException($"An object with key '{key}' does not exist");
                
                if (_data[key].isActualTill <= DateTime.Now)                
                    throw new ArgumentException($"An object with key '{key}' is invalid due to its Date Of Actuality");
                
                KeyValuePair<Type, object> retval;
                if (_data[key].storage == STORAGE.MEMORY)                
                    retval = new KeyValuePair<Type, object>(_data[key].type, _data[key].obj);
                else
                {
                    IFormatter formatter = new BinaryFormatter();
                    using (Stream stream = new FileStream(key + ".bin", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        retval = new KeyValuePair<Type, object>(_data[key].type, formatter.Deserialize(stream));
                    }
                }
                return retval;
            }
        }

        public void Add(string key, object value, DateTime? isActualTill = null)
        {
            lock (_locker)
            {
                if (_data.ContainsKey(key))                
                    throw new ArgumentException($"An object with key '{key}' already exists");                

                if (value == null)                
                    throw new ArgumentException($"An object with key '{key}' contains NULL value");                

                ObjectData objdata;
                objdata.obj = value;
                objdata.type = value.GetType();
                objdata.storage = STORAGE.MEMORY;
                objdata.isAddedAt = DateTime.Now;
                objdata.isActualTill = isActualTill ?? DateTime.MaxValue;
                _data.Add(key, objdata);
            }
        }

        public void Add(string key, object value, STORAGE storage, DateTime? isActualTill = null)
        {
            if (storage == STORAGE.MEMORY)
                Add(key, value, isActualTill);
            else
            {
                lock (_locker)
                {
                    if (_data.ContainsKey(key)) 
                        throw new ArgumentException($"An object with key '{key}' already exists");                    

                    if (value == null) 
                        throw new ArgumentException($"An object with key '{key}' contains NULL value");                    

                    if (!value.GetType().IsSerializable)                    
                        throw new ArgumentException($"An object with key '{key}' is not Serializable");                    

                    ObjectData objdata;
                    objdata.obj = null;
                    objdata.type = value.GetType();
                    objdata.storage = STORAGE.DISK;
                    objdata.isAddedAt = DateTime.Now;
                    objdata.isActualTill = isActualTill ?? DateTime.MaxValue;

                    IFormatter formatter = new BinaryFormatter();
                    using (Stream stream = new FileStream(key + ".bin", FileMode.Create, FileAccess.Write, FileShare.None))
                        formatter.Serialize(stream, value);
                    _data.Add(key, objdata);
                }
            }
        }

        public void Remove(string key)
        {
            lock (_locker)
            {
                if (_data.ContainsKey(key) == false)
                    throw new ApplicationException(String.Format("An object with key '{0}' does not exist", key));
                lock (_locker)
                {
                    if (_data[key].storage == STORAGE.DISK)
                    {
                        File.Delete(key + ".bin");
                    }
                    _data.Remove(key);
                }
            }
        }

        public void Dispose() {
            foreach (var el in _data) {
                if (el.Value.storage == STORAGE.DISK) {
                    File.Delete(el.Key + ".bin");
                }
            }
        }
    }
}
