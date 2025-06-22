using UnityEngine;
using UnityEngine.UI;

public class ParameterGauge : MonoBehaviour
{
    //
    //他のスクリプトでParentParameter,ChilParameterを変更する場合、同じ個所でChangeGagueを呼び出してください
    //Paramater引数には、ValueManagementのChildParamaterまたはParentParameterを利用してください。
    //Targetイメージには呼び出し時に親子それぞれのImageを利用してください。
    //Debug用にキー入力で値が変わるようになっています。
    //




    [Header("値管理データ[ValueManagement]のScriptableObject")]
    [SerializeField] private ValueManagement ValueManagement_;

    [Header("子供,親のパラメータを反映するイメージ")]
    [SerializeField] private Image ParentGauge;
    [SerializeField] private Image ChildGauge;

    private float MaxHeight;
    private int MaxParameter;

    private void Start()
    {
        //Starで呼び出すとシーン遷移毎に初期化される心配あり
        InitializeParamater();




        if (ValueManagement_ == null)
        {
            ValueManagement_ = GetComponent < ValueManagement>();
            MaxParameter = ValueManagement_.MaxParameter;
        }

        // 親ゲージのsizeDeltaから最大の高さを取得する
        // rect.heightよりもsizeDeltaの方が安定していることが多いです
        if (ParentGauge != null)
        {
            MaxHeight = ParentGauge.rectTransform.sizeDelta.y;
        }
        else
        {
            Debug.LogError("ParentGaugeが設定されていません！");
            return;
        }
        MaxParameter = ValueManagement_.MaxParameter;

        // 各ゲージの高さを更新
        ChangeGauge(ValueManagement_.ParentParameter, ParentGauge);
        ChangeGauge(ValueManagement_.ChildParameter, ChildGauge);
    }

    private void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.I))
        {
            ValueManagement_.ParentParameter++;
            if(ValueManagement_.ParentParameter >= MaxParameter) ValueManagement_.ParentParameter = MaxParameter; 

            ChangeGauge(ValueManagement_.ParentParameter, ParentGauge);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ValueManagement_.ChildParameter++;
            if (ValueManagement_.ChildParameter >= MaxParameter) ValueManagement_.ChildParameter = MaxParameter;

            ChangeGauge(ValueManagement_.ChildParameter, ChildGauge);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ValueManagement_.ParentParameter--;
            if (ValueManagement_.ParentParameter <= 0) ValueManagement_.ParentParameter = 0;

            ChangeGauge(ValueManagement_.ParentParameter, ParentGauge);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ValueManagement_.ChildParameter--;
            if (ValueManagement_.ChildParameter <= 0) ValueManagement_.ChildParameter = 0;

            ChangeGauge(ValueManagement_.ChildParameter, ChildGauge);
        }
    }

    public void ChangeGauge(int Parameter, Image TargetImage)
    {
        // 【修正点1】Nullチェックの条件を修正
        if (TargetImage == null)
        {
            // エラーメッセージのスペルも修正
            Debug.LogError("TargetImageがNullです");
            return;
        }

        RectTransform rectTransform = TargetImage.rectTransform;
        Vector2 size = rectTransform.sizeDelta;

        // 【修正点2】高さの計算方法を修正
        // パラメータが0未満にならないようにClamp（制限）する
        float currentParameter = Mathf.Clamp(Parameter, 0, MaxParameter);

        // (現在値 / 最大値) の割合を計算
        float ratio = currentParameter / (float)MaxParameter;

        // 割合に応じて高さを計算
        float newHeight = MaxHeight * ratio;

        // 計算した高さをVector2のyに設定
        size.y = newHeight;

        // 【修正点3】変更したsizeをrectTransformに再設定する
        rectTransform.sizeDelta = size;
    }

    /// <summary>
    /// 値の初期化
    /// </summary>
    public void InitializeParamater()
    {
        Debug.LogWarning("パラメータが初期化されました");
        ValueManagement_.ParentParameter = ValueManagement_.InitialParentParamater;
        ValueManagement_.ChildParameter = ValueManagement_.InitialChildParamater;
    }
}