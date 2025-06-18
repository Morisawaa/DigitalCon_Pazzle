using UnityEngine;
using TMPro; // TextMeshProを使うために必要！

public class Piece : MonoBehaviour
{
    public ShapeData shapeData;
    public GameObject cellPrefab; // ブロック1つ分のプレハブ

    [Header("テキスト設定")]
    public TextMeshPro textPrefab; // TextMeshProのプレハブをここに設定

    void Start()
    {
        GenerateCells();
        GenerateTexts(); // テキストを生成する処理を呼び出す
    }

    // ブロックのセルを生成する処理
    void GenerateCells()
    {
        if (shapeData == null) return;

        foreach (Vector2Int cellPosition in shapeData.Cells)
        {
            Vector3 worldPosition = new Vector3(cellPosition.x, cellPosition.y, 0);
            Instantiate(cellPrefab, transform.position + worldPosition, Quaternion.identity, this.transform);
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
            char character = shapeData.BlockChar[i];
            Vector2Int textGridPos = shapeData.TextPos[i];

            // テキストのワールド座標を計算
            // ブロックより少し手前(Z値を小さく)に表示すると、重なった時に見やすい
            Vector3 textWorldPos = new Vector3(textGridPos.x, textGridPos.y, -0.1f);

            // プレハブから新しいテキストオブジェクトを生成し、このPieceの子にする
            TextMeshPro newText = Instantiate(textPrefab, this.transform);
            newText.transform.localPosition = textWorldPos;

            // テキストの内容とサイズをShapeDataから設定
            newText.text = character.ToString();
            newText.fontSize = shapeData.TextSize;
        }
    }
}