using System;
using ui6;

public class DayPresenter : Presenter<DayElementView>, ITableElement
{
    string text = null;
    public float height;
    public DateTime date { get; private set; }

    float IHasHeight.height {
        get {
            return height;
        }
    }

    public DayPresenter(DateTime date, DayElementView prefab, float height) : base(prefab) {
        this.date = date;
        this.height = height;
    }

    protected override void RefreshView() {
        if ( IsViewExist ) {
            if ( text == null ) {
                text= Helper.GetDateString(date);
            }
            View.Refresh(text);
        }
    }

    protected override void SubscribeForView() {
    }

    protected override void UnsubscribeFromView() {
    }
}
