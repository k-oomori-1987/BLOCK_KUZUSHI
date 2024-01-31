using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]

public class Block : MonoBehaviour
{
    public static Block instance;

    [SerializeField] public short blocklife;    // ブロックのライフ

    private GameObject parentobject;            // 親オブジェクト

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// スタート
    /// </summary>
    void Start()
    {
        // ブロックの親オブジェクト（GameClearオブジェクト）を取得
        parentobject = transform.parent.gameObject;
    }

    /// <summary>
    /// ブロックに衝突したとき（オーバーライド可能）
    /// </summary>
    /// <param name="collision">衝突したオブジェクト</param>
    protected virtual void OnCollisionEnter(Collision collision)
    {
        // ブロック破壊処理
        DestroyBlock(collision);
    }

    /// <summary>
    /// ブロックに衝突したとき（大玉の場合）
    /// </summary>
    /// <param name="other">衝突したオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") == true)
        {
            if (Ball.instance.balltype == Item.ItemType.BallBigger)
            {
                // ブロックのライフを強制的に減らす
                blocklife = 0;

                if (blocklife <= 0)
                {
                    // ブロックを消去
                    Destroy(this.gameObject);

                    // 消去したブロック数をカウント
                    GameClear.instance.DestroyBlockCount += 1;

                    // クリア条件をチェック
                    if (GameClear.instance.CheckGameClear() == true)
                    {
                        // クリアの場合、ボールを消す
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }

    #endregion

    #region ◆◆◆　　ブロックの破壊処理　　◆◆◆

    /// <summary>
    /// ブロックの破壊処理、ブロックのライフ減、ゲームクリア処理
    /// </summary>
    /// <param name="collision">衝突したオブジェクト</param>
    /// <returns>True：ブロックを破壊, False：ブロック破壊できず</returns>
    public bool DestroyBlock(Collision collision)
    {
        // ブロックのライフを減らす
        if (Ball.instance.balltype == Item.ItemType.BallMetal)
        {
            // ボール（金属化）の場合
            blocklife -= 2;
        }
        else
        {
            // ボール（通常）の場合
            blocklife -= 1;
        }

        if (blocklife <= 0)
        {
            // ブロックを消去
            Destroy(this.gameObject);

            // 消去したブロック数をカウント
            GameClear.instance.DestroyBlockCount += 1;

            // クリア条件をチェック
            if (GameClear.instance.CheckGameClear() == true)
            {
                // クリアの場合、ボールを消す
                Destroy(collision.gameObject);
            }

            // ブロックを破壊
            return true;
        }

        // ブロックを破壊できず
        return false;
    }

    #endregion

}
