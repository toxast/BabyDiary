using System;
using System.Diagnostics;



public abstract class SearchRequest5<T> : FilterRequest<T> {
	
	public int OrderID { get; set; }
	
	public event Action OnFilterValueChanged = delegate { };
	
	public string StringRequest { get; set; }
	
	public virtual bool IsActive { get { return !string.IsNullOrEmpty(StringRequest); } }
	public SimpleFilterViewImpl5 view;
    bool castStringToLower;

    public SearchRequest5(SimpleFilterViewImpl5 view, int orderId = 0, bool castStringToLower = false) {
		this.view = view;
		this.view.OnFilterValueChanged += OnFilterChanged;
        this.castStringToLower = castStringToLower;
        OrderID = orderId;
        StringRequest = string.Empty;
        RefreshValue();
    }
	
	public abstract bool MatchFilter(T item);

    public void RefreshValue() {
        if (view != null && view.TextField != null) {
            if (castStringToLower) {
                StringRequest = view.TextField.text.ToLower();
            } else {
                StringRequest = view.TextField.text;
            }
        }
    }

	void OnFilterChanged() {
		if ( view == null )
			return;

        string wasString = StringRequest;
        RefreshValue();
        if (StringRequest != wasString) {
            OnFilterValueChanged();
        }
	}
	
}
