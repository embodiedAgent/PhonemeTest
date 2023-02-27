using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.Events;

public class Server : MonoBehaviour
{
    public UnityEvent<string> onSpeak;
    public UnityEvent<string> onCommand;
    public UnityEvent<string,int> onExpression;

    public string port;
    IAsyncResult status;
    HttpListener server;

    private Queue<string> speakQueue;
    private Queue<string> commandQueue;
    // private string speakQueue;
    // private string commandQueue;
    // private string expressionQueue;
    // private int expressionValue;
    private void Start()
    {
        speakQueue = new Queue<string>();
        speakQueue.Clear();
        commandQueue = new Queue<string>();
        commandQueue.Clear();

        Application.runInBackground = true;
        server = new HttpListener();
        server.Prefixes.Add("http://localhost:"+port+"/");
        server.Start();
        status = server.BeginGetContext(HandleRequest, server);
    }

    private void Update()
    {
        if(speakQueue.Count > 0)
        {
            onSpeak.Invoke(speakQueue.Dequeue());
        }
        
        if(commandQueue.Count > 0)
        {
            onCommand.Invoke(commandQueue.Dequeue());
        }

        // if (!String.IsNullOrEmpty(speakQueue))
        // {
        //     onSpeak.Invoke(speakQueue);
        //     speakQueue = "";
        // }
        // if (!String.IsNullOrEmpty(commandQueue))
        // {
        //     onCommand.Invoke(commandQueue);
        //     commandQueue = "";
        // }
        // if (!String.IsNullOrEmpty(expressionQueue))
        // {
        //     onExpression.Invoke(expressionQueue,expressionValue);
        //     expressionQueue = "";
        // }
    }

    private void HandleRequest(IAsyncResult result)
    {
        Debug.Log("handling request");
        var server = (HttpListener)result.AsyncState;
        var context = server.EndGetContext(result);
        var request = context.Request;
        var response = context.Response;

        var requestBody = new StreamReader(request.InputStream).ReadToEnd();

        var json = JsonUtility.FromJson<CommandRequest>(requestBody);

        string speak = json.text;
        if (!String.IsNullOrEmpty(speak))
            speakQueue.Enqueue(speak);
        
        string command = json.command;
        if (!String.IsNullOrEmpty(command))
            commandQueue.Enqueue(command);

        // speakQueue = json.text;
        // commandQueue = json.command;
        // expressionQueue = json.expression;
        // expressionValue = json.expression_value;
        response.StatusCode = 200;
        response.ContentType = "text/plain";
        response.Close();
        Debug.Log("finished handling request");
        server.BeginGetContext(HandleRequest, server);
        return;
    }

    public class CommandRequest
    {
        public string command;
        public string text;
        public string expression;
        public int expression_value;
    }

    void OnDestroy(){
        server.Stop();
    }
}