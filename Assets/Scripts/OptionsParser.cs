using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class OptionsParser : MonoBehaviour {
    private List<string> protoOptions = new List<string>();
    private List<string> options = new List<string>();
    public List<string> myOptions = new List<string>();

    void Start() {
        string filePath = "Assets\\Misc\\options.txt";
        if (File.Exists(filePath)) {
            protoOptions = new List<string>(File.ReadAllLines(filePath));
            ParseOptions();
        } else {
            Debug.LogError("File not found: " + filePath);
        }
        print(myOptions[0]);
    }

    private void ParseOptions() {
        int startQuote = 0;
        int endQuote = 1;

        List<int> quoteIndexes = ListDuplicatesOf(protoOptions, '"');

        while (endQuote <= quoteIndexes.Count) {
            options = protoOptions.GetRange(quoteIndexes[startQuote], quoteIndexes[endQuote] - quoteIndexes[startQuote] + 1);
            string temp = StringBuilder(options);
            myOptions.Add(temp);
            temp = "";
            options.Clear();
            startQuote += 2;
            endQuote += 2;
        }
    }

    private List<int> ListDuplicatesOf(List<string> seq, char item) {
        List<int> locs = new List<int>();
        int startAt = -1;

        while (true) {
            try {
                int loc = seq.IndexOf(item.ToString(), startAt + 1);
                locs.Add(loc);
                startAt = loc;
            } catch (System.Exception) {
                break;
            }
        }

        return locs;
    }

    private string StringBuilder(List<string> list) {
        string result = "";
        foreach (string item in list) {
            result += item;
        }
        return result;
    }
}