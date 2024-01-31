using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]

public class GameStart : MonoBehaviour
{
    [SerializeField] private Text pressAnyKey;          // スタート時のテキスト
    [SerializeField] private byte speed = 5;            // 点滅速度
    [SerializeField] private float interval = 0.02f;    // 次の点滅までのインターバル
    [SerializeField] private GameObject ball;           // ボールオブジェクト
    [SerializeField] private float startTime = 1;       // 開始からスタート可能な時間

    private float InitClearValue;   // 透過度（初期値）
    private float pastTime = 0;     // スタートから経過した時間

    #region ◆◆◆　　イベント　　◆◆◆

    /// <summary>
    /// スタート
    /// </summary>
    void Start()
    {
        // ボールオブジェクトを非表示
        ball.SetActive(false);

        // 透過度を保持
        InitClearValue = pressAnyKey.color.a;

        // スタートから経過した時間を初期化
        pastTime = 0;

        // コルーチンを開始
        StartCoroutine(FlashText());
    }

    /// <summary>
    /// 随時処理（キー入力待ち処理）
    /// </summary>
    void Update()
    {
        // 経過時間を足していく
        pastTime += Time.deltaTime;

        Debug.Log(pastTime.ToString());

        if (pastTime >= startTime)
        {
            // 何かキーを押下された場合（ただし、マウスは対象外）
            if (Input.anyKey && !Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)) 
            {
                // 「何かキーを押してください」の文字が表示されている場合
                if (this.gameObject.activeSelf == true)
                {
                    // コルーチンを停止
                    StopCoroutine(FlashText());

                    // スタート文字を非表示
                    this.gameObject.SetActive(false);

                    // ボールオブジェクトを表示し、発射する
                    ball.SetActive(true);
                    Ball.instance.BallStart();
                }
            }
        }
    }

    #endregion

    #region ◆◆◆　　コルーチン　　◆◆◆

    /// <summary>
    /// 文字の点滅処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashText()
    {
        bool changeFlg = false; // 点滅切替フラグ

        while (true)
        {
            if (changeFlg)
            {
                // 徐々に戻す
                pressAnyKey.color = pressAnyKey.color + new Color32(0, 0, 0, speed);
                yield return new WaitForSeconds(interval);
            }
            else
            {
                // 徐々に透過
                pressAnyKey.color = pressAnyKey.color - new Color32(0, 0, 0, speed);
                yield return new WaitForSeconds(interval);
            }

            if (InitClearValue <= pressAnyKey.color.a)
            {
                // 元の透過度を超えた場合、透過するようにフラグを更新
                changeFlg = false;
            }
            else if(pressAnyKey.color.a <= 0)
            {
                // 完全に透過した場合、元にもどすようにフラグを更新
                changeFlg = true;
            }
        }
    }

    #endregion

}
