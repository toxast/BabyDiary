using UnityEngine;
using ui6;
using UnityEngine.UI;

public class DayElementView : BaseView5
{
    [SerializeField] Text label;
    [SerializeField] public float height;

    public void Refresh(string text) {
        label.text = text;
    }
}

