using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Utils;

public class Drawer : MonoBehaviour
{
    public RectTransform Content;

    public float AnimationTime = 2f;

    private bool _isOpen;

    private float _width;
    private float _closedXPosition;
    private float _openXPosition;

    private int? _moveDrawerTweenId;

    // Start is called before the first frame update
    void Start()
    {
        _width = Content.sizeDelta.x;
        _closedXPosition = 0f - _width;
        _openXPosition = 0f;

        _isOpen = true;
        CloseDrawer(snap: true);
    }

    public void CloseDrawer(bool snap = false)
    {
        if (_isOpen == false)
            return;
        _isOpen = false;
        Vector3 newPos = new Vector2(_closedXPosition, Content.localPosition.y);

        if (snap)
        {
            Content.localPosition = newPos;
        }
        else
        {
            float time = AnimationTime;

            if (_moveDrawerTweenId.HasValue)
            {
                LeanTween.cancel(_moveDrawerTweenId.Value);
                _moveDrawerTweenId = null;
                time = GetAnimationTime(newPos.x);
            }

            _moveDrawerTweenId = LeanTween.moveLocal(Content.gameObject, newPos, time).setEase(LeanTweenType.easeInOutBack).id;
            LeanTween.descr(_moveDrawerTweenId.Value).setOnComplete(() =>
            {
                LeanTween.cancel(_moveDrawerTweenId.Value);
                _moveDrawerTweenId = null;
            });
        }
    }

    public void OpenDrawer()
    {
        if (_isOpen)
            return;
        _isOpen = true;
        Vector3 newPos = new Vector2(_openXPosition, Content.localPosition.y);

        float time = AnimationTime;

        if (_moveDrawerTweenId.HasValue)
        {
            LeanTween.cancel(_moveDrawerTweenId.Value);
            _moveDrawerTweenId = null;
            time = GetAnimationTime(newPos.x, isClosing: true);
        }

        _moveDrawerTweenId = LeanTween.moveLocal(Content.gameObject, newPos, time).setEase(LeanTweenType.easeInOutExpo).id;
        LeanTween.descr(_moveDrawerTweenId.Value).setOnComplete(() =>
        {
            LeanTween.cancel(_moveDrawerTweenId.Value);
            _moveDrawerTweenId = null;
        });
    }

    private float GetAnimationTime(float newPosX, bool isClosing = false)
    {
        return AnimationTime / 2;
        ////float percent = UsefullUtils.GetValuePercent(Content.localPosition.x, newPosX);
        ////if (isClosing)
        ////{
        //    var percent = UsefullUtils.GetValuePercent(System.Math.Abs(Content.localPosition.x), _width);
        ////}

        //var actualTime = AnimationTime - UsefullUtils.GetPercent(AnimationTime, percent);
        //Debug.Log(actualTime);
        //return actualTime;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.Q))
        //{
        //    CloseDrawer();
        //}
        //if (Input.GetKeyUp(KeyCode.E))
        //{
        //    OpenDrawer();
        //}
    }
}
