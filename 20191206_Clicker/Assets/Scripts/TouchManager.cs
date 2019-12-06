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
        Vector3 nearPlane = mTouchCamera.ScreenToWorldPoint(
                                new Vector3(screenPos.x, screenPos.y, mTouchCamera.nearClipPlane));
        Vector3 farPlane = mTouchCamera.ScreenToWorldPoint(
                                new Vector3(screenPos.x, screenPos.y, mTouchCamera.farClipPlane));
        return new Ray(nearPlane, farPlane - nearPlane);
    }

    // Touch가 눌렸는지를 가지고 오는 함수이다.
    public bool GetTouch()
    {
        // 여기로 들어왔다는 것은 터치를 진행하고 있다는 의미이다.
        // Update프레임 함수 내이기 때문에 이런 for문 방식이 가능해진다.
        // 유니티에서는 멀티터치가 최대 10개가 지원이 된다.
        for (int i = 0; i < Input.touchCount; i++)
        {
            // Touch라는 데이터가 있다.
            Touch touch = Input.GetTouch(i);
            // Touch의 상태를 보는 것을 알아야 한다.
            // GetKeyDown, GetKeyState, GetKeyUp처럼 Touch도 상태가 나뉘어져 있다.
            // touch.phase는 enum이다.
            // Began, Cancled, Ended, Moved, Stationaly 5종류의 상태가 있다.
            if (touch.phase == TouchPhase.Began)
            {
                // touch.position이 screen position이다.
                Ray ray = GenerateRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject == gameObject)
                    {
                        GameObject dummy = Instantiate(mDummy);
                        dummy.transform.position = hit.point;
                        // GameController.Touch();
                        // touch를 하고 나서 return을 해주어야지 그 다음 update를 하지 않는다.
                        return true;

                        // 강사님께서는 이 기능을 함수로 만들어서 bool로 return을 받아 update에서
                        // 터치를 했는지 안했는지 체크를 해준다.
                    }
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GenerateRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.gameObject == gameObject)
                {
                    GameObject dummy = Instantiate(mDummy);
                    dummy.transform.position = hit.point;
                    // GameController.Touch();
                }
            }
        }

        // touchCount : 유니티는 멀티터치 기능이 내장되어 있다.
        // touchCount가 0보다 크다는 것은 무엇인가 터치를 했다는 의미이다.
        //else if (Input.touchCount > 0)
        if (GetTouch())
        {
            // GameController.Touch();

            #region 20191206_touchCount
            //// 여기로 들어왔다는 것은 터치를 진행하고 있다는 의미이다.
            //// Update프레임 함수 내이기 때문에 이런 for문 방식이 가능해진다.
            //// 유니티에서는 멀티터치가 최대 10개가 지원이 된다.
            //for (int i = 0; i < Input.touchCount; i++)
            //{
            //    // Touch라는 데이터가 있다.
            //    Touch touch = Input.GetTouch(i);
            //    // Touch의 상태를 보는 것을 알아야 한다.
            //    // GetKeyDown, GetKeyState, GetKeyUp처럼 Touch도 상태가 나뉘어져 있다.
            //    // touch.phase는 enum이다.
            //    // Began, Cancled, Ended, Moved, Stationaly 5종류의 상태가 있다.
            //    if (touch.phase == TouchPhase.Began)
            //    {
            //        // touch.position이 screen position이다.
            //        Ray ray = GenerateRay(touch.position);
            //        RaycastHit hit;
            //        if (Physics.Raycast(ray, out hit))
            //        {
            //            if (hit.collider.gameObject == gameObject)
            //            {
            //                GameObject dummy = Instantiate(mDummy);
            //                dummy.transform.position = hit.point;
            //                // GameController.Touch();
            //                // touch를 하고 나서 return을 해주어야지 그 다음 update를 하지 않는다.
            //                return;

            //                // 강사님께서는 이 기능을 함수로 만들어서 bool로 return을 받아 update에서
            //                // 터치를 했는지 안했는지 체크를 해준다.
            //            }
            #endregion
        }
    }
}
