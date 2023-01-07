using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SpeechLib;

public class TTS : MonoBehaviour
{
    public string fileName;
    public UnityEvent onExpressionStart;
    public UnityEvent onExpressionStop;
    public AudioSource audioSource;
    private AudioClip audioClip;

    public int currentVoiceIndex;
    private SpVoice voice;
    private ISpeechObjectTokens voices;
    private SpFileStream objFSTRM;

    private Queue<string> queue;

    void Start()
    {
        queue = new Queue<string>();
        queue.Clear();
        objFSTRM = new SpFileStream();
        voice = new SpVoice();
        voices = voice.GetVoices();
        Debug.Log(voices.Item(currentVoiceIndex).GetDescription());
    }

    void Update()
    {
        if(queue.Count>0&&!audioSource.isPlaying)
        {
            speak(queue.Dequeue());
        }
    }

    public void AcceptSpeak(string text)
    {
        queue.Enqueue(text);
    }

    public void speak(string text)
    {
        onExpressionStop.Invoke();
        Debug.Log(voices.Item(currentVoiceIndex).GetDescription() +": "+ text);
        voice.Voice=voices.Item(currentVoiceIndex);
        objFSTRM.Open(Application.streamingAssetsPath + "/" + fileName, SpeechStreamFileMode.SSFMCreateForWrite, false);
        voice.AudioOutputStream = objFSTRM;
        voice.Speak(text);
        objFSTRM.Close();
        onExpressionStart.Invoke();
        StartCoroutine(LoadAudio());
    }

    private IEnumerator LoadAudio()
    {
        WWW request = GetAudioFromFile();
        yield return request;
        audioClip = request.GetAudioClip();
        PlayAudio();
    }

    private WWW GetAudioFromFile()
    {
        return new WWW("file://" + Application.streamingAssetsPath + "/" + fileName);
    }

    private void PlayAudio()
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
