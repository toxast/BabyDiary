using System;
using ui6;

public class BoolActionElementPresenter : Presenter<BoolActionElementView>, IActionPresenter
{
    public float height;
    float IHasHeight.height {
        get {
            return height;
        }
    }
    public BoolManager manager { get; private set; }

    public BoolActionElementPresenter(BoolManager manager, BoolActionElementView prefab, float height) : base(prefab) {
        this.height = height;
        this.manager = manager;
    }

    public void Tick() {
    }

    protected override void RefreshView() {
        if ( IsViewExist ) {
            View.Refresh(manager.value);
        }
    }

    protected override void SubscribeForView() {
        View.OnClick += ClickHandler;
    }

    protected override void UnsubscribeFromView() {
        View.OnClick -= ClickHandler;
    }

    private void ClickHandler() {
        manager.Toggle();
        RefreshView();
    }
   
}
