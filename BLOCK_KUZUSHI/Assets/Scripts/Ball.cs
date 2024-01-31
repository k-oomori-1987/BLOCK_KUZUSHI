using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]

public class Ball : MonoBehaviour
{

    public static Ball instance;

    [SerializeField] public float speed = 3.0f;             // スピード
    [SerializeField] private Rigidbody myRigid;             // RigidBody（最初に力を加える用）
    [SerializeField] private AudioSource ballsound;         // ボールが壁等に当たったときのサウンド
    [SerializeField] private AudioSource metalballsound;    // ボール（金属）が当たったときのサウンド
    [SerializeField] private AudioSource blockSound;        // ボールがブロックに当たったときのサウンド
    [SerializeField] private AudioSource waterSound;        // ボールが水トラップに入ったときのサウンド

    [SerializeField] private float biggersize = 3.0f;       // 巨大化したときの大きさ
    [SerializeField] private Material metalmaterial;        // 金属っぽいマテリアル

    [SerializeField] private Camera myCamera;               // メインカメラオブジェクト
    [SerializeField] public float magnitude = 3.0f;         // カメラの揺れ幅
    [SerializeField] public float backtime = 1.0f;          // カメラが揺れたあと戻るまでの時間

    private Vector3 mainPos;            // メインカメラの初期位置
    public Item.ItemType balltype;      // 現在のボールタイプ

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
        // ボールタイプを初期化
        balltype =  Item.ItemType.Nothing;

        // カメラの初期位置取得
        mainPos = myCamera.transform.position;

    }

    /// <summary>
    /// ボールオブジェクト衝突時
    /// </summary>
    /// <param name="collision">衝突してきたオブジェクト</param>
    private void OnCollisionEnter(Collision collision)
    {
        // ゲームクリア or ゲームオーバー時は処理しない
        if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
        {
            // ボールの速度を一定に保つ（ブロック等に当たると徐々にボールスピードが落ちるため、一定に保つように制御）
            KeepBallSpeed();

            if (collision.gameObject.tag == "Block")
            {
                // ブロックに当たったときの音
                if (blockSound != null)
                {
                    // 音を鳴らす
                    blockSound.PlayOneShot(blockSound.clip);
                }

            }
            else
            {
                // ボール（金属）の場合
                if (balltype == Item.ItemType.BallMetal)
                {
                    if (metalballsound != null)
                    {
                        // 金属音を鳴らす
                        metalballsound.PlayOneShot(metalballsound.clip);
                    }
                }
                else
                {
                    if (ballsound != null)
                    {
                        // ボールが壁等に当たる音を鳴らす
                        ballsound.PlayOneShot(ballsound.clip);
                    }
                }
            }
        }
    }

    /// <summary>
    /// ボールオブジェクトに入った時（大玉で衝突した時）
    /// </summary>
    /// <param name="other">入ってきたオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        // ゲームクリア or ゲームオーバー時は処理しない
        if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
        {
            if (balltype == Item.ItemType.BallBigger)
            {
                if (other.CompareTag("Block") == true)
                {
                    // ブロックに当たったときの音
                    if (blockSound != null)
                    {
                        // 音を鳴らす
                        blockSound.PlayOneShot(blockSound.clip);
                    }

                    // 画面を揺らす
                    Shake();
                }
                else
                {
                    if (ballsound != null)
                    {
                        // ボールが壁等に当たる音を鳴らす
                        ballsound.PlayOneShot(ballsound.clip);
                    }
                }
            }
        }
    }

    #endregion

    #region ◆◆◆　　アイテムによる変更　　◆◆◆

    /// <summary>
    /// ボールスピードアップ
    /// </summary>
    public void BallSpeedUp()
    {
        // ゲームクリア or ゲームオーバー時は処理しない
        if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
        {
            speed *= 2;
            myRigid.velocity *= 2;
        }
    }

    /// <summary>
    /// ボール巨大化（衝突 ⇒ 当たり判定）
    /// </summary>
    public void BallBigger()
    {
        // ゲームクリア or ゲームオーバー時は処理しない
        if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
        {
            this.transform.localScale *= biggersize;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z + 1); //巨大化したことにより位置を調整

            // ボールタイプを設定
            balltype = Item.ItemType.BallBigger;
        }
    }

    /// <summary>
    /// ボール金属化
    /// </summary>
    public void BallMetal()
    {
        // マテリアルを変更
        this.GetComponent<MeshRenderer>().material = metalmaterial;

        // ボールタイプを設定
        balltype = Item.ItemType.BallMetal;
    }

    #endregion

    #region ◆◆◆　　ボールスピード一定化　　◆◆◆

    /// <summary>
    ///  ボールスピード一定化
    /// </summary>
    private void KeepBallSpeed()
    {
        // 現在の速度を取得
        Vector3 currentSpeed = myRigid.velocity;

        // 横向きしか動かなくなる場合、下向きに力を少し加える
        if (Mathf.Abs(currentSpeed.z) < 0.2)
        {
            currentSpeed.z = -0.2f;
        }

        // ボールの速度を一旦１に戻す
        currentSpeed.Normalize();

        // 指定した速度に設定
        currentSpeed *= speed;
        myRigid.velocity = currentSpeed;
    }

    #endregion

    #region ◆◆◆　　ボール巨大化によるカメラの揺れ制御　　◆◆◆

    /// <summary>
    /// カメラの揺れ制御
    /// </summary>
    public void Shake()
    {
        // コルーチン開始
        StartCoroutine(DoShake());
    }

    /// <summary>
    /// カメラの揺れ処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoShake()
    {
        var pos = mainPos;

        // x軸、z軸に対して揺らす
        var x = Random.Range(-1.0f, 1.0f) * magnitude;
        var z = Random.Range(-1.0f, 1.0f) * magnitude;

        // 少しずつ戻る距離を算出
        float distantX = x * Time.deltaTime / backtime;
        float distantZ = z * Time.deltaTime / backtime;

        while (true)
        {
            // カメラを移動
            myCamera.transform.localPosition = new Vector3(pos.x - x, pos.y, pos.z - z);

            // 少しずつ初期位置に戻す
            x -= distantX;
            z -= distantZ;

            yield return null;

            // 元の位置に戻った場合、処理を抜ける
            if (Mathf.Sign(x) != Mathf.Sign(distantX) && Mathf.Sign(z) != Mathf.Sign(distantZ))
            {
                break;
            }
        }

        // 元の位置に完全に戻す（小数点以下など細かな誤差が残るため）
        myCamera.transform.localPosition = mainPos;

        yield return null;
    }

    #endregion

    #region ◆◆◆　　ボールスタート処理　　◆◆◆

    /// <summary>
    /// スタート時のボール発射処理
    /// </summary>
    public void BallStart()
    {
        myRigid = this.GetComponent<Rigidbody>();
        myRigid.AddForce(new Vector3(0, 0, -1) * speed, ForceMode.VelocityChange);    // 下方向に力を加える
    }

    #endregion

}
