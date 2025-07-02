using UnityEngine;
using UnityEngine.UI;

using Value;
namespace Paramete
{
    public class ParameterGauge : MonoBehaviour
    {
        //===========================================
        //�S����:���{��
        //�@�\:�e�q�̃p�����[�^���Q�[�W�ɔ��f
        //===========================================


        //
        //���̃X�N���v�g��ParentParameter,ChilParameter��ύX����ꍇ�A��������ChangeGague���Ăяo���Ă�������
        //Paramater�����ɂ́AValueManagement��ChildParamater�܂���ParentParameter�𗘗p���Ă��������B
        //Target�C���[�W�ɂ͌Ăяo�����ɐe�q���ꂼ���Image�𗘗p���Ă��������B
        //Debug�p�ɃL�[���͂Œl���ς��悤�ɂȂ��Ă��܂��B
        //




        [Header("�l�Ǘ��f�[�^[ValueManagement]��ScriptableObject")]
        [SerializeField] private ValueManagement valueManagement;

        [Header("�q��or�e�̃p�����[�^�𔽉f����UI.Image")]
        [SerializeField] private Image parentGauge;
        [SerializeField] private Image childGauge;

        private float maxHeight;
        private int maxParameter;

        private void Start()
        {
            //Star�ŌĂяo���ƃV�[���J�ږ��ɏ����������S�z����
            InitializeParamater();




            if (valueManagement == null)
            {
                valueManagement = GetComponent<ValueManagement>();
                maxParameter = valueManagement.MaxParameter;
            }

            // �e�Q�[�W��sizeDelta����ő�̍������擾����
            // rect.height����sizeDelta�̕������肵�Ă��邱�Ƃ������ł�
            if (parentGauge != null)
            {
                maxHeight = parentGauge.rectTransform.sizeDelta.y;
            }
            else
            {
                Debug.LogError("ParentGauge���ݒ肳��Ă��܂���I");
                return;
            }
            maxParameter = valueManagement.MaxParameter;

            // �e�Q�[�W�̍������X�V
            ChangeGauge(valueManagement.ParentParameter, parentGauge);
            ChangeGauge(valueManagement.ChildParameter, childGauge);
        }

        private void Update()
        {
            //�f�o�b�O�p
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
        /// l̏
        /// </summary>
        public void InitializeParamater()
        {
            Debug.LogWarning("p[^܂");
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