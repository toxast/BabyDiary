using UnityEngine;
using Newtonsoft.Json;

public class BoolManager
{
    public bool value;

    string _ValueKey = "value";
    string prefix;

    public BoolManager(string prefix) {
        this.prefix = prefix;
    }

    string SaveKey {
        get {
            return prefix + _ValueKey;
        }
    }

    public void Set(bool value) {
        this.value = value;
        Save();
    }

    public void Toggle() {
        Set(!value);
    }

    private void Save() {
        string json = JsonConvert.SerializeObject(value, Formatting.Indented);
        PlayerPrefs.SetString(SaveKey, json);
    }

    public void Load() {
        string json = PlayerPrefs.GetString(SaveKey);
        if (!string.IsNullOrEmpty(json)) {
            value = JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
