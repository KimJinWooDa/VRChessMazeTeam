using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class XRController : MonoBehaviour {
    /**<summary>컨트롤러에서 플레이어가 가리키는 방향으로 쏘는 <c>Ray</c>를 반환한다.</summary>
     * <param name="left">참이면 왼쪽 컨트롤러, 거짓이면 오른쪽 컨트롤러</param>
     */
    public Ray ControllerRay(bool left) {
        throw new System.NotImplementedException();
    }

    /**<summary>컨트롤러 검지 버튼 클릭 여부를 반환한다.</summary>
     * <param name="left">참이면 왼쪽 컨트롤러, 거짓이면 오른쪽 컨트롤러</param>
     */
    bool _controllerPressed;
    public bool ControllerPressed(bool left) { 
        switch (left)
        {
            //GetDown은 버튼 한번 클릭했을 때에만 반응
            case true:
                if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)) _controllerPressed = true;
                else _controllerPressed = false;
                break;
            default:
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) _controllerPressed = false;
                else _controllerPressed = false;
                break;
        }
        return _controllerPressed;
    }

    /**<summary>컨트롤러의 조이스틱 방향을 반환한다.</summary>
     * <param name="left">참이면 왼쪽 컨트롤러, 거짓이면 오른쪽 컨트롤러</param>
     */
    public Vector2 ControllerJoystick(bool left) {
        return left ? OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) : OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
    }

    private void Awake()
    {
       
    }


    void Start() {

    }

    
    void Update() {
        //xr에 따라 손과 카메라 움직이기
    }

}
