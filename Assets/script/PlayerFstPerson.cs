using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerFstPerson : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private float jumpingForce;
    [SerializeField] float speed;
    //[SerializeField] float rotspeed = 500;
    enum eventInput {
        JUMP, // F
        GOUNDED, // T
        DOUBLE, // T
        NBR_OF_INPUT_CHECK,
    };

    private float horizontalIntput;
    private float verticalIntput;
    bool[] fixedUpdatChecker = new bool[(int)eventInput.NBR_OF_INPUT_CHECK];
    //Transform player_transform;
    Rigidbody rbody;
    // Start is called before the first frame update
    void Start()
    {
        fixedUpdatChecker[(int)eventInput.GOUNDED] = true;
        rbody =  GetComponent<Rigidbody>();
        //player_transform =  GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetButtonDown("Jump") &&(fixedUpdatChecker[(int)eventInput.GOUNDED] || 
        !fixedUpdatChecker[(int)eventInput.DOUBLE]))
            fixedUpdatChecker[(int)eventInput.JUMP] = true;
        horizontalIntput = Input.GetAxis("Horizontal");
        verticalIntput = Input.GetAxis("Vertical");
    }
    
    private Vector3 v;
    private void FixedUpdate() 
    {
        
        v.Set(horizontalIntput, 0, verticalIntput);

        for (int i = 0; i < (int)eventInput.NBR_OF_INPUT_CHECK; i++) {
            if (fixedUpdatChecker[i] == true) {
                if ((eventInput) i == eventInput.JUMP) {
                    
                    if (fixedUpdatChecker[(int)eventInput.GOUNDED]) {
                        rbody.AddForce(0,1 * jumpingForce, 0, ForceMode.VelocityChange);
                        fixedUpdatChecker[(int)eventInput.GOUNDED] = false;
                    }   
                    else {
                        fixedUpdatChecker[(int)eventInput.DOUBLE] = true;
                        rbody.AddForce(0,2 * jumpingForce, 0, ForceMode.VelocityChange);
                    }
                    fixedUpdatChecker[i] = false;
                    
                }
                //if ((eventInput) i == eventInput.?) {fixedUpdatChecker[i] = false}
               
            }
        }
        v.Normalize();
        v = v * speed;
        rbody.AddForce(v, ForceMode.VelocityChange);
        if (v != Vector3.zero) {
            Quaternion toRot = Quaternion.LookRotation(v, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRot, speed);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        fixedUpdatChecker[(int)eventInput.GOUNDED] = true;
        fixedUpdatChecker[(int)eventInput.DOUBLE] = false;
    }
}

