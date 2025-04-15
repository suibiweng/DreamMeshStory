using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class StorySender : MonoBehaviour
{
    public string serverUrl = "http://192.168.0.139:5000/StoryGenerator";  // Change to your Flask server URL

    public void SendStory()
    {
        StartCoroutine(PostStoryCoroutine("A pig infront of WareHouse", "StoryTest"));
    }

    IEnumerator PostStoryCoroutine(string story, string urlid)
    {
        WWWForm form = new WWWForm();
        form.AddField("Story", story);
        form.AddField("URLID", urlid);

        using (UnityWebRequest request = UnityWebRequest.Post(serverUrl, form))
        {
            request.SetRequestHeader("Accept", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("[StoryGenerator] Response: " + request.downloadHandler.text);

                // Optional: Parse JSON result
                // JSONObject obj = new JSONObject(request.downloadHandler.text);
                // do something with obj
            }
            else
            {
                Debug.LogError("[StoryGenerator] Error: " + request.error);
            }
        }
    }
}
