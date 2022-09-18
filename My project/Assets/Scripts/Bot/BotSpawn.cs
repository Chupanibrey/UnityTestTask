using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������� �������� �����
/// </summary>
public class BotSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject botPrefab;
    [SerializeField]
    MeshCollider plane;
    [SerializeField]
    int numberOfBots;

    public List<Bot> allBots;

    void Start()
    {
        for (int i = 0; i < numberOfBots; i++)
            BotInstantiate();

        foreach (var bot in allBots)
            bot.Initialization();
    }

    /// <summary>
    /// ����� ������ ���� � ��������� ����� �����������
    /// </summary>
    void BotInstantiate()
    {
        var x = Random.Range(plane.bounds.min.x, plane.bounds.max.x);
        var z = Random.Range(plane.bounds.min.z, plane.bounds.max.z);
        var spawnPos = new Vector3(x, 1.15f, z);

        if (CheckSpawnPoint(spawnPos))
        {
            allBots.Add(Instantiate(botPrefab, spawnPos, Quaternion.identity, this.transform)
                        .GetComponent<Bot>());
        }
        else
            BotInstantiate();
    }

    /// <summary>
    /// ����� ��������� ����������� �����������
    /// </summary>
    /// <param name="spawnPos">����� �� �����������</param>
    bool CheckSpawnPoint(Vector3 spawnPos)
    {
        var colliders = Physics.OverlapBox(spawnPos, botPrefab.transform.localScale);

        return colliders.Length > 0 ? false : true;
    }
}