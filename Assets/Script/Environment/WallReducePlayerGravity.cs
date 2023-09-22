using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallReducePlayerGravity : MonoBehaviour {

    public float lastGravity;
    public float gravityFactor;
    public bool applied = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Wall! in");
            lastGravity = collision.gameObject.GetComponent<Rigidbody2D>().gravityScale;
        }
            
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (!applied && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Wall!");
            applied = true;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale /= gravityFactor;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Wall! out");
            applied = false;
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = lastGravity;
        }
    }
}
