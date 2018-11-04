using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ui6;
using UnityEngine.UI;

public class TableUI : MonoBehaviour, ICurrentUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] Button back;
    [SerializeField] TableElementView elementPrefab;
    [SerializeField] DayElementView dayPrefab;
    [SerializeField] ui6.CustomHeightListBuilder5 listBuilder;

    [NonSerialized] public List<ITableElement> presenters = new List<ITableElement>();
    public event Action OnBack = delegate { };
    DateIntervalManager manager;

    bool _visible = false;
    bool IsVisible {
        get { return _visible; }
        set {
            _visible = value;
            canvas.enabled = value;
            listBuilder.IsActive = value;
        }
    }

    void Awake() {
        dayPrefab.CreatePool(1);
        elementPrefab.CreatePool(1);
        listBuilder.LinkList(new ElementsHolderCustom<ITableElement>(presenters));
        back.onClick.AddListener(() => OnBack());
    }

    public void Open(DateIntervalManager manager) {
        this.manager = manager;
        IsVisible = true;
        Refresh();
        listBuilder.scrollView.SetBottomOffset(0);
    }


    void Refresh() {
        ClearList();
        DateTime? previous = null;
        foreach ( var item in manager.intervals ) {
            CreateDay(previous, item.start);
            var elem = new TableIntervalPresenter(item, previous, elementPrefab, elementPrefab.height);
            elem.OnEdit += () => HandleIntervalEdit(elem);
            presenters.Add(elem);
            previous = item.finish;
            if ( item.start.Date != item.finish.Date ) {
                CreateDay(item.finish);
            }
        }

        var currentIntervalStart = manager.currentIntervalStart;
        if ( currentIntervalStart != null ) {
            CreateDay(previous, currentIntervalStart.Value);
            var elem = new TableCurrentIntervalPresenter(currentIntervalStart.Value, previous, elementPrefab, elementPrefab.height);
            elem.OnEdit += () => HandleCurrentEdit(elem);
            presenters.Add(elem);
        }
        listBuilder.SetDirty();
    }

    void CreateDay(DateTime? previous, DateTime date) {
        if ( previous != null ) {
            if ( previous.Value.Date != date.Date ) {
                CreateDay(date);
            }
        } else {
            CreateDay(date);
        }
    }

    void CreateDay(DateTime date) {
        presenters.Add(new DayPresenter(date, dayPrefab, dayPrefab.height));
    }

    private void HandleCurrentEdit(TableCurrentIntervalPresenter elem) {
        Singleton<EditDateUI>.Instance.Open(elem.start, (newdata) => { manager.EditCurrentInterval(newdata); Refresh(); }, null);
    }

    private void HandleIntervalEdit(TableIntervalPresenter elem) {
        Singleton<EditIntervalUI>.Instance.Open(elem.interval, (d1, d2) => HandleEditDone(elem, d1, d2), null, () => HandleDelete(elem));
    }

    private void HandleDelete(TableIntervalPresenter elem) {
        manager.Remove(elem.interval.guid);
        Refresh();
    }

    private void HandleEditDone(TableIntervalPresenter elem, DateTime d1, DateTime d2) {
        manager.Edit(elem.interval.guid, d1, d2);
        Refresh();
    }

    void ICurrentUI.Close() {
        IsVisible = false;
        ClearList();
    }

    void ClearList() {
        presenters.ForEach(it => it.Release());
        presenters.Clear();
        listBuilder.SetDirty();
    }

    void ICurrentUI.Tick() {
    }
}
