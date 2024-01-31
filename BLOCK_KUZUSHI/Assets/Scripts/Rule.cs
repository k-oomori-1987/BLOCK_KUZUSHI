using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class Rule : MonoBehaviour
{
    public static Rule instance;

    [SerializeField] private GameObject rulePanel;  // ルールパネル（今開いているパネル）
    [SerializeField] private GameObject nextPanel;  // 次のルールパネル
    [SerializeField] private GameObject backPanel;  // 前のルールパネル

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// 随時処理（キー入力受付）
    /// </summary>
    void Update()
    {
        // スペース キー押下時
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 遊び方ボタン押下イベント
            OpenRulePanel();
        }
        // ← キー押下時
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 前へボタン押下イベント
            BackRulePanel();
        }
        // → キー押下時
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 次へボタン押下イベント
            NextRulePanel();
        }
        // ESC キー押下時
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 閉じるボタン押下時イベント
            CloseRulePanel();
        }
    }

    /// <summary>
    /// 遊び方ボタン押下時
    /// </summary>
    public void OpenRulePanel()
    {
        if (rulePanel != null)
        {
            rulePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 閉じるボタン押下時
    /// </summary>
    public void CloseRulePanel()
    {
        rulePanel.SetActive(false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// 次へボタン押下時
    /// </summary>
    public void NextRulePanel()
    {
        if (nextPanel != null)
        {
            nextPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 前へボタン押下時
    /// </summary>
    public void BackRulePanel()
    {
        if (backPanel != null)
        {
            backPanel.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

    #endregion
}
