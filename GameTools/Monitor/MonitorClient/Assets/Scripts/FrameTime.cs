using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 1フレームあたりの処理時間を計測するクラス
/// </summary>
public class FrameTime : MonoBehaviour
{
    private static FrameTime _instance;
    public static FrameTime Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject(nameof(FrameTime)).AddComponent<FrameTime>();
            }
            return _instance;
        }
    }

    /// <summary>
    /// CPUの1フレーム当たりの処理時間
    /// </summary>
    public float CpuFrameTime { get; private set; }
    /// <summary>
    /// GPUの1フレーム当たりの処理時間
    /// </summary>
    public float GpuFrameTime { get; private set; }

    private FrameTiming[] _frameTimings = new FrameTiming[1];

    public Text text;

    private void Update()
    {
        // フレーム情報をキャプチャする
        FrameTimingManager.CaptureFrameTimings();

        // 必要なフレーム数分の情報を取得する
        // 戻り値は実際に取得できたフレーム情報の数
        var numFrames = FrameTimingManager.GetLatestTimings((uint)_frameTimings.Length, _frameTimings);
        if (numFrames == 0) // 2020.02.16修正しました
        {
            // 1フレームの情報も得られていない場合はスキップ
            return;
        } 

        // CPUの処理時間、CPUの処理時間を格納
        CpuFrameTime = (float)(_frameTimings[0].cpuFrameTime * 1000);
        GpuFrameTime = (float)(_frameTimings[0].gpuFrameTime * 1000);

        text.text = string.Format($"CPU = {CpuFrameTime},GPU={GpuFrameTime}");
    }
}