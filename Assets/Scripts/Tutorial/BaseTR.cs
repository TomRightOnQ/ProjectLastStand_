using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Base in tutorial mode
public class BaseTR : MonoBehaviour
{
    [SerializeField] private GameObject GameUI;
    [SerializeField] private GameObject blockingCanvas;

    private void Awake()
    {
        Vector3 Pos1 = new Vector3(50, 0, 0);
        Vector3 Pos2 = new Vector3(70, 0, 0);
        spawnDropeed(Pos1, 1);
        StartCoroutine(wait());
        spawnDropeed(Pos2, 2);
    }

    private IEnumerator wait()
    {
        yield return new WaitForSecondsRealtime(0.1f);
    }

    private void spawnDropeed(Vector3 pos, long id)
    {
        GameObject dropObj = Instantiate(PrefabManager.Instance.DroppedWeapon, pos, Quaternion.identity);
        DroppedItems dropped = dropObj.GetComponent<DroppedItems>();
        dropped.WeaponIndex = 1;
        dropped.SetUp();
        dropped.DroppedId = id;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
            GameManager.Instance.LockInput();
            Players player = GameManager.Instance.GetLocalPlayer();
            player.Deactivate();
            GameUI.SetActive(false);
            ExpAndLevels.Instance.gameObject.SetActive(false);
            StartCoroutine(LerpWhiteScreenRoutine());
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

    public void MainMenu()
    {
        SceneManager.LoadScene("Scoreboard");
        Time.timeScale = 1f;
    }
}
