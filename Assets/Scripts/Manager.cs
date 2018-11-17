using System.Collections;
using UnityEngine;
using Newtonsoft;
using System.Linq;
using UnityEngine.UI;
using System;
using System.Globalization;
using System.Collections.Generic;

public class Manager : MonoBehaviour {
    //TODO:
    //load all prev history
    //mer ge (auto-select 5 min?)
    //pick date to see table, table per date
    //move previous data
    //pick range of dates
    //add comment
    //load in packs
    //sum info
    //bars, pies, 

    [SerializeField] FastActionsUI fastActionsUI;
    [SerializeField] TableUI tableUI;
    [SerializeField] MainIconData sleepIconData;
    [SerializeField] MainIconData eatIconData;

    ICurrentUI current;

    [System.NonSerialized] List<DateIntervalManager> managers = new List<DateIntervalManager>();
    BoolManager boobManager;
    //FloatPerDateManager weightManager;
    //FloatPerDateManager lengthManager;

    void Awake() {

        //weightManager = new FloatPerDateManager(Type2Prefix(SaveType.Weight));
        //weightManager.Load();

        //lengthManager = new FloatPerDateManager(Type2Prefix(SaveType.Length));
        //lengthManager.Load();

        {
            var manager = new DateIntervalManager(sleepIconData, Type2Prefix(SaveType.Sleep));
            manager.Load();
            managers.Add(manager);
        }
        {
            var manager = new DateIntervalManager(eatIconData, Type2Prefix(SaveType.Eat));
            manager.Load();
            managers.Add(manager);
        }
        {
            boobManager = new BoolManager(Type2Prefix(SaveType.Left_Right_Boob));
            boobManager.Load();
        }

        fastActionsUI.Open(managers, boobManager);
        current = fastActionsUI;

        tableUI.OnBack += TableUI_OnBack;
    }

    void TableUI_OnBack() {
        SetCurrent(fastActionsUI);
        fastActionsUI.Open(managers, boobManager);
    }

    public void OpenTable(DateIntervalManager manager) {
        SetCurrent(tableUI);
        tableUI.Open(manager);
    }

    void Update() {
        current.Tick();
    }

    void SetCurrent(ICurrentUI next) {
        if ( current != null ) {
            current.Close();
        }
        current = next;
    }

    string Type2Prefix(SaveType type) {
        var i = ( int ) type;
        return i.ToString("000");
    }
}
