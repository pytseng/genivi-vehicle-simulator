using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class Logger : MonoBehaviour
{


    //Global variables
    private GameObject player;




    /**
    private Vector3 pos; //Position of player
    private Vector3 velocity; //Velocity of the player
    private float time; //Elapsed time since start of the game
    private int frame; //Elapsed number of frames since start of the game
    **/


    private Queue<string> databuffer = new Queue<string>(); //Data buffer queue
    private ArrayList rawData = new ArrayList(); //To store unformatted data values
    private string data; //Formatted data to put into the buffer queue
    private string sep = ";";


    private string path; //Location of the log file
    private StreamWriter logfile; //StreamWriter object to open the log file

    private Thread send; //Thread for sending the data over the network
    private bool isSending; //Boolean to control data sending
    byte[] msg;
    string server;
    int port;
    NetworkStream stream;
    TcpClient client;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Player");


        int epoch = (int)(System.DateTime.UtcNow - new System.DateTime(1970, 1, 1)).TotalSeconds; //Epoch Time

        path = @"C:\Users\Public\Documents\Unity Projects\Roll a Ball\Roll a Ball\Logs\" + epoch.ToString() + ".txt"; //Log file path

        //path_trig = @"C:\Users\Public\Documents\Unity Projects\Roll a Ball\Roll a Ball\Logs\" + epoch.ToString() + "trig.txt"; //Log file of triggers path;

        logfile = File.AppendText(path); //Creating and opening log file in append mode


        port = 23456;
        server = "10.132.129.197";
        try
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.
            client = new TcpClient(server, port);
            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            stream = client.GetStream();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log("ArgumentNullException: " + e.ToString());
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e.ToString());
        }

        data = Dataset1("D1", true);
        databuffer.Enqueue(data);
        data = "";
        isSending = true;
        send = new Thread(ContinuousDataSend);
        send.Start();

    }

    // Update is called once per frame
    void Update()
    {

        data = Dataset1("D1");
        databuffer.Enqueue(data);
        data = "";

        Vector3 test = TrackController.Instance.car.transform.position;

    }

    //Function to send the first element of the buffer queue
    void DataSend()
    {
        //check whether queue is empty
        if (databuffer.Count != 0)
        {
            string dat = databuffer.Dequeue();
            msg = System.Text.Encoding.ASCII.GetBytes(dat);
            WriteData(dat, logfile);
            stream.Write(msg, 0, msg.Length);
        }
        //if queue not empty, dequeue one element and send that data
    }

    void OnApplicationQuit()
    {
        isSending = false; //Stopping data sending from the independent thread

        /**Checking the status of data and buffer queue and sending
         * the remaining data 
         * **/
        if (!data.Equals("") || databuffer.Count != 0)
        {
            if (!data.Equals(""))
            {
                databuffer.Enqueue(data);
            }
            data = "";
            while (databuffer.Count != 0)
            {
                DataSend();
            }
        }
        stream.Close();
        client.Close();
        logfile.Close(); //Closing the logfile
        Debug.Log(GetFrame().ToString());
    }


    //Getting the velocity of the object
    Vector3 GetVelocity(GameObject obj)
    {
        return obj.transform.GetComponent<Rigidbody>().velocity;
    }

    //Getting the position of the object
    Vector3 GetPos(GameObject obj)
    {
        return obj.transform.position;
    }


    bool GetState(GameObject obj)
    {
        return obj.activeSelf;
    }

    //Getting the elapsed time
    float GetTime()
    {
        return Time.time;
    }


    //Getting the frame count
    int GetFrame()
    {
        return Time.frameCount;
    }

    string Dataset1(string encode, bool header = false)
    {
        rawData.Clear();
        string dat = encode + sep;
        if (header)
        {
            dat = dat + "Frame" + sep + "Time" + sep + "Position"+sep+"Velocity\r\n";
        }
        else
        {
            rawData.Add(GetFrame());
            rawData.Add(GetTime());
            rawData.Add(GetPos(player));
            rawData.Add(GetVelocity(player));
            dat = dat + FormatData(rawData, sep);
        }
        rawData.Clear();
        return dat;
    }






    //Function to format rawData
    string FormatData(ArrayList al, string separator)
    {
        string dat = "";
        for (int i = 0; i < al.Count; i++)
        {
            var elem = al[i];
            string el;
            if (!(elem is string)) el = elem.ToString();
            else { el = (string)elem; }

            if (i == al.Count - 1) dat = dat + el + "\r\n";
            else { dat = dat + el + separator; }
        }
        return dat;
    }

    //Function to write a string in a file
    void WriteData(string a, StreamWriter file)
    {
        file.Write(a);
    }

    //Function to continuously send data
    //This runs in an independent thread
    void ContinuousDataSend()
    {
        while (isSending)
        {
            DataSend();
        }
    }
}