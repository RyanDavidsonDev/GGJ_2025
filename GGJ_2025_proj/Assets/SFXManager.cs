using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    [SerializeField] private AudioSource[] audioSources;
    private Stack<AudioSource> InactiveAudioSources = new Stack<AudioSource>();
    private List<AudioSource> ActiveAudioSources= new List<AudioSource>();



    public static SFXManager _instance;
    public static SFXManager Instance { get { return _instance; } }


    [SerializeField] public AudioClip PlayerHurt;
    [SerializeField] public AudioClip TurretHurt;


    // Start is called before the first frame update
    void Awake()
    {
        //is this the first time we've created this singleton
        if (_instance == null)
        {
            //we're the first gameManager, so assign ourselves to this instance
            _instance = this;

            //keep ourselves between levels
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            //if there's another one, then destroy this one
            Destroy(this.gameObject);

        }

        audioSources = gameObject.GetComponents<AudioSource>();
        foreach (AudioSource source in audioSources)
        {
            InactiveAudioSources.Push(source);
        }
        Debug.Log("size" + InactiveAudioSources.Count);
    }

    public void PlaySound(AudioClip clip)
    {
        if(ActiveAudioSources == null)
        {
            Debug.LogError("out of audio sources - too many sounds are playing consecutively - add more to sfxman");
            return;

        }

        AudioSource source = InactiveAudioSources.Pop();
        InactiveAudioSources.Push(source);
        source.clip = clip;

        source.Play();

        

    }
    private IEnumerator PlayAudioAndDeactivate(AudioSource source, AudioClip clip)
    {
        while (source.isPlaying)
        {
            yield return null;
        }
        ActiveAudioSources.Remove(source);
        InactiveAudioSources.Push(source);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
