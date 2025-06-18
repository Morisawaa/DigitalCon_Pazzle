using UnityEngine;
using UnityEngine.UI;

public class ParameterGauge : MonoBehaviour
{
    [SerializeField] private DebugParameter DebugParameter_;
    [SerializeField] private Image ParentGauge;
    [SerializeField] private Image ChildGauge;

    private float MaxHeight;
    // �p�����[�^�̍ő�l���C���X�y�N�^�[����ݒ�ł���悤�ɂ���
    private int MaxParameter = 10;

    private void Start()
    {
        //���݂�DebugParameter����A�e�q�̃p�����[�^�[�Ƃ��̏���l���Q�Ƃ��Ă���
        //�K�v�ȏꍇ���̎Q�Ɛ��ύX

        if (DebugParameter_ == null)
        {
            DebugParameter_ = GetComponent<DebugParameter>();
        }

        // �e�Q�[�W��sizeDelta����ő�̍������擾����
        // rect.height����sizeDelta�̕������肵�Ă��邱�Ƃ������ł�
        if (ParentGauge != null)
        {
            MaxHeight = ParentGauge.rectTransform.sizeDelta.y;
        }
        else
        {
            Debug.LogError("ParentGauge���ݒ肳��Ă��܂���I");
            return;
        }
        MaxParameter = DebugParameter_.MaxParameter;

        // �e�Q�[�W�̍������X�V
        ChangeGauge(DebugParameter_.ParentParameter, ParentGauge);
        ChangeGauge(DebugParameter_.ChildParameter, ChildGauge);
    }

    private void Update()
    {
        //�f�o�b�O�p
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
        // �y�C���_1�zNull�`�F�b�N�̏������C��
        if (TargetImage == null)
        {
            // �G���[���b�Z�[�W�̃X�y�����C��
            Debug.LogError("TargetImage��Null�ł�");
            return;
        }

        RectTransform rectTransform = TargetImage.rectTransform;
        Vector2 size = rectTransform.sizeDelta;

        // �y�C���_2�z�����̌v�Z���@���C��
        // �p�����[�^��0�����ɂȂ�Ȃ��悤��Clamp�i�����j����
        float currentParameter = Mathf.Clamp(Parameter, 0, MaxParameter);

        // (���ݒl / �ő�l) �̊������v�Z
        float ratio = currentParameter / (float)MaxParameter;

        // �����ɉ����č������v�Z
        float newHeight = MaxHeight * ratio;

        // �v�Z����������Vector2��y�ɐݒ�
        size.y = newHeight;

        // �y�C���_3�z�ύX����size��rectTransform�ɍĐݒ肷��
        rectTransform.sizeDelta = size;
    }
}