using UnityEngine;

[DisallowMultipleComponent]

public class BGM : MonoBehaviour
{
    public static BGM instance;
    private AudioSource cameraBgm;      // メインカメラに取り付けたAudioSource

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
        cameraBgm = GetComponent<AudioSource>();
    }

    #endregion

    #region ◆◆◆　　BGM関数　　◆◆◆

    /// <summary>
    /// BGMを止める
    /// </summary>
    public void StopBGM()
    {
        cameraBgm.Stop();
    }

    /// <summary>
    /// BGMをゆっくりにする
    /// </summary>
    public void SpeedDownBGM()
    {
        cameraBgm.pitch = 0.9f;
    }

    /// <summary>
    /// BGMを元の速度に戻す
    /// </summary>
    public void SpeedNormalBGM()
    {
        cameraBgm.pitch = 1.0f;
    }

    #endregion

    #region ◆◆◆　　BGM以外の関数　　◆◆◆

    /// <summary>
    /// DestroyするオブジェクトからAudioを鳴らす関数
    /// </summary>
    /// <param name="clip">AudioClip</param>
    /// <remarks>注意：カメラの近くで鳴らさないと、聞こえない</remarks>
    public void PlayAudioClip(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }

    #endregion

}
