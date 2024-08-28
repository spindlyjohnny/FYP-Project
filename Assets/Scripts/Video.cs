using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Video : MonoBehaviour {
    public string videoName;
    public VideoPlayer videoPlayer;
    [SerializeField] RenderTexture BGTexture;
    // Start is called before the first frame update
    void Awake() {
        videoPlayer = GetComponent<VideoPlayer>();
        if (SceneManager.GetActiveScene().buildIndex == 0 && videoPlayer.targetTexture == BGTexture) PlayVideo();
    }

    // Update is called once per frame
    void Update() {
        print(videoName + videoPlayer.isPlaying);
    }
    public void PlayVideo() {
        if (videoPlayer) {
            print(videoPlayer.targetTexture);
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
            print(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}
