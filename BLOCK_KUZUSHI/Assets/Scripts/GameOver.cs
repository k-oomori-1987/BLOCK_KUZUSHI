using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class GameOver : MonoBehaviour
{
    public static GameOver instance;

    [SerializeField] private GameObject gameoverPanel;      // ゲームオーバーのパネル
    [SerializeField] private GameObject retryButton;        // リトライボタン
    [SerializeField] private AudioSource gameoverSound;     // ゲームオーバー時のサウンド

    [SerializeField] private GameObject explosionPrefab;    // 爆発のエフェクト
    [SerializeField] private AudioClip explosionSound;      // 爆発のサウンド

    [SerializeField] private GameObject rulePanel;          // 遊び方パネル
    [SerializeField] private GameObject ruleButton;         // 遊び方ボタン

    public bool gameoverFlg = false;    // ゲームオーバーフラグ

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
        // 初期化
        gameoverFlg = false;

        // ゲームオーバーパネル
        gameoverPanel.SetActive(false);
        retryButton.SetActive(false);

    }

    /// <summary>
    /// 随時処理（キー入力受付）
    /// </summary>
    void Update()
    {
        // リトライボタンが表示されている場合
        if (retryButton.activeSelf == true)
        {
            // Enter キー押下時
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // リトライボタン押下イベント
                RetryButton();
            }
        }

    }

    /// <summary>
    /// リトライボタン押下時
    /// </summary>
    public void RetryButton()
    {
        // 現在のシーンを再読み込み
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// ゲームオーバーラインに入った時
    /// </summary>
    /// <param name="other">入ってきたオブジェクト</param>
    private void OnTriggerEnter(Collider other)
    {
        // ボールやアイテムを消す
        Destroy(other.gameObject);

        // ボールの場合、ゲームオーバー
        if (other.CompareTag("Ball") == true)
        {
            // フラグを更新
            gameoverFlg = true;

            // BGMを止める
            BGM.instance.StopBGM();

            // 爆発のエフェクト表示
            GameObject effect = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
            Destroy(effect, 1.0f);

            // 爆発の音を鳴らす
            BGM.instance.PlayAudioClip(explosionSound);

            // 遊び方パネル・ボタンを非表示
            rulePanel.SetActive(false);
            ruleButton.SetActive(false);

            // 遅延処理
            StartCoroutine(GameOverDerayProcess());
        }
    }

    #endregion

    #region ◆◆◆　　遅延処理　　◆◆◆

    private IEnumerator GameOverDerayProcess()
    {
        if (gameoverFlg == true)
        {
            // しばらく待つ
            yield return new WaitForSeconds(1.0f);

            // ゲームオーバーパネル（リトライボタンはまだ非表示）
            gameoverPanel.SetActive(true);

            // しばらく待つ
            yield return new WaitForSeconds(0.5f);

            // ゲームオーバー時のサウンドを鳴らす
            gameoverSound.Play();

            // リトライボタンを表示
            retryButton.SetActive(true);

            // コルーチンを止める
            StopCoroutine(GameOverDerayProcess());
        }
    }

    #endregion

}
