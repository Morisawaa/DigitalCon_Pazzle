using UnityEngine;
using TMPro; // TextMeshPro���g�����߂ɕK�v�I

public class Piece : MonoBehaviour
{
    public ShapeData shapeData;
    public GameObject cellPrefab; // �u���b�N1���̃v���n�u

    [Header("�e�L�X�g�ݒ�")]
    public TextMeshPro textPrefab; // TextMeshPro�̃v���n�u�������ɐݒ�

    void Start()
    {
        GenerateCells();
        GenerateTexts(); // �e�L�X�g�𐶐����鏈�����Ăяo��
    }

    // �u���b�N�̃Z���𐶐����鏈��
    void GenerateCells()
    {
        if (shapeData == null) return;

        foreach (Vector2Int cellPosition in shapeData.Cells)
        {
            Vector3 worldPosition = new Vector3(cellPosition.x, cellPosition.y, 0);
            Instantiate(cellPrefab, transform.position + worldPosition, Quaternion.identity, this.transform);
        }
    }

    // �����𐶐����鏈��
    void GenerateTexts()
    {
        if (shapeData == null || textPrefab == null) return;

        // ���S�΍�F�������X�g�ƍ��W���X�g�̐�������Ȃ��ꍇ�̓G���[���o���ď����𒆒f
        if (shapeData.BlockChar.Count != shapeData.TextPos.Count)
        {
            Debug.LogError("ShapeData���̕������X�g(BlockChar)�ƍ��W���X�g(TextPos)�̐�����v���܂���I");
            return;
        }

        // for���[�v�ŁA�C���f�b�N�X(i)���g���Ȃ��痼���̃��X�g�ɃA�N�Z�X����
        for (int i = 0; i < shapeData.BlockChar.Count; i++)
        {
            char character = shapeData.BlockChar[i];
            Vector2Int textGridPos = shapeData.TextPos[i];

            // �e�L�X�g�̃��[���h���W���v�Z
            // �u���b�N��菭����O(Z�l��������)�ɕ\������ƁA�d�Ȃ������Ɍ��₷��
            Vector3 textWorldPos = new Vector3(textGridPos.x, textGridPos.y, -0.1f);

            // �v���n�u����V�����e�L�X�g�I�u�W�F�N�g�𐶐����A����Piece�̎q�ɂ���
            TextMeshPro newText = Instantiate(textPrefab, this.transform);
            newText.transform.localPosition = textWorldPos;

            // �e�L�X�g�̓��e�ƃT�C�Y��ShapeData����ݒ�
            newText.text = character.ToString();
            newText.fontSize = shapeData.TextSize;
        }
    }
}