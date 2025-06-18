using UnityEngine;
using UnityEngine.UI;

public class ParameterGauge : MonoBehaviour
{
    [SerializeField] private DebugParameter DebugParameter_;
    [SerializeField] private Image ParentGauge;
    [SerializeField] private Image ChildGauge;

    private float MaxHeight;
    // パラメータの最大値をインスペクターから設定できるようにする
    private int MaxParameter = 10;

    private void Start()
    {
        //現在はDebugParameterから、親子のパラメーターとその上限値を参照している
        //必要な場合この参照先を変更

        if (DebugParameter_ == null)
        {
            DebugParameter_ = GetComponent<DebugParameter>();
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
        MaxParameter = DebugParameter_.MaxParameter;

        // 各ゲージの高さを更新
        ChangeGauge(DebugParameter_.ParentParameter, ParentGauge);
        ChangeGauge(DebugParameter_.ChildParameter, ChildGauge);
    }

    private void Update()
    {
        //デバッグ用
        if (Input.GetKeyDown(KeyCode.I))
        {
            DebugParameter_.ParentParameter++;
            if(DebugParameter_.ParentParameter >= MaxParameter) DebugParameter_.ParentParameter = MaxParameter; 

            ChangeGauge(DebugParameter_.ParentParameter, ParentGauge);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            DebugParameter_.ChildParameter++;
            if (DebugParameter_.ChildParameter >= MaxParameter) DebugParameter_.ChildParameter = MaxParameter;

            ChangeGauge(DebugParameter_.ChildParameter, ChildGauge);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            DebugParameter_.ParentParameter--;
            if (DebugParameter_.ParentParameter <= 0) DebugParameter_.ParentParameter = 0;

            ChangeGauge(DebugParameter_.ParentParameter, ParentGauge);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            DebugParameter_.ChildParameter--;
            if (DebugParameter_.ChildParameter <= 0) DebugParameter_.ChildParameter = 0;

            ChangeGauge(DebugParameter_.ChildParameter, ChildGauge);
        }
    }

    private void ChangeGauge(int Parameter, Image TargetImage)
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
}