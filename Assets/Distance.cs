using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distance : MonoBehaviour {
    public GameObject a;

    //public GameObject b;

    public Vector3 m;

    public Vector3 n;

    Vector3 o;
    Vector3 p;

    float timer_f = 0f;
    int timer_i = 0;
    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        m = a.transform.position;
        //n = b.transform.position;
        
        if (Vector3.Distance(m, n) < 6)
        {
            o = m;
        }
        //print(Vector3.Distance(m, n));
        if (Vector3.Distance(m, n) > 6) {
            this.transform.position = o;
            /*
            timer_f += Time.deltaTime;
            timer_i = (int)timer_f;
            if (timer_i == 5)
            {
                //this.transform.position = new Vector3(n.x, 0, n.z);
                this.transform.position = n;

            }
            Debug.Log(timer_i);
            */
        }
        if (Input.GetKey(KeyCode.R)) {
            this.transform.position = n;
        }
       


    }
    void OnTriggerEnter(Collider aaa) //aaa為自定義碰撞事件
    {
        if (aaa.gameObject.layer == 10) //如果aaa碰撞事件的物件名稱是wall
        {
            //print("OK");
            n = aaa.gameObject.transform.position;
           
            //Debug.Log(n);
        }
    }
}
