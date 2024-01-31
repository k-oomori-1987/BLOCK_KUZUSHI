using UnityEngine;

[DisallowMultipleComponent]

public class Player : MonoBehaviour
{
    [SerializeField] public float speed = 10.0f;        // プレイヤーの動くスピード
    [SerializeField] public float range = 3.9f;         // 壁からプレイヤーまでの距離
    [SerializeField] private AudioSource itemSound;     // アイテム取得時のサウンド
    [SerializeField] private AudioSource waterSound;    // 水トラップのサウンド
    [SerializeField] private GameObject ball;           // ボールオブジェクト

    private Vector3 playersize;                 // プレイヤーの大きさ（初期設定）
    private float playerSpeed;                  // プレイヤーの動くスピード（初期設定）

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// スタート
    /// </summary>
    void Start()
    {
        // 現在の大きさを取得
        playersize = this.transform.localScale;

        // プレイヤーのスピードを保持
        playerSpeed = speed;
    }

    /// <summary>
    /// 随時処理（プレイヤーの移動処理）
    /// </summary>
    void Update()
    {
        // ゲームクリア or ゲームオーバー時は移動できない
        if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
        {
            // ← キー押下時
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                // 左側の壁を超えない位置の場合
                if(this.transform.position.x > -1 * range)
                {
                    this.transform.position += speed * Time.deltaTime * Vector3.left;
                }
            }

            // → キー押下時
            if (Input.GetKey(KeyCode.RightArrow))
            {
                // 右側の壁を超えない位置の場合
                if (this.transform.position.x < range)
                {
                    this.transform.position += speed * Time.deltaTime * Vector3.right;
                }
            }
        }

    }

    /// <summary>
    /// プレイヤーオブジェクトに入ってきた時
    /// </summary>
    /// <param name="other">入ってきたオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        // アイテムを取得した場合
        if (other.CompareTag("Item") == true)
        {
            // アイテム取得時のサウンドを鳴らす
            if (itemSound != null)
            {
                itemSound.Play();
            }

            // 取得したアイテムの種類によって効果を分岐
            switch (other.gameObject.GetComponent<Item>().itemtype)
            {
                case Item.ItemType.PlayerExtention:
                    // プレイヤー延長（X軸に2倍）
                    this.transform.localScale = new Vector3(playersize.x * 2, playersize.y, playersize.z);
                    range = 2.85f;
                    break;

                case Item.ItemType.BallMetal:
                    // ボール（金属化）
                    Ball.instance.BallMetal();
                    break;

                case Item.ItemType.BallSpeedUp:
                    // ボールスピードアップ
                    Ball.instance.BallSpeedUp();
                    break;

                case Item.ItemType.BallBigger:
                    // ボール巨大化
                    Ball.instance.BallBigger();
                    GameClear.instance.ChangeAtariHantei();
                    break;

                case Item.ItemType.TrapDestroy:
                    // 水トラップ破壊

                    // アイテムの効果はItem.cs側で処理する

                    // スピード、ＢＧＭを元に戻す（水トラップ内でアイテムを取得した場合の対応）
                    ReturnSpeedBGM();

                    break;
            }

            // アイテムを消す
            Destroy(other.gameObject);

        }
        // 水トラップに進入した場合
        else if(other.CompareTag("Trap") == true)
        {
            if (waterSound != null)
            {
                // 水トラップのサウンドを鳴らす
                waterSound.Play();

                // ゲームクリア or ゲームオーバー時は遅くしない
                if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
                {
                    // プレイヤーのスピードを遅くする
                    speed = playerSpeed / 3;

                    // BGMをゆっくりにする
                    BGM.instance.SpeedDownBGM();
                }
            }
        }
    }

    /// <summary>
    /// プレイヤーオブジェクトから出た時
    /// </summary>
    /// <param name="other">出たオブジェクト</param>
    private void OnTriggerExit(Collider other)
    {
        // 水トラップの場合
        if (other.CompareTag("Trap") == true)
        {
            // スピード、BGMを戻す
            ReturnSpeedBGM();
        }
    }

    #endregion

    #region ◆◆◆　　水トラップから出た場合の処理　　◆◆◆

    /// <summary>
    /// 水トラップから出た場合の処理
    /// </summary>
    private void ReturnSpeedBGM()
    {
        // スピードを元に戻す
        speed = playerSpeed;

        // BGMを元に戻す
        BGM.instance.SpeedNormalBGM();
    }

    #endregion
}
