using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public static class Helper
{
    public static string GetTableText(DateTime start, DateTime finish, DateTime? previous) {
        var startStr = GetTimeString(start);
        var finishStr = GetTimeString(finish);
        string start2finish = startStr + " -> " + finishStr;

        var deltaSpan = "( " + (finish - start).ToReadableMinutes() + " )";
        string sinceLast = string.Empty;
        if ( previous != null ) {
            sinceLast = (start - previous.Value).ToReadableMinutes();
        }

        string str = string.Format("{0,-20} {1,-15} {2,-10}", start2finish, deltaSpan, sinceLast);

        return str;
    }

    public static string GetTimeString(DateTime date) {
        return date.ToString("t", CultureInfo.CurrentCulture);
    }

    public static string GetDateString(DateTime date) {
        var culture = CultureInfo.CurrentCulture;
        string month = date.ToString("MMMM", culture);
        return string.Format("{0} {1}, {2}", date.Day, month, date.Year);
    }

    public static string ToReadableString(this TimeSpan span) {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} day{1}, ", span.Days, span.Days == 1 ? String.Empty : "s") : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} hour{1}, ", span.Hours, span.Hours == 1 ? String.Empty : "s") : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} minute{1}, ", span.Minutes, span.Minutes == 1 ? String.Empty : "s") : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} second{1}", span.Seconds, span.Seconds == 1 ? String.Empty : "s") : string.Empty);

        if ( formatted.EndsWith(", ") )
            formatted = formatted.Substring(0, formatted.Length - 2);

        if ( string.IsNullOrEmpty(formatted) )
            formatted = "0 seconds";

        return formatted;
    }

    public static string ToReadableMinutes(this TimeSpan span) {
        string formatted = string.Format("{0}{1}{2}",
            span.Duration().Days > 0 ? string.Format("{0:0} d, ", span.Days) : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} h, ", span.Hours) : string.Empty,
            span.Duration().Minutes >= 0 ? string.Format("{0:0} m, ", span.Minutes) : string.Empty);

        if ( formatted.EndsWith(", ") )
            formatted = formatted.Substring(0, formatted.Length - 2);

        if ( string.IsNullOrEmpty(formatted) )
            formatted = string.Empty;

        return formatted;
    }

    public static string ToReadableShort(this TimeSpan span) {
        string formatted = string.Format("{0}{1}{2}{3}",
            span.Duration().Days > 0 ? string.Format("{0:0} d, ", span.Days) : string.Empty,
            span.Duration().Hours > 0 ? string.Format("{0:0} h, ", span.Hours) : string.Empty,
            span.Duration().Minutes > 0 ? string.Format("{0:0} m, ", span.Minutes) : string.Empty,
            span.Duration().Seconds > 0 ? string.Format("{0:0} s, ", span.Seconds) : string.Empty);

        if ( formatted.EndsWith(", ") )
            formatted = formatted.Substring(0, formatted.Length - 2);

        if ( string.IsNullOrEmpty(formatted) )
            formatted = string.Empty;

        return formatted;
    }


    public static IEnumerable<DateTime> GetDatesBetweenIntervals(DateTime start, DateTime finish) {
        if ( start.Date != finish.Date && start < finish ) {
            var day = start.Date.AddDays(1);
            while ( day < finish ) {
                yield return day;
                day = day.AddDays(1);
            }
        }
        yield break;
    }


    public static float HeightDifference(this ScrollRect scrollrect) {
        return Mathf.Max(scrollrect.content.rect.height - (scrollrect.transform as RectTransform).rect.height, 0f);
    }

    public static float HeightDifference(this ScrollRect scrollrect, RectTransform viewport) {
        return Mathf.Max(scrollrect.content.rect.height - viewport.rect.height, 0f);
    }

    public static float TopOffset(this ScrollRect scrollrect) {
        return scrollrect.HeightDifference() * (1f - scrollrect.verticalNormalizedPosition);
    }

    public static float TopOffset(this ScrollRect scrollrect, RectTransform scrollrectTransform) {
        return scrollrect.HeightDifference(scrollrectTransform) * (1f - scrollrect.verticalNormalizedPosition);
    }

    public static float BottomOffset(this ScrollRect scrollrect) {
        return scrollrect.HeightDifference() * scrollrect.verticalNormalizedPosition;
    }

    public static float BottomOffset(this ScrollRect scrollrect, RectTransform scrollrectTransform) {
        return scrollrect.HeightDifference(scrollrectTransform) * scrollrect.verticalNormalizedPosition;
    }

    public static void SetBottomOffset(this ScrollRect scrollrect, float bottomOffset) {
        scrollrect.SetBottomOffset(scrollrect.transform as RectTransform, bottomOffset);
    }

    public static void SetBottomOffset(this ScrollRect scrollrect, RectTransform scrollrectTransform, float bottomOffset) {
        float heighDiff = scrollrect.HeightDifference(scrollrectTransform);
        if ( Mathf.Approximately(heighDiff, 0) ) {
            scrollrect.verticalNormalizedPosition = 0;
        } else {
            scrollrect.verticalNormalizedPosition = Mathf.Clamp01(bottomOffset / heighDiff);
        }
    }

    public static void SetTopOffset(this ScrollRect scrollrect, float topOffset) {
        scrollrect.SetTopOffset(scrollrect.transform as RectTransform, topOffset);
    }


    public static void SetTopOffset(this ScrollRect scrollrect, RectTransform viewport, float topOffset) {
        float heighDiff = scrollrect.HeightDifference(viewport);
        if ( Mathf.Approximately(heighDiff, 0) ) {
            scrollrect.verticalNormalizedPosition = 0;
        } else {
            scrollrect.verticalNormalizedPosition = Mathf.Clamp01(1 - topOffset / heighDiff);
        }
    }

    /// <returns><c>true</c>, if list remains dirty and second update needed
    static public bool SetAreaHeight(float areaHeight, ScrollRect scrollView, RectTransform content, RectTransform scrollViewTransform) {
        bool newRebuildRequired = false;
        if ( content.sizeDelta.y != areaHeight && scrollView.verticalNormalizedPosition == 0 ) {
            //rebuild because of scroll height changed at the bottom of scroll
            newRebuildRequired = true;
        }

        content.sizeDelta = new Vector2(content.sizeDelta.x, areaHeight);

        if ( scrollViewTransform.rect.height > content.sizeDelta.y ) {
            var was = content.anchoredPosition;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
            if ( was != content.anchoredPosition ) {
                Debug.LogError("new rebuild");
                newRebuildRequired = true;
            }
        }
        bool tooFarDown = -content.anchoredPosition.y >= scrollView.viewport.rect.height;
        bool tooFarUp = -content.anchoredPosition.y + areaHeight <= 0;
        if ( tooFarDown || tooFarUp ) {
            var was = content.anchoredPosition;
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, 0);
            if ( was != content.anchoredPosition ) {
                Debug.LogError("new rebuild 2 " + tooFarDown + " " + tooFarUp);
                newRebuildRequired = true;
            }
        }
        return newRebuildRequired;
    }

}