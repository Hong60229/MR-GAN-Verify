using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalRounds = 0; // �`�^�X��
    public int correctCount = 0; // ���T����
    public int wrongCount = 0; // ���~����

    public int maxRounds = 10; // �̤j�^�X��
    private UIManager uiManager; // �ޥ� UI �޲z��
    private bool isGameEnded = false; // �лx�C���O�_����

    private void Start()
    {
        // ��� UI �޲z��
        uiManager = FindObjectOfType<UIManager>();

        if (uiManager == null)
        {
            Debug.LogError("GameUIManager �����I");
        }
    }

    public float Accuracy
    {
        get
        {
            if (totalRounds == 0) return 0f;
            return (float)correctCount / totalRounds * 100f;
        }
    }

    public void RecordResult(bool isCorrect)
    {
        if (isGameEnded) return; // �p�G�C���w�����A������^

        totalRounds++;

        if (isCorrect)
        {
            correctCount++;
        }
        else
        {
            wrongCount++;
        }

        // ��s UI
        if (uiManager != null)
        {
            uiManager.UpdateUI();
        }

        // �p�G�F��̤j�^�X�ơA�����C��
        if (totalRounds >= maxRounds)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        isGameEnded = true; // �аO�C������

        // ��ܰV�m���i
        Debug.Log("�C�������I");
        if (uiManager != null)
        {
            uiManager.ShowReport(correctCount, wrongCount, Accuracy);
        }
    }

    public bool IsGameEnded()
    {
        return isGameEnded;
    }

}
