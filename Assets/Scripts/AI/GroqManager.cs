using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GroqManager : MonoBehaviour
{
    public static GroqManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    [System.Serializable]
    public class GroqRequest
    {
        public string model;
        public List<GroqMessage> messages;
    }

    [System.Serializable]
    public class GroqMessage
    {
        public string role;
        public string content;
    }

    public async Task<string> ExtractObjectName(
        string userPrompt)
    {
        string url =
            "https://api.groq.com/openai/v1/chat/completions";

        GroqRequest body =
            new GroqRequest
            {
                model =
                    "llama-3.3-70b-versatile",

                messages =
                    new List<GroqMessage>()
                    {
                        new GroqMessage
                        {
                            role = "system",
                            content =
@"You are an object extraction system.

Return only the requested object.

Examples:
spawn a giant dragon -> dragon
build a ladder -> ladder
create a red sports car -> sports car

Return ONLY the object name."
                        },

                        new GroqMessage
                        {
                            role = "user",
                            content = userPrompt
                        }
                    }
            };

        string json =
            JsonConvert.SerializeObject(
                body);

        UnityWebRequest request =
            new UnityWebRequest(
                url,
                "POST");

        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(
                json);

        request.uploadHandler =
            new UploadHandlerRaw(
                bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json");

        request.SetRequestHeader(
            "Authorization",
            "Bearer " +
            APIKeys.GroqApiKey);

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            Debug.LogError(
                request.downloadHandler.text);

            return null;
        }

        string responseJson =
            request.downloadHandler.text;

        Debug.Log(
            "Groq Response: " +
            responseJson);

        GroqResponse response =
            JsonConvert.DeserializeObject<
                GroqResponse>(
                responseJson);

        if (
            response == null ||
            response.choices == null ||
            response.choices.Count == 0)
        {
            Debug.LogError(
                "Groq Parse Failed");

            return null;
        }

        return response
            .choices[0]
            .message
            .content
            .Trim();
    }

    public async Task<string> Ask(
        string prompt)
    {
        string url =
            "https://api.groq.com/openai/v1/chat/completions";

        GroqRequest body =
            new GroqRequest
            {
                model =
                    "llama-3.3-70b-versatile",

                messages =
                    new List<GroqMessage>()
                    {
                        new GroqMessage
                        {
                            role = "user",
                            content = prompt
                        }
                    }
            };

        string json =
            JsonConvert.SerializeObject(
                body);

        UnityWebRequest request =
            new UnityWebRequest(
                url,
                "POST");

        byte[] bodyRaw =
            Encoding.UTF8.GetBytes(
                json);

        request.uploadHandler =
            new UploadHandlerRaw(
                bodyRaw);

        request.downloadHandler =
            new DownloadHandlerBuffer();

        request.SetRequestHeader(
            "Content-Type",
            "application/json");

        request.SetRequestHeader(
            "Authorization",
            "Bearer " +
            APIKeys.GroqApiKey);

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if (request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            Debug.LogError(
                request.downloadHandler.text);

            return null;
        }

        string responseJson =
            request.downloadHandler.text;

        GroqResponse response =
            JsonConvert.DeserializeObject<
                GroqResponse>(
                responseJson);

        return response
            .choices[0]
            .message
            .content
            .Trim();
    }
}