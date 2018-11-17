using System;
using ui6;
using UnityEngine;
using UnityEngine.UI;

public class BoolActionElementView : BaseView5
{
    [SerializeField] Button toggleButtonLeft;
    [SerializeField] Image imageLeft;
    [SerializeField] Text textLeft;

    [SerializeField] Button toggleButtonRight;
    [SerializeField] Image imageRight;
    [SerializeField] Text textRight;

    [SerializeField] public float height;
    public event Action OnClick = delegate { };

    [SerializeField] Color on;
    [SerializeField] Color off;

    private void Awake() {
        toggleButtonLeft.onClick.AddListener(() => OnClick());
        toggleButtonRight.onClick.AddListener(() => OnClick());
    }

    public void Refresh(bool value) {
        Toggle(value, imageLeft, textLeft);
        Toggle(!value, imageRight, textRight);
    }


    void Toggle(bool value, Image img, Text text) {
        img.enabled = !value;
        text.fontSize = value ? 28 : 24;
        text.color = value ? on : off;
        text.fontStyle = value ? FontStyle.Bold : FontStyle.Normal;
    }
}
