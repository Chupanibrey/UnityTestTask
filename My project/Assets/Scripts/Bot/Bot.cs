using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Bot : MonoBehaviour
{
    #region [ BotData ]
    [SerializeField]
    public float healph;
    float speed;
    float damage;
    #endregion

    [SerializeField]
    BotSpawn gameManager;

    Bot target;
    NavMeshAgent navMesh;

    float timeTilNextFire = 0.0f;
    float timeBetweenFires = 0.5f;

    public void Initialization()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager")
            .GetComponent<BotSpawn>();
        navMesh = GetComponent<NavMeshAgent>();
        RandomData();
        InvokeRepeating("TargetSearch", 0, 1f);
    }

    void Update()
    {
        if (target == null)
            TargetSearch();

        if (Vector3.Distance(transform.position, target.transform.position) > 1f )
            MoveTo(target);
        else
        {
            Attack();

            if (target.healph <= 0)
            {
                gameManager.allBots.Remove(target);
                Destroy(target.gameObject);
            }
        }
    }

    void Attack()
    {
        if (timeTilNextFire < 0)
        {
            target.healph -= damage;
            timeTilNextFire = timeBetweenFires;
        }

        timeTilNextFire -= Time.deltaTime;
    }

    void MoveTo(Bot target)
    {
        navMesh.SetDestination(target.transform.position);
    }

    void RandomData()
    {
        healph = Random.Range(40f, 100f);
        damage = Random.Range(5f, 15f);
        speed = Random.Range(4f, 8f);
    }

    void TargetSearch()
    {
        target = gameManager.allBots
            .SelectMany(b => b.GetComponents<Bot>())
            .OrderBy(b => CalculateDistance(b))
            // ѕосле сортировки, самый ближний бот это текущий, поэтому себ€ мы пропускаем
            .Skip(1)
            .First();
    }

    float CalculateDistance(Bot target)
    {
        NavMeshPath path = new NavMeshPath();
        float distance = 0f;

        NavMesh.CalculatePath(transform.position, target.transform.position, NavMesh.AllAreas, path);

        for (int i = 0; i < path.corners.Length - 1; i++)
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);

        return distance;
    }
}