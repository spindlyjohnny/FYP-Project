using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Video : MonoBehaviour {
    [SerializeField] string videoName;
    public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start() {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update() {

    }
    public void PlayVideo() {
        if (videoPlayer) {
            string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoName);
            print(videoPath);
            videoPlayer.url = videoPath;
            videoPlayer.Play();
        }
    }
}
