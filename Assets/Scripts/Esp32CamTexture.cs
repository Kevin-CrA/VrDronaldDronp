using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;

public class Esp32CamTexture : MonoBehaviour
{
    private WebSocket _websocket;

    // Start is called before the first frame update
    async void Start()
    {
        _websocket = new WebSocket("ws://192.168.1.100:8888"); //here you can change the port and the ip address for the esp32cam
       // _websocket = new WebSocket("ws://192.168.100.4:8888"); //here you can change the port and the ip address for the esp32cam
        _websocket.OnOpen += () =>{
        Debug.Log("Connection open!");
        };
        _websocket.OnError += (e) =>{
        Debug.Log("Error! " + e);
        };
        _websocket.OnClose += (e) =>{
        Debug.Log("Connection closed!");
        };
        _websocket.OnMessage += (bytes) =>{
        // getting the message as a string
        /*
        Debug.Log("OnMessage length: " + bytes.Length);
        */
        if (bytes.Length>0){
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(bytes);

            if (GetComponent<Renderer>()!=null)
            {
                GetComponent<Renderer>().material.mainTexture = tex;
            }
        };
        };

        await _websocket.Connect();
        
        }

    // Update is called once per frame
    void Update()
    {
        _websocket.DispatchMessageQueue(); 
    }

    private void OnApplicationQuit()
    {
        _websocket.Close();
    }


}
