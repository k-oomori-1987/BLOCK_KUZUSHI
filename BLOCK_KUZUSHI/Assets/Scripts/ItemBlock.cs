using UnityEngine;

[DisallowMultipleComponent]

public class ItemBlock : Block
{
    [SerializeField] private GameObject item;       // アイテムオブジェクト
    [SerializeField] public float speed = 3.0f;     // アイテムを落とすスピード

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// スタート
    /// </summary>
    void Start()
    {
        // アイテムを非表示にする
        item.SetActive(false);
    }

    /// <summary>
    /// ブロックに衝突したとき（オーバーライド）
    /// </summary>
    /// <param name="collision">衝突したオブジェクト</param>
    protected override void OnCollisionEnter(Collision collision)
    {
        // ブロックの可否破壊チェック
        if (DestroyBlock(collision) == true)
        {
            // ゲームクリア時でない場合
            if (GameClear.instance.CheckGameClear() == false)
            {
                // アイテムを表示
                item.SetActive(true);

                // アイテムを下に落とす
                Rigidbody itemRigid = item.GetComponent<Rigidbody>();
                itemRigid.AddForce(new Vector3(0, 0, -1) * speed, ForceMode.VelocityChange);    // 下方向に力を加える
            }
        }
    }

    #endregion

}
