using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] mOriginArr;

    // mOriginArr이 늘어나지는 않을 것인데 이것을 굳이 List로 하여 이중List를 사용할 필요는 없다.
    [SerializeField]
    private List<GameObject>[] mPool;

    protected void PoolSetUp()
    {
        // List[]를 초기화해준다.
        mPool = new List<GameObject>[mOriginArr.Length];

        for(int i = 0; i < mPool.Length; i++)
        {
            mPool[i] = new List<GameObject>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PoolSetUp();
    }

    public GameObject GetFromPool(int id)
    {
        // mPool[id]는 List이기 때문에 Length가 아닌 Count이다.
        for(int i = 0; i < mPool[id].Count; i++)
        {
            if(!mPool[id][i].gameObject.activeInHierarchy)
            {
                mPool[id][i].gameObject.SetActive(true);
                return mPool[id][i];
            }
        }

        // 숙제
    }

    // 숙제
}
