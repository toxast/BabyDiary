using UnityEngine;
using ui6;
using UnityEngine.UI;
using System;

public class TableElementView : BaseView5
{
    [SerializeField] Text label;
    [SerializeField] public float height;
    [SerializeField] Button edit;

    public event Action OnEdit = delegate { };

    public void Refresh(string text, bool editAvaliable) {
        label.text = text;
        edit.gameObject.SetActive(editAvaliable);
        edit.onClick.AddListener(() => OnEdit());
    }
}

