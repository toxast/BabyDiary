using System;
using System.Collections.Generic;
using ui6;
using UnityEngine;

public class FastActionsUI : MonoBehaviour, ICurrentUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] ActionElementPresenter element;
    [SerializeField] ActionElementView elementPrefab;
    [SerializeField] ui6.CustomHeightListBuilder5 listBuilder;
    [NonSerialized] List<ActionElementPresenter> presenters = new List<ActionElementPresenter>();

    bool _visible = false;
    bool IsVisible {
        get { return _visible; }
        set {
            _visible = value;
            canvas.enabled = value;
            listBuilder.IsActive = value;
        }
    }

    private void Awake() {
        elementPrefab.CreatePool(1);
        listBuilder.LinkList(new ElementsHolderCustom<ActionElementPresenter>(presenters));
    }

    public void Open(List<DateIntervalManager> managers) {
        ClearList();
        foreach ( var item in managers ) {
            var presenter = new ActionElementPresenter(item, elementPrefab, elementPrefab.height);
            presenter.OnTable += () => Presenter_OnTable(presenter);
            presenters.Add(presenter);
        }
        IsVisible = true;
    }

    private void Presenter_OnTable(ActionElementPresenter presenter) {
        Singleton<Manager>.Instance.OpenTable(presenter.manager);
    }

    public void Tick() {
        foreach ( var item in presenters ) {
            item.Refresh();
        }
    }

    public void Close() {
        IsVisible = false;
        ClearList();
    }

    void ClearList() {
        presenters.ForEach(it => it.Release());
        presenters.Clear();
    }
}
