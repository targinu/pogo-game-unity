using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private AudioSource musicAudioSource; //referência para o AudioSource da música
    private AudioSource[] sfxAudioSources; //array de referências para os AudioSources dos efeitos sonoros
    private float initialMusicVolume; //volume inicial da música

    void Start()
    {
        //obtém as referências dos AudioSources
        musicAudioSource = GameObject.Find("SoundManager").GetComponent<AudioSource>();
        sfxAudioSources = GameObject.Find("Players").GetComponentsInChildren<AudioSource>();

        //obtém o volume inicial da música
        initialMusicVolume = musicAudioSource.volume;

        //define os valores iniciais dos sliders
        musicSlider.value = initialMusicVolume;
        sfxSlider.value = sfxAudioSources[0].volume; //assumindo que o primeiro AudioSource é o que você deseja controlar com o slider.

        //define os métodos de callback dos sliders
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
    }

    //método para alterar o volume da música
    void ChangeMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    //método para alterar o volume dos efeitos sonoros
    void ChangeSFXVolume(float volume)
    {
        foreach (AudioSource sfxAudioSource in sfxAudioSources)
        {
            sfxAudioSource.volume = volume;
        }
    }
}
