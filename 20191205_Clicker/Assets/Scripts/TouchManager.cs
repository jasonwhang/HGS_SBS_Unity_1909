using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    [SerializeField]
    private Camera mTouchCamera;
    [SerializeField]
    private GameObject mDummy;

    public Ray GenerateRay(Vector3 screenPos)
    {
        
        // 복사 붙여넣기를 해도 되긴 하지만 위험성이 있기 때문에 new로 해준다.
        // 카메라의 최대 사정거리가 1000이 되어 있는데 현재 10만큼 땡겨져있으므로 현재 990이 나올 것이다.
        Vector3 farPlane = mTouchCamera.ScreenToWorldPoint(
                                    new Vector3(screenPos.x, screenPos.y, mTouchCamera.farClipPlane));

        //Vector3 nearPlane = new Vector3(worldPos.x, worldPos.y, mTouchCamera.nearClipPlane);
        Vector3 nearPlane = mTouchCamera.ScreenToWorldPoint(
                                    new Vector3(screenPos.x, screenPos.y, mTouchCamera.nearClipPlane));

        return new Ray(nearPlane, farPlane - nearPlane);
    }

    // Update is called once per frame
    void Update()
    {
        //Ray ray = new Ray();
        // 이것은 Editor에서 사용할 때 하는 기능이지 실제로는 Touch마우스기능을 따로 만들어야 한다.
        // 0 : 마우스 왼쪽클릭, 1 : 마우스 오른쪽클릭, 2 : 마우스 휠클릭
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = GenerateRay(Input.mousePosition);
            RaycastHit hit;

            //int layer = LayerMask.GetMask("Default", "Water");
            //int layer = LayerMask.GetMask("Default");
            //int layer = LayerMask.GetMask("Water");

            //Debug.Log(layer);

            // layermask를 1로 하자.
            //if(Physics.Raycast(ray, out hit, layer))
            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Find out Target");
                //Debug.Log(hit.collider.gameObject.name);
                // 같은 오브젝트인지를 검출하기
                if (hit.collider.gameObject == gameObject)
                {
                    // 부딫힌 좌표점
                    //Debug.Log(hit.point);

                    // Light.fx를 부딫힌 좌표점에 보여주게 한다.
                    GameObject dummy = Instantiate(mDummy);
                    dummy.transform.position = hit.point;
                    // GameController.Touch();
                }
            }
        }
    }
}
