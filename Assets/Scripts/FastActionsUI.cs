using System;
using System.Collections.Generic;
using ui6;
using UnityEngine;

public class FastActionsUI : MonoBehaviour, ICurrentUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] IntervalActionElementPresenter element;
    [SerializeField] ActionElementView elementPrefab;
    [SerializeField] BoolActionElementView boolElementPrefab;
    [SerializeField] ui6.CustomHeightListBuilder5 listBuilder;
    [NonSerialized] List<IActionPresenter> presenters = new List<IActionPresenter>();

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
        listBuilder.LinkList(new ElementsHolderCustom<IActionPresenter>(presenters));
    }

    public void Open(List<DateIntervalManager> managers, BoolManager boolManager) {
        ClearList();
        foreach ( var item in managers ) {
            var presenter = new IntervalActionElementPresenter(item, elementPrefab, elementPrefab.height);
            presenter.OnTable += () => Presenter_OnTable(presenter);
            presenters.Add(presenter);
        }
        {
            var presenter = new BoolActionElementPresenter(boolManager, boolElementPrefab, boolElementPrefab.height);
            presenters.Add(presenter);
        }
        IsVisible = true;
    }

    private void Presenter_OnTable(IntervalActionElementPresenter presenter) {
        Singleton<Manager>.Instance.OpenTable(presenter.manager);
    }

    public void Tick() {
        foreach ( var item in presenters ) {
            item.Tick();
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
