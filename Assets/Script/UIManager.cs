using TMPro; // �ޤJ TextMeshPro ���R�W�Ŷ�
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI roundsText; // �^�X�Ƥ奻
    public TextMeshProUGUI accuracyText; // �ǽT�פ奻

    public GameObject reportWindow; // �V�m���i���f
    public TextMeshProUGUI reportText; // �V�m���i���e

    public GameObject hintWindow;
    public GameObject scoreWindow;

    private GameManager gameManager; // �ޥ� GameManager

    private void Start()
    {
        // ����έp�޲z��
        gameManager = FindObjectOfType<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameStatsManager �����I");
        }

        // ��l�� UI
        UpdateUI();

        // ���ðV�m���i���f
        if (reportWindow != null)
        {
            reportWindow.SetActive(false);
        }
    }

    public void UpdateUI()
    {
        if (gameManager == null) return;

        // ��s�^�X�ƩM�ǽT��
        roundsText.text = $"�^�X�� : {gameManager.totalRounds + 1} / {gameManager.maxRounds}";
        accuracyText.text = $"�ǽT�� : {gameManager.Accuracy:F2}%";
    }

    public void ShowReport(int correct, int wrong, float accuracy)
    {
        // ��ܰV�m���i���f
        if (reportWindow != null)
        {
            reportWindow.SetActive(true);
        }

        // �]�w�V�m���i���e
        if (reportText != null)
        {
            reportText.text = $"�V�m���i\n\n" +
                              $"���T����: {correct}\n" +
                              $"���~����: {wrong}\n" +
                              $"�ǽT��: {accuracy:F2}%";
        }

        if (hintWindow != null)
        {
            hintWindow.SetActive(false);
        }

        if (scoreWindow != null)
        {
            scoreWindow.SetActive(false);
        }

    }

    public void ToggleHintWindow()
    {
        if (hintWindow != null)
        {
            // ���� HintWindow ���ҰʩM�������A
            hintWindow.SetActive(!hintWindow.activeSelf);
        }
        else
        {
            Debug.LogWarning("HintWindow ���]�m�I");
        }
    }

    public void ToggleScoreWindow()
    {
        if (scoreWindow != null)
        {
            // ���� HintWindow ���ҰʩM�������A
            scoreWindow.SetActive(!scoreWindow.activeSelf);
        }
        else
        {
            Debug.LogWarning("ScoreWindow ���]�m�I");
        }
    }

    public void RestartProject()
    {
        // �����e���ʳ������W��
        string currentSceneName = SceneManager.GetActiveScene().name;

        // �[����e�����A��{���ҮĪG
        SceneManager.LoadScene(currentSceneName);

        Debug.Log("�M�פw���ҡI");
    }

}
