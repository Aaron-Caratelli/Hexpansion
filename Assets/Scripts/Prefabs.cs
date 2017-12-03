using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

namespace Hexpansion
{
    public class Prefabs : MonoBehaviour
    {
        private static Dictionary<Type, MonoBehaviour> _prefabs;

        static Prefabs()
        {
            _prefabs = new Dictionary<Type, MonoBehaviour>();
            var allPrefabNames = Directory.GetFiles("Assets/Resources/Prefabs/", "*.prefab")
                .Select(Path.GetFileNameWithoutExtension).ToList();

            foreach (string name in allPrefabNames)
            {
                var prefabType = Type.GetType("Hexpansion." + name);
                _prefabs[prefabType] = (MonoBehaviour)Resources.Load("Prefabs/" + name, prefabType);
            }
        }

        public static T Get<T>() where T : MonoBehaviour
        {
            var prefabType = typeof(T);
            System.Diagnostics.Debug.Assert(_prefabs.ContainsKey(prefabType));
            return (T)_prefabs[prefabType];
        }

        public static MonoBehaviour Get(Type prefabType)
        {
            System.Diagnostics.Debug.Assert(_prefabs.ContainsKey(prefabType));
            return _prefabs[prefabType];
        }
    }

}
