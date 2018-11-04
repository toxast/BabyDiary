using System;
using ui6;

public class TableCurrentIntervalPresenter : Presenter<TableElementView>, ITableElement
{
    string text = null;
    public float height;
    public DateTime start { get; private set; }
    DateTime? previous;
    public event Action OnEdit = delegate { };

    float IHasHeight.height {
        get {
            return height;
        }
    }

    public TableCurrentIntervalPresenter(DateTime start, DateTime? previous, TableElementView prefab, float height) : base(prefab) {
        this.start = start;
        this.height = height;
        this.previous = previous;
    }

    protected override void RefreshView() {
        if ( IsViewExist ) {
            if ( text == null ) {
                var textParam = Helper.GetTableText(start, DateTime.Now, previous);
                text = string.Format(@"<color={0}>{1}</color>", "blue", textParam);
            }
            View.Refresh(text, true);
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