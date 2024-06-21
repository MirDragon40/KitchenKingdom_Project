using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioClip bgmClip;

    private AudioSource bgmSource;
    private Dictionary<string, GameObject> audioSources = new Dictionary<string, GameObject>();

    public static SoundManager Instance;
    public PhotonView PhotonView;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PhotonView = GetComponent<PhotonView>();
        // BGM AudioSource 설정
        GameObject bgmObject = new GameObject("BGM");
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.clip = bgmClip;
        bgmSource.Play();
    }

    public AudioClip GetAudioClip(string clipname)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == clipname)
                return clip;
        }
        return null;
    }

    public void PlayAudio(string clipName, bool loop = false)
    {
        // -> RPC       
        PhotonView.RPC("playAudio", RpcTarget.All, clipName, loop);
    }

    public void StopAudio(string clipName)
    {
        PhotonView.RPC("stopAudio", RpcTarget.All, clipName);
    }


    [PunRPC]
    public void playAudio(string clipName, bool loop = false)
    {
        if (!audioSources.ContainsKey(clipName))
        {
            GameObject soundObject = new GameObject(clipName + "AudioSource");
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(clipName);
            audioSource.loop = loop;
            audioSource.Play();
            audioSources.Add(clipName, soundObject);

            if (!loop)
            {
                StartCoroutine(DestroyAfterPlay(audioSource, soundObject));
            }
        }
    }

    private IEnumerator DestroyAfterPlay(AudioSource audioSource, GameObject soundObject)
    {
        yield return new WaitForSeconds(audioSource.clip.length);

        if (audioSource != null && soundObject != null)
        {
            audioSource.Stop();

            string clipName = audioSource.clip.name;
            audioSources.Remove(clipName);

            Destroy(soundObject);
        }
    }
    [PunRPC]
    public void stopAudio(string clipName)
    {
        if (audioSources.ContainsKey(clipName))
        {
            GameObject soundObject = audioSources[clipName];
            AudioSource audioSource = soundObject.GetComponent<AudioSource>();
            audioSource.Stop();
            audioSources.Remove(clipName);
            Destroy(soundObject);
        }
    }
    public bool IsPlaying(string clipName)
    {
        if (audioSources.ContainsKey(clipName))
        {
            AudioSource audioSource = audioSources[clipName].GetComponent<AudioSource>();
            return audioSource.isPlaying;
        }
        return false;
    }
}