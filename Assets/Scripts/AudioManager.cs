using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
public class AudioManager : MonoBehaviour {
    public static AudioManager instance;
    [SerializeField] AudioSource audio, sfxaudio; // one audio source for music, one for sfx
    public AudioClip levelmusic,titlemusic;
    // Start is called before the first frame update
    private void Awake() {
        if (instance == null) { // create singleton.
            instance = this;
            DontDestroyOnLoad(gameObject);
        } 
        else {
            Destroy(gameObject);
        }
    }
    public void StopMusic() {
        audio.Stop();
    }
    public void StopSFX() {
        sfxaudio.Stop();
    }
    public void PlayMusic(AudioClip music) {
        if (audio.isPlaying) return;
        audio.clip = music;
        audio.Play();
    }
    public void PlaySFX(AudioClip clip) {
        sfxaudio.PlayOneShot(clip);
    }
    public void ResumeMusic() {
        audio.UnPause();
    }
    public void PauseMusic() {
        audio.Pause();
    }
    public void PauseSFX() {
        sfxaudio.Pause();
    }
    public void ResumeSFX() {
        sfxaudio.UnPause();
    }
    public AudioClip CheckClip() { // for the purpose of checking the current audio clip being played at the start of a level
        return audio.clip;
    }
    public bool IsPlaying() {
        return audio.isPlaying;
    }
    void Start() {

    }
    public IEnumerator SwitchMusic(AudioClip music) {
        StopMusic();
        yield return new WaitForSeconds(.3f);
        PlayMusic(music);
    }
    // Update is called once per frame
    void Update() {
        if(SceneManager.GetActiveScene().buildIndex < 1) { // play title music on main menu. main menu buildindex is 0.
            PlayMusic(titlemusic);
        }
    }

}