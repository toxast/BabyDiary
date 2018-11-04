using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditIntervalUI : MonoBehaviour {
    [SerializeField] Canvas canvas;

    [SerializeField] ButtonWithText dateFrom;
    [SerializeField] ButtonWithText dateTo;

    [SerializeField] Button done;
    [SerializeField] Button cancel;
    [SerializeField] Button delete;

    DateInterval result;
    public event Action<DateTime, DateTime> OnDone;
    public event Action OnCancel;
    public event Action OnDelete;

    void Awake () {
        done.onClick.AddListener(HandleDone);
        cancel.onClick.AddListener(HandleCancel);
        delete.onClick.AddListener(HandleDelete);
        dateFrom.button.onClick.AddListener(HandleFromClick);
        dateTo.button.onClick.AddListener(HandleToClick);
    }

    public void Open(DateInterval date, Action<DateTime, DateTime> OnDone, Action OnCancel, Action OnDelete) {
        canvas.enabled = true;
        //this.date = date;
        this.OnDone = OnDone;
        this.OnCancel = OnCancel;
        this.OnDelete = OnDelete;
        result = date;
        Refresh();
    }

    void Refresh() {
        dateFrom.text.text = Helper.GetTimeString(result.start);
        dateTo.text.text = Helper.GetTimeString(result.finish);
    }

    private void HandleFromClick() {
        Singleton<EditDateUI>.Instance.Open(result.start, (date) => { result.start = date; Refresh(); }, null);
    }
    private void HandleToClick() {
        Singleton<EditDateUI>.Instance.Open(result.finish, (date) => {result.finish = date; Refresh(); }, null);
    }


    private void HandleCancel() {
        canvas.enabled = false;
        if ( OnCancel != null ) {
            OnCancel();
        }
    }

    private void HandleDone() {
        canvas.enabled = false;
        if ( OnDone != null ) {
            OnDone(result.start, result.finish);
        }
    }

    private void HandleDelete() {
        canvas.enabled = false;
        if ( OnDelete != null ) {
            OnDelete();
        }
    }
}
