using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class StartButton : MonoBehaviour
{
    [SerializeField] private string scenename;

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// 随時処理（キー入力受付）
    /// </summary>
    void Update()
    {
        // Enter キー押下時
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // スタートボタン押下イベント
            SceneMove();
        }

    }

    /// <summary>
    /// スタートボタン押下時
    /// </summary>
    public void SceneMove()
    {
        // シーンに移動
        SceneManager.LoadScene(scenename);
    }

    #endregion

}
