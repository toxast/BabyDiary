using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;
using System.Linq;

public class FloatPerDateManager : PerDateManager<float>
{
    public FloatPerDateManager(string prefix) : base(prefix) {
    }
}

public struct PerDateRecord<G> 
{
    public Guid guid;
    public DateTime date;
    public G field;

    public PerDateRecord(Guid guid, DateTime date, G field) {
        this.guid = guid;
        this.date = date;
        this.field = field;
    }
}

public class PerDateManager<G> {
    public List<PerDateRecord<G>> records;
    string prefix;
    string NONE = "NONE";
    string _listKey = "list";
    string ListKey {
        get {
            return prefix + _listKey;
        }
    }

    public PerDateManager(string prefix) {
        this.prefix = prefix;
    }

    public void AddRecord(G field) {
        AddRecord(DateTime.Now, field);
        Save();
    }

    public void AddRecord(DateTime date, G field) {
        records.Add(new PerDateRecord<G>(Guid.NewGuid(), date, field));
        Save();
    }

    public void RemoveRecord(Guid guid) {
        int index = records.FindLastIndex(it => it.guid == guid);
        if ( index >= 0 ) {
            records.RemoveAt(index);
            Save();
        }
    }

    public void EditRecord(Guid guid, DateTime date, G field) {
        int index = records.FindLastIndex(it => it.guid == guid);
        if ( index >= 0 ) {
            var record = records[index];
            record.date = date;
            record.field = field;
            Save();
        }
    }

    public void Save() {
        SaveRecords();
    }

    public void Load() {
        LoadRecords();
    }

    private void SaveRecords() {
        string json = JsonConvert.SerializeObject(records, Formatting.Indented);
        PlayerPrefs.SetString(ListKey, json);
    }

    private void LoadRecords() {
        string json = PlayerPrefs.GetString(ListKey, NONE);
        if ( json != NONE ) {
            records = JsonConvert.DeserializeObject<List<PerDateRecord<G>>>(json);
        } else {
            records = new List<PerDateRecord<G>>();
        }
    }
}

public class DateIntervalManager {

    public DateTime? currentIntervalStart { get; private set; }
    public List<DateInterval> intervals = new List<DateInterval>();
    public string captionNoProgress { get; private set; }
    public string captionInProgress { get; private set; }

    string prefix;
    string _listKey = "list";
    string _notFinishedIntervalKey = "notFinished"; 

    string ListKey {
        get {
            return prefix + _listKey;
        }
    }

    string NotFinishedIntervalKey {
        get {
            return prefix + _notFinishedIntervalKey;
        }
    }

    string NONE = "NONE";

    public MainIconData mainIconData;

    public DateIntervalManager(MainIconData mainIconData, string prefix) {
        this.prefix = prefix;
        this.mainIconData = mainIconData;
    }

    public bool IsInProgress {
        get {
            return currentIntervalStart != null;
        }
    }

    public void Start() {
        StartIntervalFrom(DateTime.Now);
    }

    public void StartIntervalFrom(DateTime date) {
        if ( currentIntervalStart == null ) {
            currentIntervalStart = date;
            Save();
        }
    }

    public void EditCurrentInterval(DateTime newdata) {
        if ( currentIntervalStart != null ) {
            currentIntervalStart = newdata;
            Save();
        }
    }

    public void FinishCurrentInterval(DateTime finishTime) {
        if ( currentIntervalStart != null && currentIntervalStart.Value < finishTime) {
            intervals.Add(new DateInterval(Guid.NewGuid(), currentIntervalStart.Value, finishTime));
            currentIntervalStart = null;
            Save();
        }
    }

    public void FinishCurrentInterval() {
        FinishCurrentInterval(DateTime.Now);
    }

    public string GetProgressTime() {
        if ( currentIntervalStart != null ) {
            return (DateTime.Now - currentIntervalStart.Value).ToReadableMinutes();
        }
        return null;
    }

    public string GetSinceTime() {
        if ( currentIntervalStart != null ) {
            return null;
        }
        if ( intervals.Count > 0 ) {
            return (DateTime.Now - intervals.Last().finish).ToReadableMinutes();
        }
        return null;
    }

    public void Remove(Guid guid) {
        int index = intervals.FindLastIndex(it => it.guid == guid);
        if ( index >= 0 ) {
            intervals.RemoveAt(index);
            Save();
        }
    }

    public void Edit(Guid guid, DateTime d1, DateTime d2) {
        int index = intervals.FindLastIndex(it => it.guid == guid);
        if ( index >= 0 ) {
            intervals[index] = new DateInterval(guid, d1, d2);
            Save();
        }
    }

    public void Save() {
        SaveIntervals();
        SaveNotFinishedInterval();
    }

    public void Load() {
        LoadIntervals();
        LoadNotFinishedInterval();
    }

    private void SaveIntervals() {
        string json = JsonConvert.SerializeObject(intervals, Formatting.Indented);
        PlayerPrefs.SetString(ListKey, json);
    }

    private void LoadIntervals() {
        string json = PlayerPrefs.GetString(ListKey, NONE);
        if ( json != NONE ) {
            intervals = JsonConvert.DeserializeObject<List<DateInterval>>(json);
        } else {
            intervals = new List<DateInterval>();
        }
    }

    private void SaveNotFinishedInterval() {
        if ( currentIntervalStart != null ) {
            string json = JsonConvert.SerializeObject(currentIntervalStart.Value, Formatting.Indented);
            PlayerPrefs.SetString(NotFinishedIntervalKey, json);
        } else {
            PlayerPrefs.SetString(NotFinishedIntervalKey, NONE);
        }
    }

    private void LoadNotFinishedInterval() {
        string json = PlayerPrefs.GetString(NotFinishedIntervalKey, NONE);
        if ( json != null && json != NONE ) {
            currentIntervalStart = JsonConvert.DeserializeObject<DateTime>(json);
        } else {
            currentIntervalStart = null;
        }
    }
}
