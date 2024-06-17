using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private GameObject EnemyPrefab;
    [SerializeField] private Transform Platform;
    [SerializeField] private int AmountToSpawn;
    [SerializeField] private float SizeMin, SizeMax;

    private int _amountOfEnemies;

    private Vector2 _platformX = Vector2.zero;
    private Vector2 _platformZ = Vector2.zero;

    [SerializeField]
    private List<Enemy> Enemies = new List<Enemy>();

    [SerializeField]
    private List<Enemy> BlueEnemies = new List<Enemy>();
    [SerializeField]
    private List<Enemy> RedEnemies = new List<Enemy>();
    [SerializeField]
    private List<Enemy> GreenEnemies = new List<Enemy>();
    [SerializeField]
    private List<Enemy> YellowEnemies = new List<Enemy>();

    private Dictionary<Color, int> _scoreColor = new Dictionary<Color, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        ClearColorPoints();
    }

    void Start()
    {
        _platformX = new Vector2(Platform.localScale.x / 2, -Platform.localScale.x / 2);
        _platformZ = new Vector2(Platform.localScale.z / 2, -Platform.localScale.z / 2);

        while (_amountOfEnemies < AmountToSpawn)
        {
            float x = Random.Range(_platformX.y, _platformX.x);
            float z = Random.Range(_platformZ.y, _platformZ.x);
            float r = Random.Range(SizeMin, SizeMax);

            Vector3 nextPosition = new Vector3(x, 0.66f, z);

            if (Enemies.Count == 0)
            {
                GameObject temp = Instantiate(EnemyPrefab, nextPosition, Quaternion.identity);
                Enemy enemyTemp = temp.GetComponent<Enemy>();
                enemyTemp.SetRadious(r);
                Enemies.Add(enemyTemp);
                AssignRandomColor(enemyTemp);
                _amountOfEnemies++;
                continue;
            }

            bool canSpawn = true;
            foreach (Enemy enemy in Enemies)
            {
                Vector3 enemyPosition = enemy.transform.position;
                if (Vector3.Distance(nextPosition, enemyPosition) < r + enemy.GetRadious())
                {
                    canSpawn = false;
                    break;
                }
            }

            if (canSpawn)
            {
                GameObject temp = Instantiate(EnemyPrefab, nextPosition, Quaternion.identity);
                Enemy enemyTemp = temp.GetComponent<Enemy>();
                enemyTemp.SetRadious(r);
                Enemies.Add(enemyTemp);
                AssignRandomColor(enemyTemp);
                _amountOfEnemies++;
            }
        }
    }

    void AssignRandomColor(Enemy enemy)
    {
        Renderer renderer = enemy.GetComponent<Renderer>();

        if (renderer != null)
        {
            Color[] colors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow, 
                PlayerController.Instance.playerColor, PlayerController.Instance.playerColor };

            Color randomColor = colors[Random.Range(0, colors.Length)];

            renderer.material.color = randomColor;

            if (randomColor == Color.red)
            {
                RedEnemies.Add(enemy);
            }
            else if (randomColor == Color.blue)
            {
                BlueEnemies.Add(enemy);
            }
            else if (randomColor == Color.green)
            {
                GreenEnemies.Add(enemy);
            }
            else if (randomColor == Color.yellow)
            {
                YellowEnemies.Add(enemy);
            }
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        Renderer renderer = enemy.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color randomColor = renderer.material.color;
            if (randomColor == Color.red)
            {
                if (_scoreColor.ContainsKey(Color.red))
                {
                    _scoreColor[Color.red] += 1;
                }

                RedEnemies.Remove(enemy);
            }
            else if (randomColor == Color.blue)
            {
                if (_scoreColor.ContainsKey(Color.blue))
                {
                    _scoreColor[Color.blue] += 1;
                }

                RedEnemies.Remove(enemy);
            }
            else if (randomColor == Color.green)
            {
                if (_scoreColor.ContainsKey(Color.green))
                {
                    _scoreColor[Color.green] += 1;
                }

                RedEnemies.Remove(enemy);
            }
            else if (randomColor == Color.yellow)
            {
                if (_scoreColor.ContainsKey(Color.yellow))
                {
                    _scoreColor[Color.yellow] += 1;
                }

                RedEnemies.Remove(enemy);
            }
        }
    }

    public Dictionary<Color, int> GetColorPoints()
    {
        return _scoreColor;
    }

    public void ClearColorPoints() 
    {
        _scoreColor[Color.red] = 0;
        _scoreColor[Color.blue] = 0;
        _scoreColor[Color.green] = 0;
        _scoreColor[Color.yellow] = 0;
    }

    public void SetColorPoints(Dictionary<Color, int> newScoreColor)
    {
        _scoreColor = newScoreColor;
    }
}

