using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RealityEditor;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.UIElements;



public class GenerateScene : MonoBehaviour
{
  public TMP_InputField tMP_InputField;
    public RealityEditorManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager =FindObjectOfType<RealityEditorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Coroutine checkCoroutine;


    public void SendTextToserver(){

        OscMessage message= new OscMessage();

        message.address="/GenerateStory";

        message.values.Add(tMP_InputField.text);


        manager.osc.Send(message);

    String sId=TimestampGenerator.GetTimestamp();  
    checkCoroutine = StartCoroutine(CheckForJsonOnServer( manager.ServerURL+sId+"_StorySet.json",3f));

    }

    bool doneDownload;

   IEnumerator CheckForJsonOnServer(string jsonURL,float checkInterval)
    {
        while (true)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(jsonURL))
            {
                // Send the request and wait for the response
                yield return request.SendWebRequest();

                // Check if the file exists or there are any errors
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogWarning("JSON not found or error fetching JSON: " + request.error);
                }
                else
                {
                    // If the file exists, read and parse the JSON
                    string json = request.downloadHandler.text;
                //   InteractableDreamMesh jsonData = JsonUtility.FromJson<InteractableDreamMesh>(json);

                //     interactableDreamMesh= jsonData;

                    // Stop checking after successful reading
                    StopCheckingForJson();
                    yield break;
                }
            }

            // Wait for the specified interval before checking again
            yield return new WaitForSeconds(checkInterval);
        }
    }






        public void StopCheckingForJson()
    {
        if (checkCoroutine != null)
        {
            StopCoroutine(checkCoroutine);
            Debug.Log("Coroutine manually stopped.");
        }
    }


    IEnumerator managerGenerateTheSpot(){

        yield return new WaitForSeconds(0.5f);





        
    }




}
