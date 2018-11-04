using System;
using ui6;

public class ActionElementPresenter : Presenter<ActionElementView>, IDrawableCustomHeight
{
    public float height;
    float IHasHeight.height {
        get {
            return height;
        }
    }
    public DateIntervalManager manager { get; private set;}
    public event Action OnTable = delegate { };

    public ActionElementPresenter(DateIntervalManager manager, ActionElementView prefab, float height) : base(prefab) {
        this.height = height;
        this.manager = manager;
    }

    public void Refresh() {
        RefreshView();
    }

    protected override void RefreshView() {
        if ( IsViewExist ) {
            View.Refresh(manager);
        }
    }

    protected override void SubscribeForView() {
        View.OnStart += StartHandler;
        View.OnStartedAt += StartAtHandler;
        View.OnStop += StopHandler;
        View.OnStopedAt += StopedAtHandler;
        View.OnTable += TableHandler;
    }

    protected override void UnsubscribeFromView() {
        View.OnStart -= StartHandler;
        View.OnStartedAt -= StartAtHandler;
        View.OnStop -= StopHandler;
        View.OnStopedAt -= StopedAtHandler;
        View.OnTable -= TableHandler;
    }

    private void StartAtHandler() {
        Singleton<EditDateUI>.Instance.Open(DateTime.Now, (date) => manager.StartIntervalFrom(date), null);
    }

    private void TableHandler() {
        OnTable();
    }

    private void StartHandler() {
        manager.Start();
    }

    private void StopHandler() {
        manager.FinishCurrentInterval();
    }

    private void StopedAtHandler() {
        Singleton<EditDateUI>.Instance.Open(DateTime.Now, (date) => manager.FinishCurrentInterval(date), null);
    }
}
