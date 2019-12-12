using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colleague : MonoBehaviour
{
    private Rigidbody2D mRb2D;

    [SerializeField]
    private float mSpeed;

    private Animator mAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator Movement()
    {
        WaitForSeconds moveTime = new WaitForSeconds(1);

        while(true)
        {
            // Set Direction
            int dir = Random.Range(0, 2);
            if(dir == 0)
            {
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            // Move or Stay
            int moveOrStay = Random.Range(0, 2);
            if (moveOrStay == 0)
            {
                mRb2D.velocity = Vector2.zero;
            }
            else
            {
                mRb2D.velocity = transform.right * -mSpeed;
            }
            // Wait
            yield return moveTime;
        }
    }

    private IEnumerator Function(float time)
    {
        WaitForSeconds term = new WaitForSeconds(time);

        while(true)
        {
            // wait
            yield return term;
            // run special function
        }
    }
}
