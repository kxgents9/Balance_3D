using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Random = UnityEngine.Random;

public class GetDataWii : MonoBehaviour
{
    public static SerialPort rs = new SerialPort("COM9", 460800);
    public byte[] buftemp;
    public float RunTime;
    private float result;
    public int TotalRunTime = 10;
    private bool stop = false;
    private List<byte> InputData = new List<byte> { };
    private string DataReceived, Decode;
    private int signal_count, tick;
    private int decode_state;
    private int TotalDataCount = 0;
    private byte[] DATA = new byte[8];
    private double[] signal = new double[4];
    public BinDataSaveWii BinDataSaveWii;
    float total = 0;
    int cc = 0;
    public float MoveSpeed = 3;
    public float RotateSpeed = 20;
    float avg;

    void Start()
    {
        try
        {

            if (rs != null)
            {
                stop = false;
                rs.Close();
                rs.ReadBufferSize = 5000;
                rs.Open();
                rs.DiscardInBuffer();
                rs.DiscardOutBuffer();
                buftemp = new byte[10];
                signal_count = 1;
                tick = 1;
                decode_state = 1;
            }
            else
                Debug.Log("BluetoothPort Not Found");
        }
        catch
        {

        }

    }

    void Read_Data(byte input)
    {
        System.Convert.ToString(10, 16);
        int flag = 0;
        int i = 0;
        float Temp, LF = 0, LB = 0, RF = 0, RB = 0;
        if (decode_state == 1)
        {
            if (input == 0xFF)
                decode_state++;
        }
        else if (decode_state == 2)
        {
            if (input == 0xF0)
                decode_state++;
            else
                decode_state = 1;
        }
        else if (decode_state == 3)
        {
            DATA[0] = input;
            decode_state++;
        }
        else if (decode_state == 4)
        {
            DATA[1] = input;
            decode_state++;
        }
        else if (decode_state == 5)
        {
            DATA[2] = input;
            decode_state++;
        }
        else if (decode_state == 6)
        {
            DATA[3] = input;
            decode_state++;
        }
        else if (decode_state == 7)
        {
            DATA[4] = input;
            decode_state++;
        }
        else if (decode_state == 8)
        {
            DATA[5] = input;
            decode_state++;
        }
        else if (decode_state == 9)
        {
            DATA[6] = input;
            decode_state++;
        }
        else if (decode_state == 10)
        {
            DATA[7] = input;
            decode_state = 1;

            if (flag == 0)//右前
            {
                Temp = ((DATA[flag] << 8) + DATA[flag + 1]); //結合高低位元
                Temp = Temp * (float)3.3 / Mathf.Pow(2, 12); //矯正
                Temp = (float)6757.8 * Temp - 20057;
                if (Temp < 0) { Temp = 0; }
                LF = Temp;
                Decode += Temp.ToString("#0.000") + "  ";
                signal[i] = Temp;
                i++;
                flag += 2; //進入下一筆資料
            }
            if (flag == 2)//左前
            {
                Temp = ((DATA[flag] << 8) + DATA[flag + 1]); //結合高低位元
                Temp = Temp * (float)3.3 / Mathf.Pow(2, 12); //矯正
                Temp = (float)134.6 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 13) + 79 - (float)86.39 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 12) - (float)29.04 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 4) + (float)38.93 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 2);
                if (Temp < 0) { Temp = 0; }
                RF = Temp;
                Decode += Temp.ToString("#0.000") + "  ";
                signal[i] = Temp;
                i++;
                flag += 2; //進入下一筆資料
            }
            if (flag == 4)//右後
            {
                Temp = ((DATA[flag] << 8) + DATA[flag + 1]); //結合高低位元
                Temp = Temp * (float)3.3 / Mathf.Pow(2, 12); //矯正
                Temp = (float)6230.5 * Temp - 16303;
                if (Temp < 0) { Temp = 0; }
                LB = Temp;
                Decode += Temp.ToString("#0.000") + "  ";
                signal[i] = Temp;
                i++;
                flag += 2; //進入下一筆資料
            }
            if (flag == 6)//左後
            {
                Temp = ((DATA[flag] << 8) + DATA[flag + 1]); //結合高低位元
                Temp = Temp * (float)3.3 / Mathf.Pow(2, 12); //矯正
                Temp = (float)213.3 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 13) + (float)70.9 + (float)171.5 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 12) - (float)23.51 * (float)Math.Pow(Math.Log(Temp, Mathf.Exp(1)), 2);
                if (Temp < 0) { Temp = 0; }
                RB = Temp;
                Decode += Temp.ToString("#0.000") + "  ";
                signal[i] = Temp;
                i++;
                flag += 2; //進入下一筆資料
            }
            Decode += "\n";
        
            if (RF / 10 + LF / 10 - LB / 10 - RB / 10 > 8)
            {
                if (MoveSpeed <= 15)
                {
                    MoveSpeed = MoveSpeed + 5 * Time.deltaTime;
                }
                this.transform.Translate(Vector3.forward * Time.deltaTime * -MoveSpeed);
                if (LF / 10 + LB / 10 - RF / 10 - RB / 10 > 26)
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);

                }
                else if (RF / 10 + RB / 10 - LB / 10 - LF / 10 > -19)
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                }
            }
            if (LB / 10 + RB / 10 - LF / 10 - RF / 10 > 8)
            {
                MoveSpeed = 3;
                this.transform.Translate(Vector3.forward * Time.deltaTime * MoveSpeed);
                if (LF / 10 + LB / 10 - RF / 10 - RB / 10 > 26)
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * -RotateSpeed);
                }
                else if (RF / 10 + RB / 10 - LB / 10 - LF / 10 > -19)
                {
                    this.transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
                }
            }
            if (signal_count < 101)
            {
                signal_count++;
                tick++;

                for (int k = 0; k < 8; k++)
                {
                    BinDataSaveWii.SaveList.Add(DATA[k]);
                }
                for (int k = 0; k < 4; k++)
                {
                    BinDataSaveWii.SaveSignal.Add(signal[k]);
                }
            }
            else
            {
                BinDataSaveWii.BinSave(BinDataSaveWii.SaveList, "1_1_wii");
                BinDataSaveWii.BinSaveSignal(BinDataSaveWii.SaveSignal, "1_2_wii");
                signal_count = 1;
                print("OK");
            }

        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (!stop)
        {
            if (RunTime >= TotalRunTime)
            {
                rs.Close();
                stop = true;
                Debug.Log("Total Data Count:" + TotalDataCount);
                Debug.Log("RunTime:" + RunTime);
                Debug.Log("Data Receive : \n" + DataReceived);
                Debug.Log("解碼 : \n" + Decode);
                //Debug.Log("平均 : \n" + total/(TotalDataCount/10+1));
                //Debug.Log(cc);

                DataReceived = "";
            }
            else
            {
                RunTime = Time.time;
                if (rs.IsOpen)
                {
                    rs.Read(buftemp, 0, 10);
                    for (int n = 0; n < 10; n++)
                        Read_Data(buftemp[n]);
                }
                if (RunTime > 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        TotalDataCount++;
                        DataReceived += " " + buftemp[i].ToString("#000");
                        InputData.Add(buftemp[i]);
                    }

                    DataReceived += "\n";

                }
            }
        }
    }

}