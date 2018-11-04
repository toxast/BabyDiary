using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "MainIconData")]
[System.Serializable]
public class MainIconData : ScriptableObject{
    [SerializeField] public Color color;
    [SerializeField] public Sprite mainSprite;
    [SerializeField] public List<Sprite> animation;
}
