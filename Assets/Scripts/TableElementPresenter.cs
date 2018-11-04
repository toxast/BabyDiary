using System;
using ui6;

public class TableIntervalPresenter : Presenter<TableElementView>, ITableElement
{
    string text = null;
    public float height;
    public DateInterval interval { get; private set; }
    DateTime? previous;

    float IHasHeight.height {
        get {
            return height;
        }
    }

    public event Action OnEdit = delegate { };

    public TableIntervalPresenter(DateInterval interval, DateTime? previous, TableElementView prefab, float height) : base(prefab) {
        this.interval = interval;
        this.height = height;
        this.previous = previous;
    }

    protected override void RefreshView() {
        if ( IsViewExist ) {
            if ( text == null ) {
                text = Helper.GetTableText(interval.start, interval.finish, previous);
            }
            View.Refresh(text, editAvaliable:true);
        }
    }

    protected override void SubscribeForView() {
        View.OnEdit += HandleEdit;
    }
    
    protected override void UnsubscribeFromView() {
        View.OnEdit -= HandleEdit;
    }

    private void HandleEdit() {
        OnEdit();
    }

}
