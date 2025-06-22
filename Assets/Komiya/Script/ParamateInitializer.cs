using UnityEngine;

public class ParamateInitializer : MonoBehaviour
{
    //
    //ゲーム内で使う値を初期化するためのスクリプトです。
    //随時追加する可能性があります。
    //
    //


    [SerializeField] private ValueManagement ValueManagement_;

    private void Start()
    {
        AllInitialize();
    }

    /// <summary>
    /// 全ての値を初期化(随時追加)
    /// </summary>
    private void AllInitialize()
    {
        DayInitialize();
        ParamateInitialize();
        Debug.LogWarning("全てのパラメータを初期化しました!");
    }

    /// <summary>
    /// 日付を初期化
    /// </summary>
    private void DayInitialize()
    {
        ValueManagement_.WhatDay = ValueManagement_.InitialWhatDay;
        Debug.LogWarning("日付を初期化しました");
    }

    /// <summary>
    /// パラメータを初期化
    /// </summary>
    private void ParamateInitialize()
    {
        ValueManagement_.ParentParameter = ValueManagement_.InitialParentParamater;
        ValueManagement_.ChildParameter = ValueManagement_.InitialChildParamater;
        Debug.LogWarning("パラメータを初期化しました");
    }
}
