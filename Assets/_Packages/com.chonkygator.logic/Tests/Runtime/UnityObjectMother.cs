using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GatOR.Logic.Tests
{
    public class UnityObjectMother
    {
        internal static readonly HashSet<Object> instantiatedObjects = new HashSet<Object>();


        public static void DestroyInstantiated()
        {
            foreach (Object obj in instantiatedObjects)
                Object.DestroyImmediate(obj);
            instantiatedObjects.Clear();
        }


        public static GameObject AGameObject() => AGameObject("TEST");

        public static GameObject AGameObject(string withName = "TEST", bool active = true,
            params Type[] withComponents)
        {
            var go = new GameObject(withName, withComponents);
            go.SetActive(active);
            instantiatedObjects.Add(go);
            return go;
        }

        public static GameObject AGameObjectWithParent(Transform withParent, string withName = "TEST",
            bool active = true, params Type[] withComponents)
        {
            var go = AGameObject(withName, active, withComponents);
            go.transform.SetParent(withParent);
            return go;
        }

        public static GameObject AGameObjectWithParent(GameObject parent, string withName = "TEST",
            bool active = true, params Type[] withComponents)
        {
            return AGameObjectWithParent(parent.transform, withName, active, withComponents);
        }


        public static T AComponent<T>(GameObject onGameObject = null) where T : Component
        {
            return onGameObject.Or(AGameObject).AddComponent<T>();
        }

        public static T AScriptableObject<T>(string withName = "TEST") where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance<T>();
            instance.name = withName;
            instantiatedObjects.Add(instance);
            return instance;
        }
    }
}