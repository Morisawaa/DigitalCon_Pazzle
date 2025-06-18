using UnityEngine;
using TMPro; // TextMeshPro���g�����߂ɕK�v�I
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    public ShapeData shapeData;
    public GameObject cellPrefab; // �u���b�N1���̃v���n�u

    [Header("�Z���̃T�C�Y")] public Vector2 CellSize = new Vector2(1.0f, 1.0f);
    [Header("�e�L�X�g�ݒ�")]
    public TextMeshPro textPrefab; // TextMeshPro�̃v���n�u�������ɐݒ�

    // ���݂̃s�[�X�̏�Ԃ�ێ����郊�X�g
    private List<Vector2Int> currentCellPositions;
    private List<Vector2Int> currentTextPositions;

    void Start()
    {
        // ShapeData���猻�݂̏�Ԃփf�[�^���R�s�[���ď�����
        currentCellPositions = new List<Vector2Int>(shapeData.Cells);
        currentTextPositions = new List<Vector2Int>(shapeData.TextPos);


        GenerateCells();
        GenerateTexts(); // �e�L�X�g�𐶐����鏈�����Ăяo��
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Rotate(false);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Rotate(true);
        }
    }


    // �u���b�N�̃Z���𐶐����鏈��
    void GenerateCells()
    {
        if (shapeData == null) return;

        foreach (Vector2Int cellPosition in currentCellPositions)
        {
            Vector3 worldPosition = new Vector3(
                cellPosition.x * CellSize.x,
                cellPosition.y * CellSize.y,
                0
                );
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
            string character = shapeData.BlockChar[i];
            Vector2Int textGridPos = currentTextPositions[i];

            // �e�L�X�g�̃��[���h���W���v�Z
            // �u���b�N��菭����O(Z�l��������)�ɕ\������ƁA�d�Ȃ������Ɍ��₷��
            Vector3 textWorldPos = new Vector3(
                textGridPos.x * CellSize.x,
                textGridPos.y * CellSize.y,
                -0.1f // Z�l�͂��̂܂�
            );
            // �v���n�u����V�����e�L�X�g�I�u�W�F�N�g�𐶐����A����Piece�̎q�ɂ���
            TextMeshPro newText = Instantiate(textPrefab, this.transform);
            newText.transform.localPosition = textWorldPos;

            // �e�L�X�g�̓��e�ƃT�C�Y��ShapeData����ݒ�
            newText.text = character.ToString();
            newText.fontSize = shapeData.TextSize;
        }
    }

    // ��]�����̖{��
    private void Rotate(bool clockwise)
    {
        // 1. �e���W���X�g����]������
        // �Z���̉�]
        for (int i = 0; i < currentCellPositions.Count; i++)
        {
            Vector2Int pos = currentCellPositions[i];
            currentCellPositions[i] = clockwise ? new Vector2Int(pos.y, -pos.x) : new Vector2Int(-pos.y, pos.x);
        }

        // �e�L�X�g�ʒu�̉�]
        for (int i = 0; i < currentTextPositions.Count; i++)
        {
            Vector2Int pos = currentTextPositions[i];
            currentTextPositions[i] = clockwise ? new Vector2Int(pos.y, -pos.x) : new Vector2Int(-pos.y, pos.x);
        }

        // 2. �ĕ`�悷��
        RedrawPiece();
    }

    // �s�[�X���ĕ`�悷�鏈��
    private void RedrawPiece()
    {
        // �����̃Z���ƃe�L�X�g�����ׂč폜
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // �V�������W�ōĐ���
        GenerateCells();
        GenerateTexts();
    }
}