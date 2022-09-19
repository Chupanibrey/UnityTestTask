using UnityEngine;
using UnityEngine.AI;
using System.Linq;

/// <summary>
/// Сущность бота
/// </summary>
public class Bot : MonoBehaviour
{
    public BotData data;
    BotAI AI;

    void Awake()
    {
        var gameManager = GameObject.FindGameObjectWithTag("GameManager")
            .GetComponent<BotSpawn>();
        var navMeshAgent = GetComponent<NavMeshAgent>();

        data = new BotData();
        AI = new BotAI(data, navMeshAgent, gameManager);
    }

    void Update()
    {
        AI.On(transform);
    }
}

/// <summary>
/// Класс характеристик бота
/// </summary>
public class BotData
{
    public float healph;
    public int score;
    public float damage;
    public float timeTilNextHit;
    public float timeBetweenHit;
    public bool allBotsDead;

    float speed;

    /// <summary>
    /// Метод задаёт рандомные характеристики для бота
    /// </summary>
    /// <param name="navMeshAgent">Агент навигационной сетки</param>
    public void RandomData(NavMeshAgent navMeshAgent)
    {
        healph = Random.Range(40f, 100f);
        damage = Random.Range(5f, 15f);
        speed = Random.Range(4f, 8f);
        navMeshAgent.speed = speed;

        timeTilNextHit = 0.0f;
        timeBetweenHit = Random.Range(0.5f, 1.5f);
    }
}

class BotAI
{
    BotSpawn gameManager;
    NavMeshAgent navMeshAgent;
    BotData data;
    Bot target;

    /// <summary>
    /// Конструктор подключает все необходимые данные для работы AI
    /// </summary>
    public BotAI(BotData values, NavMeshAgent navMeshAgent, BotSpawn gameManager)
    {
        values.RandomData(navMeshAgent);

        this.data = values;
        this.gameManager = gameManager;
        this.navMeshAgent = navMeshAgent;
    }


    /// <summary>
    /// Метод ищет ближайшую цель
    /// </summary>
    /// <param name="currPos">Текущая позиция бота</param>
    void TargetSearch(Transform currPos)
    {
        target = gameManager.allBots
            .SelectMany(b => b.GetComponents<Bot>())
            .OrderBy(b => CalculateDistance(currPos, b))
            // После сортировки, самый ближний бот это текущий, поэтому себя мы пропускаем
            .Skip(1)
            .First();
    }

    float timeTilNextInvoke = 0f;
    float timeBetweenInvoke = 1.5f;

    void TargetSearchRepetition(Transform currPos)
    {
        if (timeTilNextInvoke < 0)
        {
            TargetSearch(currPos);
            timeTilNextInvoke = timeBetweenInvoke;
        }

        timeTilNextInvoke -= Time.deltaTime;
    }

    float CalculateDistance(Transform currPos, Bot target)
    {
        NavMeshPath path = new NavMeshPath();
        float distance = 0f;

        NavMesh.CalculatePath(currPos.position, target.transform.position, NavMesh.AllAreas, path);

        for (int i = 0; i < path.corners.Length - 1; i++)
            distance += Vector3.Distance(path.corners[i], path.corners[i + 1]);

        return distance;
    }

    void MoveToTarget()
    {
        navMeshAgent.SetDestination(target.transform.position);
    }

    void AttackTarget()
    {
        if (data.timeTilNextHit < 0)
        {
            target.data.healph -= data.damage;
            data.timeTilNextHit = data.timeBetweenHit;
        }

        data.timeTilNextHit -= Time.deltaTime;
    }

    void TargetKilled()
    {
        data.score++;
        data.damage += 1.5f;
        gameManager.allBots.Remove(target);
        Object.Destroy(target.gameObject);
    }

    /// <summary>
    /// Метод запускает искусственный интеллект
    /// </summary>
    /// <param name="currPos">Текущая позиция бота</param>
    public void On(Transform currPos)
    {
        if (data.allBotsDead)
            return;

        if (gameManager.allBots.Count > 1)
        {
            TargetSearchRepetition(currPos);

            if (target == null)
                TargetSearch(currPos);

            if (Vector3.Distance(currPos.position, target.transform.position) > 1.5f)
                MoveToTarget();
            else
            {
                AttackTarget();

                if (target.data.healph <= 0)
                    TargetKilled();
            }
        }
        else
        {
            data.allBotsDead = true;
        }
    }
}