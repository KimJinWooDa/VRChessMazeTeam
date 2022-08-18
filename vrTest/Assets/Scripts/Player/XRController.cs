using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class XRController : MonoBehaviour {
    /**<summary>��Ʈ�ѷ����� �÷��̾ ����Ű�� �������� ��� <c>Ray</c>�� ��ȯ�Ѵ�.</summary>
     * <param name="left">���̸� ���� ��Ʈ�ѷ�, �����̸� ������ ��Ʈ�ѷ�</param>
     */
    public Ray ControllerRay(bool left) {
        throw new System.NotImplementedException();
    }

    /**<summary>��Ʈ�ѷ� ���� ��ư Ŭ�� ���θ� ��ȯ�Ѵ�.</summary>
     * <param name="left">���̸� ���� ��Ʈ�ѷ�, �����̸� ������ ��Ʈ�ѷ�</param>
     */
    bool _controllerPressed;
    public bool ControllerPressed(bool left) { 
        switch (left)
        {
            //GetDown�� ��ư �ѹ� Ŭ������ ������ ����
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

    /**<summary>��Ʈ�ѷ��� ���̽�ƽ ������ ��ȯ�Ѵ�.</summary>
     * <param name="left">���̸� ���� ��Ʈ�ѷ�, �����̸� ������ ��Ʈ�ѷ�</param>
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
        //xr�� ���� �հ� ī�޶� �����̱�
    }

}
