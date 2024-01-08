using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    // Use this for initialization
    public float speed;
    public Rigidbody rb;
    public Vector3 com;
    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
    }
    public float MoveSpeed = 3;
    public float RotateSpeed = 20;

    public GameObject camera1;
    public GameObject camera2;
    
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.K)) {
            if (camera1.activeSelf)
            {
                camera1.SetActive(false);
                camera2.SetActive(true);
            }
            else
            {
                camera1.SetActive(true);
                camera2.SetActive(false);
            }
        }
            if (this.transform.up.y > 0 && this.transform.up.y <= 10)
            if (Input.GetKey(KeyCode.W))
            {
                //print("Moving W");
                if (MoveSpeed <= 30)
                {
                    MoveSpeed = MoveSpeed + 10 * Time.deltaTime;
                }
                this.transform.Translate(Vector3.forward * Time.deltaTime * -MoveSpeed);
                if (Input.GetKey(KeyCode.A))
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                }

            }
            else if (Input.GetKey(KeyCode.S))
            {
                //print("MOving S");

                MoveSpeed = 5;
                this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
                if (Input.GetKey(KeyCode.D))
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
                }
                else if (Input.GetKey(KeyCode.A))
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                }
            }
            else if (Input.GetKey(KeyCode.A))
            {
                MoveSpeed = 3;
                //this.transform.Translate(Vector3.forward * Time.deltaTime * 1 * -MoveSpeed);
                this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                MoveSpeed = 3;
                //this.transform.Translate(Vector3.forward * Time.deltaTime * 1 * -MoveSpeed);
                this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
            }
    }
}


