using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using static XCharts.Runtime.RadarCoord;

public class DataManager : MonoBehaviour
{
    // �Ω�ͦ����s���󪺹w�s��
    public GameObject dataPrefab;
    public string materialsFolderPath = "Data"; // ��Ƨ��W�١A�s�����
    private List<Material> materials = new List<Material>(); // ����C��

    public DirectionalIndicator indicator; // ���V DirectionalIndicator

    // �N��u/���P�_������
    public Material trueMaterial;
    public Material falseMaterial;

    // ��e���󪺴�V��
    private Renderer dataRenderer;
    // �ޥ� GameStatsManager
    private GameManager gameManager;
    // �Ω�O����l�ͦ���m���R�A�ܼ�

    private static Vector3 defaultPosition;
    // �лx�O�_�w�O����l��m
    private static bool isDefaultPositionSet = false;
    // �R�A�C��A�Ω�s�x�Ҧ��� data ����
    private static List<GameObject> allDataObjects = new List<GameObject>();

    // ��l�ƪ���
    private void Start()
    {
        // �����e���󪺴�V��
        dataRenderer = GetComponent<Renderer>();

        // ��� GameStatsManager
        gameManager = FindObjectOfType<GameManager>();

        // �p�G�|���O����l�ͦ���m�A�O����e���󪺦�m
        if (!isDefaultPositionSet)
        {
            defaultPosition = transform.position;
            isDefaultPositionSet = true;
        }
        // �q��Ƨ����J����
        LoadMaterials();

        // ���Ĥ@�Ӫ�����t����
        if (materials.Count > 0 && dataRenderer != null)
        {
            AssignRandomMaterial(dataRenderer);
        }
        else
        {
            Debug.LogWarning("����C���ũΥ������V���A�L�k���Ĥ@�Ӫ�����t����I");
        }

        // �N��e����[�J�C��
        allDataObjects.Add(gameObject);
    }

    private void LoadMaterials()
    {
        // �q Resources ��Ƨ����J�Ҧ�����
        Material[] loadedMaterials = Resources.LoadAll<Material>(materialsFolderPath);

        // �p�G�����J���\�A�[�J����C��
        if (loadedMaterials.Length > 0)
        {
            materials.AddRange(loadedMaterials);
            Debug.Log($"���\���J {materials.Count} �ӧ���");
        }
        else
        {
            Debug.LogError($"����q��Ƨ� {materialsFolderPath} ���J����I");
        }
    }

    // ����i�JĲ�o�ϰ�ɰ���
    private void OnTriggerEnter(Collider other)
    {
        // �P�_Ĳ�o�ϰ쪺���Ҩöi������ˬd
        if (other.CompareTag("TruePlane"))
        {
            EvaluateMaterial("True"); // �ˬd�O�_�O True ����
        }
        else if (other.CompareTag("FalsePlane"))
        {
            EvaluateMaterial("False"); // �ˬd�O�_�O False ����
        }
    }

    // �ˬd��e���󪺧���O�_�P�ؼЧ���@�P
    private void EvaluateMaterial(string expectedPrefix)
    {
        if (gameManager == null || gameManager.IsGameEnded()) return; // �p�G�C�������A����B�z

        // �����e���󪺧���W��
        string materialName = dataRenderer.sharedMaterial.name;

        // �P�_����W�٬O�_�H���w�e��}�Y
        bool isCorrect = materialName.StartsWith(expectedPrefix);

        // �O���P�_���G
        gameManager.RecordResult(isCorrect);

        Debug.Log(isCorrect
            ? $"�P�_���T�I����W�G{materialName} �P�e�� {expectedPrefix} �ǰt"
            : $"�P�_���~�I����W�G{materialName} �P�e�� {expectedPrefix} ���ǰt");

        // �~��B�z����
        DataPass();
    }


    // �B�z����ͦ��M��e���󪺾P��
    private void DataPass()
    {
        if (gameManager != null && gameManager.IsGameEnded()) return;

        // �ͦ��s����A�ϥΰO������l��m
        GameObject newData = Instantiate(dataPrefab, defaultPosition, Quaternion.Euler(0, 180, 0));

        // �H�����s�ͦ���������t����
        AssignRandomMaterial(newData.GetComponent<Renderer>());

        // ��� DirectionalIndicator �ç�s��ؼ�
        DirectionalIndicator indicator = FindObjectOfType<DirectionalIndicator>();

        if (indicator != null)
        {
            indicator.SetDirectionalTarget(newData.transform);
        }

        // �T�η�e����A�קK���~������L�޿�
        gameObject.SetActive(false);

        // �P����e����
        Destroy(gameObject);
    }

    // �H�����t���赹���w����V��
    private void AssignRandomMaterial(Renderer renderer)
    {
        if (materials.Count == 0)
        {
            Debug.LogError("����C���šA�L�k���t����I");
            return;
        }

        // �H����ܤ@�ӧ���
        int randomIndex = Random.Range(0, materials.Count);
        renderer.material = materials[randomIndex];

        Debug.Log($"���t����G{materials[randomIndex].name}");
    }

    // Reset ��k�A�N�Ҧ� data ���󲾰ʦ^ defaultPosition
    public static void Relocate()
    {
        foreach (GameObject data in allDataObjects)
        {
            if (data != null)
            {
                data.transform.position = defaultPosition;
            }
        }

        Debug.Log("�Ҧ�����w���m���l��m");
    }
}
