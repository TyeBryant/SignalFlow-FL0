using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioChannel {

    public string name;
    public List<AudioSource> sources = new List<AudioSource>();
    public float volume;

    public AudioChannel(string name) {
        this.name = name;
    }

    public void Add(AudioSource source) {
        sources.Add(source);
    }
}

public class AudioManager : MonoBehaviour {
    public AudioClip defClip;
    
    // Singleton
    public static AudioManager _Instance;
    public static AudioManager Instance
    {
        get
        {
            if (_Instance != null)
            {
            }
            else
            {
                _Instance = new AudioManager();
            }
            return _Instance;
        }
    }

    public List<AudioChannel> channels = new List<AudioChannel>();
    public float masterVolume = 10;

    public void CreateChannel(string name) {
        AudioChannel channel = new AudioChannel(name);
        channels.Add(channel);
    }

    public AudioChannel GetChannel(string name) {
        foreach (AudioChannel channel in channels)
            if (channel.name == name)
                return channel;
        return null;
    }

    public void PlayClip(AudioClip clip, AudioChannel channel, float volume = 1, bool loop = false)
    {
        this.gameObject.AddComponent<AudioSource>();
        AudioSource source = this.gameObject.GetComponent<AudioSource>();
        channel.Add(source);
        Debug.Log(channel.name + clip.name + source.name);
        source.clip = clip;
        source.loop = loop;
        source.volume = masterVolume + volume + channel.volume;
        source.Play();
        if (!loop)
            Destroy(source, clip.length);
    }

    private void Awake()
    {
        _Instance = this;
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
        CreateChannel("SFX");
        CreateChannel("Music");
        CreateChannel("Ambient");
	}
}
