using UnityEngine;

public class PieceController : MonoBehaviour
{
    private Vector3 offset; // �}�E�X�J�[�\���ƃI�u�W�F�N�g���S�̍���
    private Vector3 initialPosition; // �h���b�O�J�n���̈ʒu
    private Vector3 initialScale; // �h���b�O�J�n���̃X�P�[��

    [HideInInspector]
    public bool isPlaced = false; // �O���b�h�ɔz�u�ς݂�

    // �h���b�O�J�n��
    private void OnMouseDown()
    {
        GridManager.Instance.UnregisterPiece(this);
        // ...���̏���
    }

    // �h���b�O�I�����i�h���b�v���j
    private void OnMouseUp()
    {
        Vector3 worldPos = transform.position;
        Vector2Int gridPos = GridManager.Instance.WorldToGridPosition(worldPos);

        if (GridManager.Instance.CanPlacePiece(this, gridPos))
        {
            GridManager.Instance.RegisterPiece(this, gridPos);
            isPlaced = true;
            // �K�v�Ȃ�transform.position��GridToWorldPosition�ŃX�i�b�v
            transform.position = GridManager.Instance.GridToWorldPosition(gridPos.x, gridPos.y);
        }
        else
        {
            // �z�u�ł��Ȃ��ꍇ�̏����i���̈ʒu�ɖ߂����j
            isPlaced = false;
        }
    }

    // �h���b�O��
    private void OnMouseDrag()
    {
        // �}�E�X�̓����ɍ��킹�ăs�[�X��ړ�
        transform.position = GetMouseWorldPos() + offset;
    }

    // �}�E�X�𗣂�����̏����i�d�����Ă���OnMouseUp��폜���A������OnMouseUp�ɓ����j
    private void HandleMouseRelease()
    {
        // �ł�߂��O���b�h�̒��S���W��v�Z
        Vector2Int gridPos = GridManager.Instance.WorldToGridPosition(transform.position);

        // ���̏ꏊ�ɔz�u�\���`�F�b�N
        if (GridManager.Instance.CanPlacePiece(this, gridPos))
        {
            // --- �z�u���� ---
            // �O���b�h�ɃX�i�b�v������
            transform.position = GridManager.Instance.GridToWorldPosition(gridPos.x, gridPos.y);
            // �O���b�h�}�l�[�W���[�Ƀs�[�X��o�^����
            GridManager.Instance.RegisterPiece(this, gridPos);
            isPlaced = true;
        }
        else
        {
            // --- �z�u���s ---
            // ���̈ʒu�ɖ߂�
            transform.position = initialPosition;
            // ����h���b�O�J�n���ɔz�u�ς݂������Ȃ�A�ēo�^����
            if (GridManager.Instance.CanPlacePiece(this, GridManager.Instance.WorldToGridPosition(initialPosition)))
            {
                GridManager.Instance.RegisterPiece(this, GridManager.Instance.WorldToGridPosition(initialPosition));
                isPlaced = true;
            }
        }

        // Z���W����ɖ߂�
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        // �Q�[���N���A�`�F�b�N��Ăяo��
        GameController.Instance.CheckGameCompletion();
    }

    // �}�E�X�J�[�\���̈ʒu����[���h���W�Ŏ擾����w���p�[�֐�
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
