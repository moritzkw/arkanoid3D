using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    public float maxX;
    public float maxZ;
    public float speed;
    public Transform playArea;
    public Transform paddle;
    private Vector3 velocity;


    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "WallHorizontal":
                velocity.z *= -1;
                GetComponent<AudioSource>().Play();
                break;
            case "WallVertical":
                velocity.x *= -1;
                GetComponent<AudioSource>().Play();
                break;
            case "WallEliminate":
                GameManager.instance.EliminateBall(this.gameObject);
                GetComponent<AudioSource>().Play();
                break;
            case "Block 30":
                if (Random.value >= 0.5) GameManager.instance.BiggerPaddlePowerup(transform.position);
                goto case "Block 150";
            case "Block 50":
                if (Random.value >= 0.5) GameManager.instance.AddBallPowerup(other.transform.position);
                goto case "Block 150";
            case "Block 10":
            case "Block 150":
                other.GetComponent<BlockScript>().Hit();
                Vector3 colPoint = other.ClosestPoint(transform.position);
                float xDif = colPoint.x - transform.position.x;
                if (xDif < 0) xDif *= -1;
                float zDif = colPoint.z - transform.position.z;
                if (zDif < 0) zDif *= -1;

                if (xDif < zDif) velocity.z *= -1;
                else if (zDif < xDif) velocity.x *= -1;
                else
                {
                    velocity.x *= -1;
                    velocity.z *= -1;
                }
                break;
            case "Paddle":
                float maxDist = 0.5f * other.transform.localScale.x +
                    0.5f * transform.localScale.x;
                float actualDist = transform.position.x - other.transform.position.x;

                float distNorm = actualDist / maxDist;
                velocity.x = distNorm * maxX * speed;
                velocity.z *= -1;
                GetComponent<AudioSource>().Play();
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Status == GameManager.LevelStatus.RUNNING)
        {
            transform.position += velocity * Time.deltaTime;
        }
        else if (GameManager.instance.Status == GameManager.LevelStatus.NOT_STARTED)
        {
            transform.position = paddle.position + new Vector3(0, 0, 1);
        }
    }

    public void StartBall()
    {
        velocity = new Vector3((Random.value - 0.5f) * 1.2f, 0, maxZ * speed);
    }

    public void ResetBall()
    {
        velocity = new Vector3(0, 0, 0);
        transform.position = paddle.position + new Vector3(0, 0, 1);
    }
}
