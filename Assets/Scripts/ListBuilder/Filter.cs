using System;
using System.Collections.Generic;

public interface Filter<T> {
    bool IsActive { get; }
    string NoMatchesFoundMessage { get; }

    void ConnectRequest(FilterRequest<T> request);
    void DisconnectRequest(FilterRequest<T> request);

    bool PassFilter(T item);
    event Action OnFilterChanged;

    void FireOnFilterChanged();
}

public sealed class FilterImpl<T> : Filter<T> {

    #region Implementation

    public bool IsActive {
        get {
            foreach ( var request in connectedRequests ) {
                if( request.IsActive )
                    return true;
            }

            return false;
        }
    }

    public string NoMatchesFoundMessage { get{ return "no match"; } }

    readonly List<FilterRequest<T>> connectedRequests = new List<FilterRequest<T>>();
    public void ConnectRequest(FilterRequest<T> request) {
        connectedRequests.Add(request);

        if ( connectedRequests.Count > 0 )
            connectedRequests.Sort((request1, request2) => request1.OrderID.CompareTo(request2.OrderID));

        request.OnFilterValueChanged += FireOnFilterChanged;
		FireOnFilterChanged ();
    }

    public void DisconnectRequest(FilterRequest<T> request) {
        bool removed = connectedRequests.Remove(request);
		if(!removed) UnityEngine.Debug.LogError("wtf");
        request.OnFilterValueChanged -= FireOnFilterChanged;
		FireOnFilterChanged ();
    }

    public bool PassFilter(T item) {
        if ( connectedRequests.Count == 0 )
            return true;

        foreach ( var request in connectedRequests ) {
            if ( !request.MatchFilter(item) )
                return false;
        }

        return true;
    }

    bool NotPassFilter(T item) { return !PassFilter(item); }

    public List<T> PassFilter(List<T> items) {
        if ( items == null )
            return null;

        return items.FindAll(PassFilter);
    }

    public List<T> ExcludeFilter(List<T> items) {
        if ( items == null )
            return null;

        return items.FindAll(NotPassFilter);
    }

    void FilterChanged() {
        OnFilterChanged();
    }

    public event Action OnFilterChanged;
    public void FireOnFilterChanged() {
        if ( OnFilterChanged != null )
            OnFilterChanged();
    }

    #endregion

}
