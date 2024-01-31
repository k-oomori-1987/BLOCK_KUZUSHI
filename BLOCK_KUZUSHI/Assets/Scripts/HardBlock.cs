using UnityEngine;

[DisallowMultipleComponent]

public class HardBlock : Block
{
    [SerializeField] private Material block;        // ブロック（通常）のマテリアル
    [SerializeField] private Material hardblock;    // ブロック（ハード）のマテリアル

    private MeshRenderer meshrender;

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// スタート
    /// </summary>
    void Start()
    {
        // マテリアル情報取得
        meshrender = GetComponent<MeshRenderer>();
    }

    /// <summary>
    /// ブロックに衝突したとき（オーバーライド）
    /// </summary>
    /// <param name="collision">衝突したオブジェクト</param>
    protected override void OnCollisionEnter(Collision collision)
    {
        // ブロックの可否破壊チェック
        if (DestroyBlock(collision) == false)
        {
            // 破壊されていない場合
            if (blocklife <= 1)
            {
                // 残りライフ1の場合、通常ブロックのマテリアルへ変更
                meshrender.material = block;
            }
            else
            {
                // 残りライフ2以上の場合、ハードブロックのマテリアルへ変更
                meshrender.material = hardblock;
            }
        }
    }

    #endregion

}
