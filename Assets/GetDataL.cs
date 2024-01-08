using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Random = UnityEngine.Random;

public class GetDataL : MonoBehaviour
{
    public static SerialPort rs = new SerialPort("COM5", 460800);
    public byte[] buftemp;
    public float RunTime;
    private float result;
    public int TotalRunTime = 5;
    private bool stop = false;
    private List<byte> InputData = new List<byte> { };
    private string DataReceived, Decode;
    private int signal_count, tick;
    private int decode_state;
    private int TotalDataCount = 0;
    private byte[] DATA = new byte[34];
    private double[] signal = new double[17];
    public BinDataSaveL BinDataSaveL;

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
                buftemp = new byte[36];
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
        float Temp;
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
            decode_state++;
        }
        else if (decode_state == 11)
        {
            DATA[8] = input;
            decode_state++;
        }
        else if (decode_state == 12)
        {
            DATA[9] = input;
            decode_state++;
        }
        else if (decode_state == 13)
        {
            DATA[10] = input;
            decode_state++;
        }
        else if (decode_state == 14)
        {
            DATA[11] = input;
            decode_state++;
        }
        else if (decode_state == 15)
        {
            DATA[12] = input;
            decode_state++;
        }
        else if (decode_state == 16)
        {
            DATA[13] = input;
            decode_state++;
        }
        else if (decode_state == 17)
        {
            DATA[14] = input;
            decode_state++;
        }
        else if (decode_state == 18)
        {
            DATA[15] = input;
            decode_state++;
        }
        else if (decode_state == 19)
        {
            DATA[16] = input;
            decode_state++;
        }
        else if (decode_state == 20)
        {
            DATA[17] = input;
            decode_state++;
        }
        else if (decode_state == 21)
        {
            DATA[18] = input;
            decode_state++;
        }
        else if (decode_state == 22)
        {
            DATA[19] = input;
            decode_state++;
        }
        else if (decode_state == 23)
        {
            DATA[20] = input;
            decode_state++;
        }
        else if (decode_state == 24)
        {
            DATA[21] = input;
            decode_state++;
        }
        else if (decode_state == 25)
        {
            DATA[22] = input;
            decode_state++;
        }
        else if (decode_state == 26)
        {
            DATA[23] = input;
            decode_state++;
        }
        else if (decode_state == 27)
        {
            DATA[24] = input;
            decode_state++;
        }
        else if (decode_state == 28)
        {
            DATA[25] = input;
            decode_state++;
        }
        else if (decode_state == 29)
        {
            DATA[26] = input;
            decode_state++;
        }
        else if (decode_state == 30)
        {
            DATA[27] = input;
            decode_state++;
        }
        else if (decode_state == 31)
        {
            DATA[28] = input;
            decode_state++;
        }
        else if (decode_state == 32)
        {
            DATA[29] = input;
            decode_state++;
        }
        else if (decode_state == 33)
        {
            DATA[30] = input;
            decode_state++;
        }
        else if (decode_state == 34)
        {
            DATA[31] = input;
            decode_state++;
        }
        else if (decode_state == 35)
        {
            DATA[32] = input;
            decode_state++;
        }
        else if (decode_state == 36)
        {
            DATA[33] = input;
            decode_state = 1;
            for (int j = 0; j < 3; j++)//加速度
            {
                Temp = ((DATA[flag] << 8) + DATA[flag + 1]); //結合高低位元
                Temp -= ((DATA[flag] & 0x80) == 0x80) ? 65536 : 0; //判斷是否為負值
                Temp = Temp / Mathf.Pow(2, 16) * 8; //矯正
                flag += 2; //進入下一筆資料
                Decode += Temp.ToString("#0.0000") + "  ";
                signal[i] = Temp;
                i++;
            }
            for (int j = 0; j < 3; j++)//陀螺儀
            {
                Temp = ((DATA[flag] << 8) + DATA[flag + 1]);
                Temp -= ((DATA[flag] & 0x80) == 0x80) ? 65536 : 0;
                Temp = Temp / Mathf.Pow(2, 16) * 4000;
                flag += 2;
                Decode += Temp.ToString("#0.0000") + "rad/s  ";
                signal[i] = Temp;
                i++;
            }
            for (int j = 0; j < 11; j++)//壓力點
            {
                float x;
                x = (float)3.5351;
                Temp = (DATA[flag] << 8) + DATA[flag + 1];
                double d1, d2;
                Temp = Temp * (float)3.3 / Mathf.Pow(2, 12); //先存TEMP看值 0~4096
                //d1 = -0.8105 * Mathf.Pow(Temp, 4) + 31.4866 * Mathf.Pow(Temp, 3) - 38.0747 * Mathf.Pow(Temp, 2) + 19.8825 * Temp - 2.0189;
                d2 = 10.3644 * Mathf.Pow((float)(Temp + 0.0165), x);
                flag += 2;
                Decode += d2.ToString("#0.0000") + "N  ";
                signal[i] = d2;
                i++;
            }
            Decode += "\n";

            if (signal_count < 101)
            {
                signal_count++;
                tick++;

                for (int k = 0; k < 34; k++)
                {
                    BinDataSaveL.SaveList.Add(DATA[k]);
                }
                for (int k = 0; k < 17; k++)
                {
                    BinDataSaveL.SaveSignal.Add(signal[k]);
                }
            }
            else
            {
                BinDataSaveL.BinSave(BinDataSaveL.SaveList, "1_1_L");
                BinDataSaveL.BinSaveSignal(BinDataSaveL.SaveSignal, "1_2_L");
                signal_count = 1;
                //print("OK");
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
                //Debug.Log("Total Data Count:" + TotalDataCount);
                //Debug.Log("RunTime:" + RunTime);
                //Debug.Log("Data Receive : \n" + DataReceived);
                Debug.Log("解碼L : \n" + Decode);
                DataReceived = "";
            }
            else
            {
                RunTime = Time.time;
                if (rs.IsOpen)
                {
                    rs.Read(buftemp, 0, 36);
                    for (int n = 0; n < 36; n++)
                        Read_Data(buftemp[n]);
                }
                if (RunTime > 0)
                {
                    for (int i = 0; i < 36; i++)
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