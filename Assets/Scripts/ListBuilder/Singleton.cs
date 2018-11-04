using System;
using System.ComponentModel;
using UnityEngine;

public static class Singleton<T> where T : MonoBehaviour {

    [NonSerialized]
    static bool hasBeenCreeated = false;

    [NonSerialized]
    static T instance = null;

    public static T Instance {
        get {
            // use created flag to prevent recreation on quit
            if ( instance == null ) instance = Instantiate();
            return instance;
        }
    }
        
    static T Instantiate() {
        var type = typeof(T);
        var objects = UnityEngine.Object.FindObjectsOfType(type);
        if ( objects != null && objects.Length > 1 )
            Debug.LogError("Present more than one singleton instance of type " + type.Name + " on scene!");

        var instance = objects != null && objects.Length > 0 ? objects[0] as T : null;
        if ( instance == null && !hasBeenCreeated ) {
            Debug.LogFormat("create singleton for type: {0}, realtimeSinceStartup: {1}", type.Name, Time.realtimeSinceStartup);
            //Debug.LogFormat("__________ create singleton for type: {0}\nrealtimeSinceStartup: {1},\n{2}", type.Name, Time.realtimeSinceStartup, StackTraceUtility.ExtractStackTrace());

            var obj = new GameObject(string.Format("_singleton_{0} - delete me in offline mode", type.Name));
            UnityEngine.Object.DontDestroyOnLoad(obj);

            instance = obj.AddComponent<T>();
        }

        hasBeenCreeated = true;
        return instance;
    }
}



