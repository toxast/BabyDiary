using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ui6
{

    public interface IDrawable : IFilterable
    {
        void Create(Transform parent);
        bool IsViewExist { get; }
        void SetPosition(float x, float y);
        void Release();
    }


    public interface ITickable
    {
        void Tick();
    }

    public interface IActionPresenter : IDrawableCustomHeight, ITickable
    {
    }

    public interface IDrawableCustomHeight : IDrawable, IHasHeight
    {
    }


    public abstract class Presenter<V> : IDrawable
           where V : class, IViewPrefab
    {
        public bool isFiltered { get; set; }
        public bool IsViewExist { get { return View != null; } }

        V prefab;
        protected V View;

        public Presenter(V prefab) {
            this.prefab = prefab;
        }

        public void SetPosition(float x, float y) {
            if ( IsViewExist ) {
                View.SetPosition(x, y);
            }
        }

        public void Create(Transform prnt) {
            if ( IsViewExist == false ) {
                var newView = prefab.GAME_OBJ.Spawn();
                View = newView.GetComponent<V>();
                View.GAME_OBJ.transform.SetParent(prnt, false);
                SubscribeForView();
                RefreshView();
            }
        }

        protected abstract void RefreshView();

        protected abstract void SubscribeForView(); //subscribe for events here
        protected abstract void UnsubscribeFromView(); //unsubscribe from view events here

        public void Release() {
            if ( IsViewExist ) {
                View.BeforeRelease();
                UnsubscribeFromView();
                View.CallRecycle();
                View = null;
            }
        }
    }

    public abstract class SelectablePresenter<VIEW, DATA> : Presenter<VIEW>, ITogglable, IDrawableCustomHeight, IClickable, IHasHeight
           where VIEW : class, IClickable, IViewPrefab
    {
        public DATA data { get; private set; }
        public bool IsOn { get; private set; }
        public float height { get; private set; }
        public event Action OnClick;

        public SelectablePresenter(DATA data, VIEW prefab, float height) : base(prefab) {
            this.data = data;
            this.height = height;
        }

        public void SetState(bool on) {
            IsOn = on;
            RefreshView();
        }

        protected override void SubscribeForView() {
            View.OnClick += View_OnClick;
        }

        protected override void UnsubscribeFromView() {
            View.OnClick -= View_OnClick;
        }

        void View_OnClick() {
            if ( OnClick != null ) {
                OnClick();
            }
        }
    }


    public interface IFilterableByString
    {
        string LowerCaseFilterString { get; }
    }
}
