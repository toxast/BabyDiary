using System;

public interface FilterRequest<T> {
    bool IsActive { get; }
    event Action OnFilterValueChanged;

    bool MatchFilter( T item );
    int OrderID { get; set; }
}

