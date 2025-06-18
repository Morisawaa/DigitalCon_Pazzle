using UnityEngine;
using TMPro; // TextMeshProを使うために必要！
using System.Collections.Generic;

public class Piece : MonoBehaviour
{
    public ShapeData shapeData;
    public GameObject cellPrefab; // ブロック1つ分のプレハブ

    [Header("セルのサイズ")] public Vector2 CellSize = new Vector2(1.0f, 1.0f);
    [Header("テキスト設定")]
    public TextMeshPro textPrefab; // TextMeshProのプレハブをここに設定

    // 現在のピースの状態を保持するリスト
    private List<Vector2Int> currentCellPositions;
    private List<Vector2Int> currentTextPositions;

    void Start()
    {
        // ShapeDataから現在の状態へデータをコピーして初期化
        currentCellPositions = new List<Vector2Int>(shapeData.Cells);
        currentTextPositions = new List<Vector2Int>(shapeData.TextPos);


        GenerateCells();
        GenerateTexts(); // テキストを生成する処理を呼び出す
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


    // ブロックのセルを生成する処理
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

    // 文字を生成する処理
    void GenerateTexts()
    {
        if (shapeData == null || textPrefab == null) return;

        // 安全対策：文字リストと座標リストの数が合わない場合はエラーを出して処理を中断
        if (shapeData.BlockChar.Count != shapeData.TextPos.Count)
        {
            Debug.LogError("ShapeData内の文字リスト(BlockChar)と座標リスト(TextPos)の数が一致しません！");
            return;
        }

        // forループで、インデックス(i)を使いながら両方のリストにアクセスする
        for (int i = 0; i < shapeData.BlockChar.Count; i++)
        {
            string character = shapeData.BlockChar[i];
            Vector2Int textGridPos = currentTextPositions[i];

            // テキストのワールド座標を計算
            // ブロックより少し手前(Z値を小さく)に表示すると、重なった時に見やすい
            Vector3 textWorldPos = new Vector3(
                textGridPos.x * CellSize.x,
                textGridPos.y * CellSize.y,
                -0.1f // Z値はそのまま
            );
            // プレハブから新しいテキストオブジェクトを生成し、このPieceの子にする
            TextMeshPro newText = Instantiate(textPrefab, this.transform);
            newText.transform.localPosition = textWorldPos;

            // テキストの内容とサイズをShapeDataから設定
            newText.text = character.ToString();
            newText.fontSize = shapeData.TextSize;
        }
    }

    // 回転処理の本体
    private void Rotate(bool clockwise)
    {
        // 1. 各座標リストを回転させる
        // セルの回転
        for (int i = 0; i < currentCellPositions.Count; i++)
        {
            Vector2Int pos = currentCellPositions[i];
            currentCellPositions[i] = clockwise ? new Vector2Int(pos.y, -pos.x) : new Vector2Int(-pos.y, pos.x);
        }

        // テキスト位置の回転
        for (int i = 0; i < currentTextPositions.Count; i++)
        {
            Vector2Int pos = currentTextPositions[i];
            currentTextPositions[i] = clockwise ? new Vector2Int(pos.y, -pos.x) : new Vector2Int(-pos.y, pos.x);
        }

        // 2. 再描画する
        RedrawPiece();
    }

    // ピースを再描画する処理
    private void RedrawPiece()
    {
        // 既存のセルとテキストをすべて削除
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // 新しい座標で再生成
        GenerateCells();
        GenerateTexts();
    }
}