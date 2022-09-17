using UnityEngine;

public class BotSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject botPrefab;
    [SerializeField]
    private MeshCollider plane;
    [SerializeField]
    private int numberOfBots;

    private void Start()
    {
        for(int i = 0; i < numberOfBots; i++)
            BotInstantiate();
    }

    /// <summary>
    /// Метод создаёт бота в случайной точке поверхности
    /// </summary>
    void BotInstantiate()
    {
        var x = Random.Range(plane.bounds.min.x, plane.bounds.max.x);
        var z = Random.Range(plane.bounds.min.z, plane.bounds.max.z);
        var spawnPos = new Vector3(x, 1.15f, z);

        if (CheckSpawnPoint(spawnPos))
            Instantiate(botPrefab, spawnPos, Quaternion.identity, this.transform);
        else
            BotInstantiate();
    }

    /// <summary>
    /// Метод проверяет пересечения коллайдеров
    /// </summary>
    /// <param name="spawnPos">Точка на поверхности</param>
    bool CheckSpawnPoint(Vector3 spawnPos)
    {
        var colliders = Physics.OverlapBox(spawnPos, botPrefab.transform.localScale);

        return colliders.Length > 0 ? false : true;
    }
}