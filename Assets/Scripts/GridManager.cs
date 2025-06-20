using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [Header("�O���b�h�ݒ�")]
    public int width = 4;
    public int height = 4;
    public float cellSize = 1.0f;
    public Vector3 originPosition = Vector3.zero;

    private Transform[,] grid;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        grid = new Transform[width, height];
    }

    // ���[���h���W -> �O���b�h���W
    public Vector2Int WorldToGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.y - originPosition.y) / cellSize);
        return new Vector2Int(x, y);
    }

    // �O���b�h���W -> ���[���h���W�i�Z���̒��S��Ԃ��悤�ɏC���j
    public Vector3 GridToWorldPosition(int x, int y)
    {
        float worldX = originPosition.x + (x * cellSize) + (cellSize * 0.5f);
        float worldY = originPosition.y + (y * cellSize) + (cellSize * 0.5f);

        worldY -= 0.5f;

        return new Vector3(worldX, worldY, 0);
    }

    // �s�[�X�̔z�u�ۂ�`�F�b�N
    public bool CanPlacePiece(PieceController piece, Vector2Int gridPos)
    {
        foreach (Transform block in piece.transform)
        {
            Vector3 worldOffset = piece.transform.TransformDirection(block.localPosition);
            int gridOffsetX = Mathf.RoundToInt(worldOffset.x / cellSize);
            int gridOffsetY = Mathf.RoundToInt(worldOffset.y / cellSize);
            Vector2Int blockOffset = new Vector2Int(gridOffsetX, gridOffsetY);

            Vector2Int checkPos = gridPos + blockOffset;

            if (checkPos.x < 0 || checkPos.x >= width || checkPos.y < 0 || checkPos.y >= height)
            {
                return false;
            }

            if (grid[checkPos.x, checkPos.y] != null && grid[checkPos.x, checkPos.y] != piece.transform)
            // grid�ɂ�block���g������悤�ɂ���
            if (grid[checkPos.x, checkPos.y] != null && grid[checkPos.x, checkPos.y] != block)
            {
                return false;
            }
        }
        return true;
    }

    // �s�[�X��O���b�h�ɓo�^
    public void RegisterPiece(PieceController piece, Vector2Int gridPos)
    {
        if (!CanPlacePiece(piece, gridPos))
        {
            Debug.LogWarning("���̈ʒu�ɂ̓s�[�X��z�u�ł��܂���B");
            return;
        }

        UnregisterPiece(piece);

        foreach (Transform block in piece.transform)
        {
            Vector3 worldOffset = piece.transform.TransformDirection(block.localPosition);
            int gridOffsetX = Mathf.RoundToInt(worldOffset.x / cellSize);
            int gridOffsetY = Mathf.RoundToInt(worldOffset.y / cellSize);
            Vector2Int blockOffset = new Vector2Int(gridOffsetX, gridOffsetY);

            Vector2Int registerPos = gridPos + blockOffset;

            if (registerPos.x >= 0 && registerPos.x < width && registerPos.y >= 0 && registerPos.y < height)
            {
                grid[registerPos.x, registerPos.y] = piece.transform;
                grid[registerPos.x, registerPos.y] = block; // block���g��o�^
            }
        }
    }

    // �s�[�X�̓o�^����
    public void UnregisterPiece(PieceController piece)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y] == piece.transform)
                // block�������̎q���ǂ����ŉ��
                if (grid[x, y] != null && grid[x, y].parent == piece.transform)
                {
                    grid[x, y] = null;
                }
            }
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray; // ���₷���悤�ɃO���[�ɕύX
        Vector3 offset = new Vector3(0, -0.5f, 0); // ���v�]�̃I�t�Z�b�g��Gizmo�ɂ�K�p

        // �c��
        for (int i = 0; i < width + 1; i++)
        {
            Vector3 startPos = originPosition + new Vector3(i * cellSize, 0, 0) + offset;
            Vector3 endPos = originPosition + new Vector3(i * cellSize, height * cellSize, 0) + offset;
            Gizmos.DrawLine(startPos, endPos);
        }
        // ����
        for (int i = 0; i < height + 1; i++)
        {
            Vector3 startPos = originPosition + new Vector3(0, i * cellSize, 0) + offset;
            Vector3 endPos = originPosition + new Vector3(width * cellSize, i * cellSize, 0) + offset;
            Gizmos.DrawLine(startPos, endPos);
        }
    }
}