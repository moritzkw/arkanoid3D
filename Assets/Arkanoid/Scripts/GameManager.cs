using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private int lives = 3;
    private int currentLevel = 1;
    private List<GameObject> hearts = new List<GameObject>();
    private int blockCounter = 0;
    private List<GameObject> balls = new List<GameObject>();
    public TMP_Text scoreDisplay;
    public TMP_Text infoDisplay;
    public TMP_Text levelDisplay;
    public static GameManager instance;
    public GameObject block10Prefab;
    public GameObject block30Prefab;
    public GameObject block50Prefab;
    public GameObject block150Prefab;
    public GameObject powerup30Prefab;
    public GameObject powerup50Prefab;
    public GameObject ballPrefab;
    public GameObject heartAlive;
    public GameObject heartDead;
    public Transform paddle;

    public enum LevelStatus
    {
        NOT_STARTED,
        RUNNING,
        GAMEOVER,
        COMPLETED,
        WON
    };

    public int Score
    {
        set
        {
            score = value;
            updateScoreDisplay();
        }
        get
        {
            return score;
        }
    }

    public int BlockCounter
    {
        set
        {
            blockCounter = value;
            if (value == 0)
            {
                LevelCompleted();
            }
        }
        get
        {
            return blockCounter;
        }
    }

    public LevelStatus Status { set; get; } = LevelStatus.NOT_STARTED;

    public int Level
    {
        set 
        {
            if (currentLevel == 1 && value > 1)
            {
                Win();
            }
            else
            {
                currentLevel = value;
            }
        }
        get { return currentLevel; }
    }

    public int Lives
    {
        set
        {
            if (value < 0) return;
            else if (value < lives)
            {
                GameObject heartToRemove = hearts[hearts.Count - 1];
                Destroy(heartToRemove);
                hearts.Remove(heartToRemove);
                if (value == 0)
                {
                    GameOver();
                }
            }
            else if (value > lives)
            {
                hearts.ForEach(heart => {
                    Destroy(heart);
                    hearts.Remove(heart);
                });
                for (int i = 1; i <= value; i++)
                {
                    hearts.Add(Instantiate(heartAlive, new Vector3(12 + i * 3, 0.5f, 10), Quaternion.identity));
                }
            }
            lives = value;
        }
        get
        {
            return lives;
        }
    }

    void updateScoreDisplay()
    {
        scoreDisplay.text = score.ToString();
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        InitLevel(1);
        for (int i = 1; i <= 3; i++)
        {
            hearts.Add(Instantiate(heartAlive, new Vector3(12 + i * 3, 0.5f, 10), Quaternion.identity));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            infoDisplay.gameObject.SetActive(false);
            if (Status == LevelStatus.COMPLETED)
            {
                InitLevel(++currentLevel);
            }
            else if (Status == LevelStatus.NOT_STARTED && lives > 0)
            {
                Status = LevelStatus.RUNNING;
                Debug.Log(paddle.position);
                balls.ForEach(ball => ball.GetComponent<BallScript>().StartBall());
            }
            else if (Status == LevelStatus.GAMEOVER)
            {
                Status = LevelStatus.RUNNING;
                Score = 0;
                Lives = 3;
                InitLevel(currentLevel);
                balls.ForEach(ball => ball.GetComponent<BallScript>().StartBall());
            }
            else if (Status == LevelStatus.WON)
            {
                Status = LevelStatus.RUNNING;
                Score = 0;
                Lives = 3;
                currentLevel = 1;
                InitLevel(currentLevel);
                balls.ForEach(ball => ball.GetComponent<BallScript>().StartBall());
            }
        }
    }

    private void InitLevel(int level)
    {
        Status = LevelStatus.NOT_STARTED;
        levelDisplay.text = "Level " + level;
        AddBall();

        switch (level)
        {
            case 1:
                AddBlock(block150Prefab, 0, 12);
                AddBlock(block50Prefab, 0, 10);
                AddBlock(block30Prefab, 0, 8);
                AddBlock(block10Prefab, 0, 6);
                break;
            case 2:
                for (int i = -8; i <= 8; i = i + 4)
                {
                    AddBlock(block150Prefab, i, 10);
                    AddBlock(block50Prefab, i, 8);
                    AddBlock(block30Prefab, i, 6);
                    AddBlock(block10Prefab, i, 4);
                }
                break;
            case 3:
                AddBlock(block50Prefab, -7.5f, 10);
                AddBlock(block150Prefab, -5, 10);
                AddBlock(block10Prefab, -2.5f, 10);
                AddBlock(block30Prefab, 0, 10);
                AddBlock(block50Prefab, 2.5f, 10);
                AddBlock(block150Prefab, 5, 10);
                AddBlock(block10Prefab, 7.5f, 10);

                AddBlock(block150Prefab, -7.5f, 8);
                AddBlock(block10Prefab, -5, 8);
                AddBlock(block30Prefab, -2.5f, 8);
                AddBlock(block50Prefab, 0, 8);
                AddBlock(block150Prefab, 2.5f, 8);
                AddBlock(block10Prefab, 5, 8);
                AddBlock(block30Prefab, 7.5f, 8);

                AddBlock(block10Prefab, -7.5f, 6);
                AddBlock(block30Prefab, -5, 6);
                AddBlock(block50Prefab, -2.5f, 6);
                AddBlock(block150Prefab, 0, 6);
                AddBlock(block10Prefab, 2.5f, 6);
                AddBlock(block30Prefab, 5, 6);
                AddBlock(block50Prefab, 7.5f, 6);
                break;
        }
    }

    private void AddBlock(GameObject prefab, float x, float z)
    {
        Instantiate(prefab, new Vector3(x, 0.5f, z), Quaternion.identity);
        blockCounter++;
    }

    private void GameOver()
    {
        Status = LevelStatus.GAMEOVER;
        currentLevel = 1;
        balls.ForEach(ball => Destroy(ball));
        balls.Clear();
        infoDisplay.text = "GAME OVER!\nPress SPACE to restart";
        infoDisplay.gameObject.SetActive(true);

    }

    private void LevelCompleted()
    {
        if (currentLevel == 3)
        {
            Win();
            return;
        }
        Status = LevelStatus.COMPLETED;
        balls.ForEach(ball => Destroy(ball));
        balls.Clear();
        infoDisplay.text = "Level " + this.currentLevel + " completed!\nPress SPACE to continue";
        infoDisplay.gameObject.SetActive(true);

    }

    private void Win()
    {
        Status = LevelStatus.WON;
        balls.ForEach(ball => Destroy(ball));
        balls.Clear();
        infoDisplay.text = "YOU WON!!!\nPress SPACE to play again";
        infoDisplay.gameObject.SetActive(true);
    }

    public void BiggerPaddlePowerup(Vector3 powerupPos)
    {
        GameObject powerup = Instantiate(powerup30Prefab, new Vector3((float)powerupPos.x, (float)powerupPos.y, (float)powerupPos.z), Quaternion.identity);
    }

    public void AddBallPowerup(Vector3 powerupPos)
    {
        GameObject powerup = Instantiate(powerup50Prefab, powerupPos, Quaternion.identity);
    }

    public void ApplyPowerup(string tag)
    {
        switch (tag)
        {
            case "Powerup30":
                paddle.localScale = new Vector3(6, 1, 0.5f);
                Invoke(nameof(ResetPaddleSize), 10f);
                break;
            case "Powerup50":
                AddBall().GetComponent<BallScript>().StartBall();
                break;
        }
    }

    private void ResetPaddleSize()
    {
        paddle.localScale = new Vector3(3, 1, 0.5f);
    }

    private GameObject AddBall()
    { 
        GameObject newBall = Instantiate(ballPrefab, paddle.position + new Vector3(0, 0, 1), Quaternion.identity);
        balls.Add(newBall);
        newBall.GetComponent<BallScript>().paddle = paddle;
        newBall.GetComponent<BallScript>().playArea = this.transform;
        return newBall;
    }

    public void EliminateBall(GameObject ball)
    {
        if (balls.Count == 1)
        {
            Status = LevelStatus.NOT_STARTED;
            Lives--;
            ball.GetComponent<BallScript>().ResetBall();

        }
        else if (balls.Count > 1)
        {
            balls.Remove(ball);
            Destroy(ball);
        }
        Debug.Log(balls.Count + " " + lives);
    }
}
