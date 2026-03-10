using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// カメラがターゲット（プレイヤー）を滑らかに追従するクラス。
/// </summary>

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothing = 5f;
    float maxY = 2000f;
    float minY = -2000f;
    float maxX = 2000f;
    float minX = -2000f;

    private static CameraFollow instance;

    private void Awake()
    {
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
    public void CameraPosLimitChange(int maxy, int miny, int maxx,int minx)
    {
        maxY = maxy;
        minY = miny;
        maxX = maxx;
        minX = minx;
    }
    public void CameraPosLimitChange()
    {
        maxY = 2000;
        minY = -2000;
        maxX = 2000;
        minX = -2000;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            target = player.transform;

            // シーン遷移直後の瞬間ワープ防止
            Vector3 pos = target.position;
            pos.x = -30f;
            transform.position = pos;
        }
        if (SceneManager.GetActiveScene().name == "Home")
        {
            CameraPosLimitChange();
        }
        else if (SceneManager.GetActiveScene().name == "Comcolle")
        {
            CameraPosLimitChange(180,180,0,0);
        }
        else if (SceneManager.GetActiveScene().name == "ToNeto")
        {
            CameraPosLimitChange();
        }
        else if (SceneManager.GetActiveScene().name == "InFrontOfLamentForest")
        {
            CameraPosLimitChange(90, 90, 320, 0);
        }
        else if (SceneManager.GetActiveScene().name == "PoisonedSpring")
        {
            CameraPosLimitChange();
        }
        else if (SceneManager.GetActiveScene().name == "ErrorVillage")
        {
            CameraPosLimitChange();
        }
        else if (SceneManager.GetActiveScene().name == "CorrupedTown")
        {
            CameraPosLimitChange();
        }
        else if (SceneManager.GetActiveScene().name == "Temple")
        {
            CameraPosLimitChange();
        }
        else if (SceneManager.GetActiveScene().name == "Unknown")
        {
            CameraPosLimitChange();
        }
    }
    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name=="Dojo")
        {
            Vector3 newPos = new Vector3(0,90,-10);
            transform.position = newPos;
        }
        else
        {
            if (target == null) return;

            Vector3 targetPos = target.position;
            targetPos.z = -15f;

            Vector3 newPos = Vector3.Lerp(
                transform.position,
                targetPos,
                smoothing * Time.deltaTime
            );

            newPos.x = Mathf.Max(Mathf.Min(newPos.x, maxX), minX);
            newPos.y = Mathf.Max(Mathf.Min(newPos.y, maxY), minY);
            newPos.x = Mathf.Round(newPos.x);
            newPos.y = Mathf.Round(newPos.y);

            transform.position = newPos;
        }
    }
}
