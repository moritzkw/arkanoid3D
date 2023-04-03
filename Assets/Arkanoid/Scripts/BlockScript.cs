using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    public int value;
    public int resistance = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        if (resistance > 0)
        {
            resistance--;
        }
        else if (resistance == 0)
        {
            Destroy(gameObject);
            GameManager.instance.Score += value;
            GameManager.instance.BlockCounter--;
        }
    }
}
