using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    public Transform playArea;
    public float speed;
    private float dir = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dir = Input.GetAxis("Horizontal");
        
        float limit = playArea.localScale.x * 0.5f * 10 - transform.localScale.x * 0.5f;
        float newX = transform.position.x + Time.deltaTime * speed * dir;
        newX = Mathf.Clamp(newX, -limit, limit);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
