using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
public class Names : MonoBehaviour
{
    //[SerializeField] TextAsset nameFile;
    //public string[] names;
    public string file;
    public string[] names;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadTextFromStreamingAssets());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator LoadTextFromStreamingAssets() {
        string path = Path.Combine(Application.streamingAssetsPath, file);
        UnityWebRequest www = UnityWebRequest.Get(path);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success) {
            string fileContents = www.downloadHandler.text;
            names = fileContents.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
        } else {
            Debug.LogError($"Failed to load file from StreamingAssets: {www.error}");
        }
    }
}
