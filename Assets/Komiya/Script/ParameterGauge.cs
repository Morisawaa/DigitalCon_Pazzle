using UnityEngine;
using UnityEngine.UI;

using Value;
namespace Paramete
{
    public class ParameterGauge : MonoBehaviour
    {

        [Header("�l�Ǘ��f�[�^[ValueManagement]��ScriptableObject")]
        [SerializeField] private ValueManagement valueManagement;

        [Header("�q��or�e�̃p�����[�^�𔽉f����UI.Image")]

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


            InitializeParamater();




            if (valueManagement == null)
            {
                valueManagement = GetComponent<ValueManagement>();
                maxParameter = valueManagement.MaxParameter;
            }



            if (parentGauge != null)
            {
                maxHeight = parentGauge.rectTransform.sizeDelta.y;
            }
            else
            {

            ChangeGauge(valueManagement.ParentParameter, parentGauge);
            ChangeGauge(valueManagement.ChildParameter, childGauge);
        }

        private void Update()
        {


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

                return;
            }

            RectTransform rectTransform = TargetImage.rectTransform;
            Vector2 size = rectTransform.sizeDelta;


            rectTransform.sizeDelta = size;
            
            Debug.Log($"ChangeGauge完了: 新しい高さ={newHeight}, 比率={ratio}");
        }

        /// <summary>

            valueManagement.ParentParameter = valueManagement.InitialParentParamater;
            valueManagement.ChildParameter = valueManagement.InitialChildParamater;
        }

        /// <summary>

            return parentGauge;
        }

        /// <summary>

            return childGauge;
        }
    }
}