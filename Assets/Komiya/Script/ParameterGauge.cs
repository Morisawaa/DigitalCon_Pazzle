using UnityEngine;
using UnityEngine.UI;

using Value;
namespace Paramete
{
    public class ParameterGauge : MonoBehaviour
    {
        //===========================================
        //担当者:小宮純
        //機能:親子のパラメータをゲージに反映
        //===========================================


        //
        //他のスクリプトでParentParameter,ChilParameterを変更する場合、同じ個所でChangeGagueを呼び出してください
        //Paramater引数には、ValueManagementのChildParamaterまたはParentParameterを利用してください。
        //Targetイメージには呼び出し時に親子それぞれのImageを利用してください。
        //Debug用にキー入力で値が変わるようになっています。
        //




        [Header("値管理データ[ValueManagement]のScriptableObject")]
        [SerializeField] private ValueManagement valueManagement;

        [Header("子供or親のパラメータを反映するUI.Image")]
        [SerializeField] private Image parentGauge;
        [SerializeField] private Image childGauge;

        private float maxHeight;
        private int maxParameter;

        private void Start()
        {
            //Starで呼び出すとシーン遷移毎に初期化される心配あり
            InitializeParamater();




            if (valueManagement == null)
            {
                valueManagement = GetComponent<ValueManagement>();
                maxParameter = valueManagement.MaxParameter;
            }

            // 親ゲージのsizeDeltaから最大の高さを取得する
            // rect.heightよりもsizeDeltaの方が安定していることが多いです
            if (parentGauge != null)
            {
                maxHeight = parentGauge.rectTransform.sizeDelta.y;
            }
            else
            {
                Debug.LogError("ParentGaugeが設定されていません！");
                return;
            }
            maxParameter = valueManagement.MaxParameter;

            // 各ゲージの高さを更新
            ChangeGauge(valueManagement.ParentParameter, parentGauge);
            ChangeGauge(valueManagement.ChildParameter, childGauge);
        }

        private void Update()
        {
            //デバッグ用
            if (Input.GetKeyDown(KeyCode.I))
            {
                valueManagement.ParentParameter++;
                if (valueManagement.ParentParameter >= maxParameter) valueManagement.ParentParameter = maxParameter;

                ChangeGauge(valueManagement.ParentParameter, parentGauge);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                valueManagement.ChildParameter++;
                if (valueManagement.ChildParameter >= maxParameter) valueManagement.ChildParameter = maxParameter;

                ChangeGauge(valueManagement.ChildParameter, childGauge);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                        valueManagement.ParentParameter--;
                if (valueManagement.ParentParameter <= 0) valueManagement.ParentParameter = 0;

                ChangeGauge(valueManagement.ParentParameter, parentGauge);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                valueManagement.ChildParameter--;
                if (valueManagement.ChildParameter <= 0) valueManagement.ChildParameter = 0;

                ChangeGauge(valueManagement.ChildParameter, childGauge);
            }
        }


        public void ChangeGauge(int Parameter, Image TargetImage)
        {
            Debug.Log($"ChangeGauge呼び出し: Parameter={Parameter}, TargetImage={(TargetImage != null ? TargetImage.name : "null")}");
            
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
            float currentParameter = Mathf.Clamp(Parameter, 0, maxParameter);

            // (現在値 / 最大値) の割合を計算
            float ratio = currentParameter / (float)maxParameter;

            // 割合に応じて高さを計算
            float newHeight = maxHeight * ratio;

            // 計算した高さをVector2のyに設定
            size.y = newHeight;

            // 【修正点3】変更したsizeをrectTransformに再設定する
            rectTransform.sizeDelta = size;
            
            Debug.Log($"ChangeGauge完了: 新しい高さ={newHeight}, 比率={ratio}");
        }

        /// <summary>
        /// 値の初期化
        /// </summary>
        public void InitializeParamater()
        {
            Debug.LogWarning("パラメータが初期化されました");
            valueManagement.ParentParameter = valueManagement.InitialParentParamater;
            valueManagement.ChildParameter = valueManagement.InitialChildParamater;
        }

        /// <summary>
        /// 親ゲージのImageを取得
        /// </summary>
        public Image GetParentGaugeImage()
        {
            Debug.Log($"GetParentGaugeImage: parentGauge={(parentGauge != null ? parentGauge.name : "null")}");
            return parentGauge;
        }

        /// <summary>
        /// 子ゲージのImageを取得
        /// </summary>
        public Image GetChildGaugeImage()
        {
            Debug.Log($"GetChildGaugeImage: childGauge={(childGauge != null ? childGauge.name : "null")}");
            return childGauge;
        }
    }
}