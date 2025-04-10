using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    static AudioManager current;

    [Header("��������")]
    public AudioClip ambientClip;
    public AudioClip musicClip;

    [Header("�����Ч")]
    public AudioClip[] walkStepClips;
    public AudioClip[] crouchStepClips;
    public AudioClip jumpClip;
    public AudioClip jumpVoiceClip;


    AudioSource ambientSource;
    AudioSource musicSource;
    AudioSource fxSource;
    AudioSource playerSource;
    AudioSource playerVoiceSource;

    private void Awake()
    {
        if(current == null)
        {
            current = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        fxSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        playerVoiceSource = gameObject.AddComponent<AudioSource>();

        StartLevelAudio();

    }

    private void StartLevelAudio()
    {
        current.ambientSource.clip = current.ambientClip;
        current.ambientSource.loop = true;
        current.ambientSource.Play();

        current.musicSource.clip = current.musicClip;
        current.musicSource.loop = true;
        current.musicSource.Play();
    }

    public static void PlayFootstepAudio()
    {
        int index = Random.Range(0, current.walkStepClips.Length);

        current.playerSource.clip = current.walkStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayCrouchFootstepAudio()
    {
        int index = Random.Range(0, current.crouchStepClips.Length);

        current.playerSource.clip = current.crouchStepClips[index];
        current.playerSource.Play();
    }

    public static void PlayJumpAudio()
    {
        current.playerSource.clip = current.jumpClip;
        current.playerSource.Play();

        current.playerVoiceSource.clip = current.jumpVoiceClip;
        current.playerVoiceSource.Play();
    }

}
