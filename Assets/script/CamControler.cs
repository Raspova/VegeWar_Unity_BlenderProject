using UnityEngine;
using System.Runtime.InteropServices;

public class CamControler : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;
    static private int Worldsize;
    //bool rtsmod = true;
    [SerializeField] private float speed = 8f;
    [SerializeField] int borderLimite = 15;
    private Vector3 mousePosRet;
    
    [DllImport("user32.dll")]
    static extern bool SetCursorPos(int X, int Y);


    float getMoveForce(int border, float target_value)
    {
        //Debug.Log(target_value);
        if (target_value < borderLimite)
            return -(borderLimite - target_value) / 10;
        if (target_value > (border - borderLimite))
            return -((border - borderLimite) - target_value) / 10;
        return 0;
    }

    void Update()
    {
        //    rtsmod = !rtsmod;
        if (Input.GetMouseButtonUp(2)) {
            SetCursorPos(Screen.width / 2, Screen.height / 2);
        }
            
    }

    
    bool IsMouseWithinWindow()
    {
        Vector3 mousePos = Input.mousePosition;
        return mousePos.x >= -10 && mousePos.x <= (Screen.width + 10) && mousePos.y >= -10 && mousePos.y <= Screen.height + 10;
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        //float rotAmount = 0;

        //if (!rtsmod)
        //{
        //    if (transform.position != (target.position + offset))
        //        transform.position = Vector3.Lerp(transform.position, target.position + offset, 5 * Time.deltaTime);
        //    return;
        //}
        if (Input.GetKeyDown(KeyCode.Tab)) {
            //50 - 150; MAX Y
            //50 - 60;  MAX ROTX
            int xrot = (int)( ((((transform.position.y) - 50f) / (150f - 50f)) * (80 - 60) ) + 60);
            transform.rotation = Quaternion.Euler(xrot, 0 , 0);
        }
        float modz = Input.GetAxis("Vertical") * speed;
        float modx = Input.GetAxis("Horizontal") * speed;
        float mody = Input.GetAxis("Mouse ScrollWheel") * speed * -300;
        //Vector3 newPos;

         if (!IsMouseWithinWindow())
                return;
        if (!Input.GetKey(KeyCode.Mouse2))
        {
            if (modz == 0)
                modz = getMoveForce(Screen.height, Input.mousePosition.y)* 2;
            if (modx == 0)
                modx = getMoveForce(Screen.width, Input.mousePosition.x) * 2;
            if (modx == 0 && mody == 0 && modz == 0)
                return;
            pos = new Vector3(pos.x + modx, pos.y + mody, pos.z + modz);
            pos.x = Mathf.Clamp(pos.x, -5, IntanciateWorld.Worldsize + 1);
            pos.z = Mathf.Clamp(pos.z, ((int)(-pos.y * 0.75)), IntanciateWorld.Worldsize + 1);
            pos.y = Mathf.Clamp(pos.y, 50, 160);
            if (mody == 0)
                transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
            else {
                transform.position = Vector3.Lerp(transform.position,new Vector3(transform.position.x, pos.y, transform.position.z),10 * Time.deltaTime);
                mousePosRet = Vector3.zero;
                int xrot = (int)( ((((transform.position.y) - 50f) / (150f - 50f)) * (80 - 60) ) + 60);
                transform.rotation = Quaternion.Euler(xrot, 0 , 0);
            }
        } else {
            //Debug.Log("-> pos" + pos);
            //Input.l  
            Vector3 rotBuff = mousePosRet - Input.mousePosition;
            if (mousePosRet == Vector3.zero) {
                mousePosRet = Input.mousePosition;
                return;
            }
            rotBuff.z = rotBuff.x;
            rotBuff.x = rotBuff.y;
            rotBuff.y = rotBuff.z;
            rotBuff.z = 0;
            
            rot *= Quaternion.Euler(rotBuff);//new Vector3(modz, modx, 0));
                                                 //rot *= Quaternion.Euler(Vector3.up * modx);
            mousePosRet = Input.mousePosition;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime);

        }
    }/**/
}

