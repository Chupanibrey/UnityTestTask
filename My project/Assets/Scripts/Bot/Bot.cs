using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class Bot : MonoBehaviour
{
    #region [ BotData ]
    [SerializeField]
    public float healph;
    public int score;

    float speed;
    float damage;

    float timeTilNextHit = 0.0f;
    float timeBetweenHit = 0.5f;
    #endregion

    BotSpawn gameManager;
    NavMeshAgent navMesh;
    Bot target;

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
        BotAI();
    }

    void BotAI()
    {
        if (gameManager.allBots.Count > 1)
        {
            if (target == null)
                TargetSearch();

            if (Vector3.Distance(transform.position, target.transform.position) < 1f)
            {
                Attack();

                if (target.healph <= 0)
                {
                    score++;
                    gameManager.allBots.Remove(target);
                    Destroy(target.gameObject);
                }
            }
            else
                MoveTo(target);
        }
        else
            CancelInvoke("TargetSearch");
    }

    void Attack()
    {
        if (timeTilNextHit < 0)
        {
            target.healph -= damage;
            timeTilNextHit = timeBetweenHit;
        }

        timeTilNextHit -= Time.deltaTime;
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
        navMesh.speed = speed;
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