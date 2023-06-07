using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using static ConfigManager;
using static MonsterConfigs;

// Main Game Manager
public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] bool tutorialMode = false;

    private bool isLoaded = false;

    private static GameManager instance;
    public DataManager dataManager;
    public MonsterManager monsterManager;

    [SerializeField] private float gameTime = 0f;

    // Data
    private MonsterConfig[] monsterData;
    private WeaponConfig[] WeaponData;

    // Canvases
    [SerializeField] private Canvas loadingScreen;
    [SerializeField] private Canvas PauseCanvas;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject blockingCanvas;

    private bool isPaused = false;

    public bool IsPaused { get { return isPaused; } }

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        Debug.Log("Awaking");
        loadingScreen.gameObject.SetActive(true);
        // Check repeated instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // init all manager objects
            instance = this;
            GameObject monsterManagerObj = new GameObject("MonsterManager");
            monsterManager = monsterManagerObj.AddComponent<MonsterManager>();
            dataManager.initData();

            if (monsterManager && dataManager)
            {
                Debug.Log("All managers loaded");
                if (!tutorialMode) {
                    monsterManager.Begin();
                }
                isLoaded = true;
            }
        }
        StartCoroutine(DisableScreenAfterDelay(3f));
    }

    void Update()
    {
        gameTime += Time.deltaTime;
        PlayerListener.Instance.UpdateTime(gameTime);
        if (!isLoaded)
        {
            Debug.Log("Loading...");
            return;
        }
        Players[] players = dataManager.GetPlayers();
    }

    public void GameOver()
    {
        LockInput();
        Players player = GetLocalPlayer();
        Camera _camera = Camera.main;
        _camera.transform.position = new Vector3(0, _camera.transform.position.y, -51.9f);
        player.Deactivate();
        GameUI.SetActive(false);
        ExpAndLevels.Instance.gameObject.SetActive(false);

        StartCoroutine(PlayDestroyAnim());
        StartCoroutine(ScreenShakeRoutine());
        StartCoroutine(LerpWhiteScreenRoutine());
    }

    private IEnumerator PlayDestroyAnim()
    {
        float delayBetweenAnims = 0.4f;
        float totalTime = 4f;
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            GameObject animObject = Instantiate(AnimConfigs.Instance.GetAnim(0), Vector3.zero, Quaternion.identity);
            animObject.transform.position = GetRandomPositionAroundCenter();
            animObject.transform.localRotation = Quaternion.Euler(45f, 0f, 0f);
            animObject.transform.localScale = new Vector3(2f, 2f, 2f);

            yield return new WaitForSeconds(delayBetweenAnims);

            elapsedTime += delayBetweenAnims;
        }
    }

    private Vector3 GetRandomPositionAroundCenter()
    {
        float radius = 10f; // Adjust the radius as desired
        Vector2 randomCirclePoint = Random.insideUnitCircle * radius;
        float randomY = Random.Range(0, 10);
        Vector3 randomPosition = new Vector3(randomCirclePoint.x, randomY, randomCirclePoint.y);
        return randomPosition;
    }

    private IEnumerator ScreenShakeRoutine()
    {
        Camera mainCamera = Camera.main;
        float shakeDuration = 4f;
        float shakeIntensity = 0.1f;
        Vector3 originalCameraPosition = mainCamera.transform.localPosition;

        float elapsedShakeTime = 0f;

        while (elapsedShakeTime < shakeDuration)
        {
            float randomX = Random.Range(-1f, 1f) * shakeIntensity;
            float randomY = Random.Range(-1f, 1f) * shakeIntensity;
            float randomZ = Random.Range(-1f, 1f) * shakeIntensity;

            mainCamera.transform.localPosition = originalCameraPosition + new Vector3(randomX, randomY, randomZ);

            elapsedShakeTime += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.localPosition = originalCameraPosition; // Reset camera position
    }

    private IEnumerator LerpWhiteScreenRoutine()
    {
        Image whiteScreen = blockingCanvas.GetComponentInChildren<Image>();
        float lerpDuration = 4f;
        float elapsedLerpTime = 0f;
        Color targetColor = whiteScreen.color;

        while (elapsedLerpTime < lerpDuration)
        {
            float alpha = Mathf.Lerp(0f, 1f, elapsedLerpTime / lerpDuration);
            targetColor.a = alpha;
            whiteScreen.color = targetColor;

            elapsedLerpTime += Time.deltaTime;
            yield return null;
        }

        targetColor.a = 1f;
        whiteScreen.color = targetColor;
        MainMenu();
    }

    // Add exp to players
    public void AddExp(float amount)
    {
        if (!PhotonNetwork.IsConnected || PhotonNetwork.IsMasterClient) {

            ExpAndLevels expObj = GameObject.FindObjectOfType<ExpAndLevels>();
            if (expObj != null && !PhotonNetwork.IsConnected)
            {
                expObj.EXP += amount;
            } else if (expObj != null && PhotonNetwork.IsConnected) 
            {
                expObj.EXP += amount/PhotonNetwork.PlayerList.Length;
            }
        }
    }

    // Looking for a player
    public Players GetLocalPlayer()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = PhotonView.Find(GameManager.Instance.dataManager.PlayerViewID);
            if (photonView != null)
            {
                return photonView.GetComponent<Players>();
            }
            else
            {
                Debug.Log("Cannot Find!");
                return null;
            }
        }
        else
        {
            return GameManager.Instance.dataManager.GetPlayers()[0];
        }
    }

    public Players GetLocalPlayer(int viewID)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = PhotonView.Find(viewID);
            if (photonView != null)
            {
                return photonView.GetComponent<Players>();
            }
            else
            {
                return null;
            }
        }
        return null;
    }

    // Looking for a dropped weapon
    public DroppedItems GetDroppedItems(int viewID)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonView photonView = PhotonView.Find(viewID);
            if (photonView != null)
            {
                DroppedItems dropped = photonView.GetComponent<DroppedItems>();
                Debug.Log($"Found DroppedItems with viewID {viewID}: {dropped}");
                return dropped;
            }
            else
            {
                Debug.LogWarning($"PhotonView not found for viewID {viewID}");
                return null;
            }
        }
        Debug.LogWarning("Not connected to Photon network");
        return null;
    }

    private IEnumerator DisableScreenAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        loadingScreen.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        PauseCanvas.gameObject.SetActive(true);
        isPaused = true;
        if (!PhotonNetwork.IsConnected)
        {
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        PauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        if (!PhotonNetwork.IsConnected)
        {
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.Disconnect();
        }
        SceneManager.LoadScene("Scoreboard");
        Time.timeScale = 1f;
    }

    public void LockInput()
    {
        blockingCanvas.SetActive(true);
    }

    public void UnlockInput()
    {
        blockingCanvas.SetActive(false);
    }
}
