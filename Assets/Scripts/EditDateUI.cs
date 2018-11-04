using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditDateUI : MonoBehaviour {
    [SerializeField] Canvas canvas;
    [SerializeField] Text weekDayName;

    [SerializeField] DateEditLinks monthLinks;
    [SerializeField] DateEditLinks dayLinks;
    [SerializeField] DateEditLinks hoursLinks;
    [SerializeField] DateEditLinks minutesLinks;

    [SerializeField] Button done;
    [SerializeField] Button cancel;

    DateTime date;
    Action<DateTime> OnDone;
    Action OnCancel;
    Dictionary<InputField, bool> IsFocused;
    Dictionary<InputField, Action> checks;
    private void Awake() {
        IsFocused = new Dictionary<InputField, bool>();
        IsFocused[monthLinks.input] = false;
        IsFocused[dayLinks.input] = false;
        IsFocused[hoursLinks.input] = false;
        IsFocused[minutesLinks.input] = false;

        checks = new Dictionary<InputField, Action>();
        checks[monthLinks.input] = HandleMonthChange;
        checks[dayLinks.input] = HandleDayChange;
        checks[hoursLinks.input] = HandleHourChange;
        checks[minutesLinks.input] = HandleMinutesChange;

        done.onClick.AddListener(HandleDone);
        cancel.onClick.AddListener(HandleCancel);

        //monthLinks.input.onValueChanged.AddListener(HandleMonthChange);
        //dayLinks.input.onValueChanged.AddListener(HandleDayChange);
        //hoursLinks.input.onValueChanged.AddListener(HandleHourChange);
        //minutesLinks.input.onValueChanged.AddListener(HandleMinutesChange);

        SubscribeForModificators(monthLinks);
        SubscribeForModificators(dayLinks);
        SubscribeForModificators(hoursLinks);
        SubscribeForModificators(minutesLinks);
    }

   

    private void Update() {
        if ( canvas.enabled ) {
            foreach ( var item in IsFocused ) {
                bool wasFocused = item.Value;
                if ( wasFocused && !item.Key.isFocused ) {
                    checks[item.Key]();
                    Debug.LogError("check");
                }
               
            }
            foreach ( var item in IsFocused.Keys.ToList() ) {
                IsFocused[item] = item.isFocused;
            }
        }
    }

    private void SubscribeForModificators(DateEditLinks links) {
        foreach ( var item in links.modificators ) {
            var localItem = item;
            localItem.input.onClick.AddListener(() => { AddValueTo(links.input, localItem.delta); });
        }
    }

    private void AddValueTo(InputField input, int delta) {
        int parsedValue;
        if ( int.TryParse(input.text, out parsedValue) ) {
            input.text = (parsedValue + delta).ToString();
            checks[input]();
        }
    }

    private void HandleMonthChange() {
        CheckMinMax(monthLinks.input, date.Month.ToString(), 1, 12, null);
    }

    private void HandleDayChange() {
        int month;
        if ( int.TryParse(monthLinks.input.text, out month) ) {
            int maxDays = DateTime.DaysInMonth(date.Year, Mathf.Clamp(month, 1, 12));
            CheckMinMax(dayLinks.input, date.Day.ToString(), 1, maxDays, monthLinks.input);
            
        }
    }

    void ShowInfoText() {
        var result = TryGetCurrentInputDate();
        if ( result != null ) {
            var val = result.Value;
            
            var weekday = val.ToString("dddd");
            bool set = false;
            if ( DateTime.Now > val ) {
                var interval = (DateTime.Now - val);
                if ( interval.TotalHours < 10 ) {
                    var intervalText = interval.ToReadableMinutes();
                    weekDayName.text = intervalText + " ago";
                    set = true;
                }
            }
            if ( !set ) {
                weekDayName.text = weekday;
            }
        } else {
            weekDayName.text = "";
        }
    }

    private void HandleHourChange() {
        CheckMinMax(hoursLinks.input, date.Hour.ToString(), 0, 23, dayLinks.input);
    }

    private void HandleMinutesChange() {
        CheckMinMax(minutesLinks.input, date.Minute.ToString(), 0, 59, hoursLinks.input);
    }

    private void CheckMinMax(InputField input, string fallback, int min, int max, InputField next) {
        int parsedValue;
        if ( !int.TryParse(input.text, out parsedValue) ) {
            if ( !string.IsNullOrEmpty(input.text) ) {
                input.text = fallback;
                checks[input]();
            }
        } else {
            if ( parsedValue > max ) {
                input.text = min.ToString();
                checks[input]();
                if ( next != null ) {
                    int nextValue;
                    if ( int.TryParse(next.text, out nextValue) ) {
                        next.text = (nextValue + 1).ToString();
                        checks[next]();
                    }
                }
            }
            if ( parsedValue < min ) {
                input.text = max.ToString();
                checks[input]();
                if ( next != null ) {
                    int prevValue;
                    if ( int.TryParse(next.text, out prevValue) ) {
                        next.text = (prevValue - 1).ToString();
                        checks[next]();
                    }
                }
            }
        }
        ShowInfoText();
    }

    public void Open(DateTime date, Action<DateTime> OnDone, Action OnCancel) {
        canvas.enabled = true;
        this.date = date;
        this.OnDone = OnDone;
        this.OnCancel = OnCancel;
        monthLinks.input.text = date.Month.ToString();
        dayLinks.input.text = date.Day.ToString();
        hoursLinks.input.text = date.Hour.ToString();
        minutesLinks.input.text = date.Minute.ToString();
        HandleDayChange();
    }

    private void HandleCancel() {
        canvas.enabled = false;
        if ( OnCancel != null ) {
            OnCancel();
        }
    }

    private void HandleDone() {
        var result = TryGetCurrentInputDate();
        if ( result != null ) {
            canvas.enabled = false;
            if ( OnDone != null ) {
                OnDone(result.Value);
            }
        }
    }

    DateTime? TryGetCurrentInputDate() {
        try {
            int month;
            int day;
            int hour;
            int minute;

            if (
                int.TryParse(monthLinks.input.text, out month) &&
                int.TryParse(dayLinks.input.text, out day) &&
                int.TryParse(hoursLinks.input.text, out hour) &&
                int.TryParse(minutesLinks.input.text, out minute)
            ) {
                DateTime resultDate = new DateTime(date.Year, month, day, hour, minute, 0, DateTimeKind.Local);
                return resultDate;
            }
        } catch ( Exception ) {
            Debug.LogError("error");
            return null;
        }
        return null;
    }


    [System.Serializable]
    public class DateEditLinks {
        [SerializeField] public List<Modificator> modificators;
        [SerializeField] public InputField input;
    }

    [System.Serializable]
    public class Modificator
    {
        [SerializeField] public int delta;
        [SerializeField] public Button input;
    }
}


