using System;
using System.Globalization;

public struct DateInterval {
    public Guid guid;
    public DateTime start;
    public DateTime finish;

    public DateInterval(Guid guid, DateTime start, DateTime finish) {
        this.guid = guid;
        this.start = start;
        this.finish = finish;
    }
}
