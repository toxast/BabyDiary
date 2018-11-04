using System;
using System.Collections;
using System.Collections.Generic;
using ui6;
using UnityEngine;
using UnityEngine.UI;

public class ActionElementView : BaseView5
{
    [SerializeField] Image mainIcon;
    [SerializeField] Text descriptionLabel;
    [SerializeField] Button startButton;
    [SerializeField] Button startAtButton;
    [SerializeField] Button stopButton;
    [SerializeField] Button stopedAtButton;
    [SerializeField] Button tableButton;
    [SerializeField] public float height;

    public event Action OnStart = delegate { };
    public event Action OnStartedAt = delegate { };
    public event Action OnStop = delegate { };
    public event Action OnStopedAt = delegate { };
    public event Action OnTable = delegate { };

    DateIntervalManager manager;
    private Coroutine routine;

    private void Awake() {
        startButton.onClick.AddListener(() => OnStart());
        startAtButton.onClick.AddListener(() => OnStartedAt());
        stopButton.onClick.AddListener(() => OnStop());
        stopedAtButton.onClick.AddListener(() => OnStopedAt());
        tableButton.onClick.AddListener(() => OnTable());
    }

    public void Refresh(DateIntervalManager manager) {

        //string caption = manager.IsInProgress ? manager.captionInProgress : manager.captionNoProgress;
        bool sameManager = this.manager == manager;
        bool sameAnimationRunning = AnimationIsRunning && sameManager;

        this.manager = manager;

      
        bool animationShouldRun = manager.IsInProgress;

        string description = string.Empty;
        var progress = manager.GetProgressTime();
        if ( progress != null ) {
            description = progress;
        } else {
            var since = manager.GetSinceTime();
            if ( since != null ) {
                description = since + " ago";
            }
        }

        //captionlabel.text = caption;
        descriptionLabel.text = description;
        startButton.gameObject.SetActive(!manager.IsInProgress);
        startAtButton.gameObject.SetActive(!manager.IsInProgress);
        stopButton.gameObject.SetActive(manager.IsInProgress);
        stopedAtButton.gameObject.SetActive(manager.IsInProgress);

        if ( !sameManager ) {
            mainIcon.color = manager.mainIconData.color;
        }
        if ( !animationShouldRun ) {
            if ( AnimationIsRunning ) {
                StopAnim();
                mainIcon.sprite = manager.mainIconData.mainSprite;
            }
            if ( !sameManager ) {
                mainIcon.sprite = manager.mainIconData.mainSprite;
            }
        } else if ( animationShouldRun ) {
            if ( !sameAnimationRunning ) {
                RestartAnim();
            }
        } 
    }

    public override void BeforeRelease() {
        base.BeforeRelease();
        StopAnim();
    }

    void RestartAnim() {
        StopAnim();
        routine = StartCoroutine(Animation());
    }

    bool AnimationIsRunning {
        get {
            return routine != null;
        }
    }

    void StopAnim() {
        if ( routine != null ) {
            StopCoroutine(routine);
            routine = null;
        }
    }

    IEnumerator Animation() {
        int current = 0;
        var list = manager.mainIconData.animation;
        mainIcon.sprite = list[current];
        while ( true ) {
            yield return new WaitForSeconds(1f);
            current++;
            if ( current >= list.Count ) {
                current = 0;
            }
            mainIcon.sprite = list[current];
        }
    }
}
