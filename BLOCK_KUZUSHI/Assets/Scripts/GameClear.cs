using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class GameClear : MonoBehaviour
{
    public static GameClear instance;

    [SerializeField] private GameObject ClearPanel;         // クリアのパネル
    [SerializeField] private GameObject GostageButton;      // 次へボタン
    [SerializeField] private AudioSource gameclearSound;    // クリア時のサウンド

    [SerializeField] private string nextSceneName;          // 次のシーン名

    [SerializeField] private GameObject rulePanel;          // 遊び方パネル
    [SerializeField] private GameObject ruleButton;         // 遊び方ボタン

    private float ClearBlockCount;      // オブジェクト内に存在するブロックの総数
    public float DestroyBlockCount;     // 破壊したブロックの総数
    public bool gameclearFlg = false;   // ゲームクリアフラグ

    public void Awake()
    {
        if(instance == null)
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
        // シーンごとに初期化
        DestroyBlockCount = 0;
        ClearBlockCount = 0;
        gameclearFlg = false;

        // クリアパネルを非表示にする 
        ClearPanel.SetActive(false);
        GostageButton.SetActive(false);

        // 配下のブロックの数を数える　→　クリアするために必要なブロック数
        foreach (Transform childobj in this.transform)
        {
            if (childobj.gameObject.tag == "Block" && childobj.gameObject.activeInHierarchy == true)
            {
                ClearBlockCount++;
            }
        }
    }

    /// <summary>
    /// 随時処理（キー入力受付）
    /// </summary>
    void Update()
    {
        // 次へボタンが表示されている場合
        if (GostageButton.activeSelf == true)
        {
            // Enter キー押下時
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // 次へボタン押下イベント
                NextButton();
            }
        }

    }

    /// <summary>
    /// 次へボタン押下
    /// </summary>
    public void NextButton()
    {
        // 次のシーンを読み込む
        SceneManager.LoadScene(nextSceneName);
    }

    #endregion

    #region ◆◆◆　　ゲームクリア関数　　◆◆◆

    /// <summary>
    /// ゲームクリア判定
    /// </summary>
    /// <returns>True：ゲームクリア, False：未クリア</returns>
    public Boolean CheckGameClear()
    {
        // クリア条件をチェック
        if (ClearBlockCount <= DestroyBlockCount)
        {
            // フラグを更新
            gameclearFlg = true;

            // BGMを止める
            BGM.instance.StopBGM();

            // クリア時のサウンドを鳴らす
            gameclearSound.Play();

            // ゲームクリアパネル（次へボタンはまだ非表示）
            ClearPanel.SetActive(true);

            // 遊び方パネル・ボタンを非表示
            rulePanel.SetActive(false);
            ruleButton.SetActive(false);

            // 遅延処理
            StartCoroutine(GameClearDerayProcess());

            return true;
        }

        return false;
    }

    /// <summary>
    /// 遅延処理
    /// </summary>
    private IEnumerator GameClearDerayProcess()
    {
        if (gameclearFlg == true)
        {
            // しばらく待つ
            yield return new WaitForSeconds(1.0f);

            // 次へボタンを表示
            GostageButton.SetActive(true);

            // コルーチンを止める
            StopCoroutine(GameClearDerayProcess());
        }
    }

    #endregion

    #region ◆◆◆　　ブロックの衝突 ⇒ 当たり判定への変更　　◆◆◆

    /// <summary>
    /// 全部ロックに対して衝突 ⇒ 当たり判定へ変更
    /// </summary>
    public void ChangeAtariHantei()
    {
        // 親オブジェクト（GameClear）が存在する場合
        if (this != null)
        {
            // 配下のオブジェクトをすべてチェック
            foreach (Transform child in this.transform)
            {
                if (child != null)
                {
                    // ブロックオブジェクトの場合
                    if (child.CompareTag("Block") == true)
                    {
                        if (child.GetComponent<BoxCollider>() != null)
                        {
                            // ブロックすべてを当たり判定に変更
                            child.GetComponent<BoxCollider>().isTrigger = true;
                        }
                    }
                }
            }
        }
    }

    #endregion

}
