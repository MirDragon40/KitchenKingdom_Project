using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour

{
    public AudioClip[] audioClips;
    public AudioClip bgmClip;

    public AudioSource bgmSource;
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    private PhotonView _photonView;

    private void Awake()
    {
        // BGM AudioSource 설정
        GameObject bgmObject = new GameObject("BGM");
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.clip = bgmClip;
        bgmSource.Play();

        _photonView = GetComponent<PhotonView>();

        // PhotonView가 로컬 플레이어의 것이 아니라면 해당 GameObject를 비활성화합니다.
        if (!_photonView.IsMine)
        {
            gameObject.SetActive(false);
        }
    }

    public AudioClip GetAudioClip(string clipname)
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i].name == clipname)
                return audioClips[i];
        }
        return null;
    }

    public void PlayAudio(string clipName, bool loop = false)
    {
        if (!audioSources.ContainsKey(clipName))
        {
            GameObject soundObject = new GameObject(clipName);
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            audioSource.clip = GetAudioClip(clipName);
            audioSource.loop = loop;
            audioSources.Add(clipName, audioSource);
        }

        AudioSource source = audioSources[clipName];
        source.Play();
    }

    public void StopAudio(string clipName, bool loop = false)
    {
        if (audioSources.ContainsKey(clipName))
        {
            GameObject soundObject = new GameObject(clipName);
            AudioSource audioSource = soundObject.AddComponent<AudioSource>();
            AudioSource source = audioSources[clipName];
            audioSource.loop = loop;
            StartCoroutine(FadeOutAndDestroy(source, clipName));
        }
    }
    private IEnumerator FadeOutAndDestroy(AudioSource source, string clipName)
    {

        float fadeOutDuration = 0.1f;
        float startVolume = source.volume;


        while (source.volume > 0)
        {
            source.volume -= startVolume * Time.deltaTime / fadeOutDuration;
            yield return null;
        }

        // Ensure volume is zero and stop playback
        source.volume = 0;
        source.Stop();
        Destroy(source.gameObject);

        audioSources.Remove(clipName);
    }
    public void PlayFireSound()
    {
        PlayAudio("FireSound", true);
    }
    public void StopFireSound()
    {
        StopAudio("FireSound", false);
    }
}