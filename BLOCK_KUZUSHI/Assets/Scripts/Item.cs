using Unity.VisualScripting;
using UnityEngine;

[DisallowMultipleComponent]

public class Item : MonoBehaviour
{
    public static Item instance;

    [SerializeField] private Material playerextentionColor;
    [SerializeField] private Material ballmetalColor;
    [SerializeField] private Material ballspeedupColor;
    [SerializeField] private Material ballbiggerColor;
    [SerializeField] private Material trapdestroyColor;

    [SerializeField] private GameObject watertrap;        // 水トラップのゲームオブジェクト
    [SerializeField] private GameObject destroyeffect;    // 水泡のエフェクト
    [SerializeField] private AudioClip destroywaterSound; // 水泡のサウンド

    public enum ItemType
    {
        Nothing,            // 何も無し
        PlayerExtention,    // プレイヤー延長
        BallMetal,          // ボール（金属化）
        BallSpeedUp,        // ボールスピードアップ
        BallBigger,         // ボール巨大化
        TrapDestroy,        // 水トラップを破壊
    }
    [SerializeField] public ItemType itemtype;

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
        //アイテムの種類によってマテリアルを変更
        switch (itemtype)
        {
            case ItemType.PlayerExtention:
                // プレイヤー延長
                this.GetComponent<MeshRenderer>().material = playerextentionColor;
                break;
            case ItemType.BallMetal:
                // ボール（金属化）
                this.GetComponent<MeshRenderer>().material = ballmetalColor;
                break;
            case ItemType.BallSpeedUp:
                // ボールスピードアップ
                this.GetComponent<MeshRenderer>().material = ballspeedupColor;
                break;
            case ItemType.BallBigger:
                // ボール巨大化
                this.GetComponent<MeshRenderer>().material = ballbiggerColor;
                break;
            case ItemType.TrapDestroy:
                // 水トラップを破壊
                this.GetComponent<MeshRenderer>().material = trapdestroyColor;
                break;
        }

    }

    /// <summary>
    /// アイテムオブジェクトに入ってきた時
    /// </summary>
    /// <param name="other">入ってきたオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        // ゲームクリア or ゲームオーバー以外
        if (GameClear.instance.gameclearFlg == false && GameOver.instance.gameoverFlg == false)
        {
            // アイテムが水トラップの場合
            if (itemtype == ItemType.TrapDestroy)
            {
                // 入ってきたオブジェクトがプレーヤーの場合
                if (other.gameObject.CompareTag("Player") == true)
                {
                    if (watertrap != null)
                    {
                        // 対応する水トラップを破壊
                        Destroy(watertrap);

                        // 水泡のエフェクト表示
                        GameObject effect = Instantiate(destroyeffect, watertrap.transform.position, Quaternion.identity);
                        Destroy(effect, 2.0f);

                        // トラップ破壊の音を鳴らす
                        BGM.instance.PlayAudioClip(destroywaterSound);
                    }
                }
            }
        }
    }

    #endregion
}
