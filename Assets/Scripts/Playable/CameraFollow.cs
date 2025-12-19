using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// カメラがターゲット（プレイヤー）を滑らかに追従するクラス。
/// </summary>

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    public float maxY = 45f;

    private static CameraFollow instance;

    private void Awake()
    {
        // 重複防止
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーン切替後に Player を再取得
        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            target = player.transform;

            // シーン遷移直後の瞬間ワープ防止
            Vector3 pos = target.position;
            pos.x = -30f;
            transform.position = pos;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 targetPos = target.position;
        targetPos.z = -15f;

        Vector3 newPos = Vector3.Lerp(
            transform.position,
            targetPos,
            smoothing * Time.deltaTime
        );

        newPos.y = Mathf.Min(newPos.y, maxY);
        newPos.x = Mathf.Round(newPos.x);
        newPos.y = Mathf.Round(newPos.y);

        transform.position = newPos;
    }
}
