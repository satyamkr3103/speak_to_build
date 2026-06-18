using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSTTManager : MonoBehaviour
{
    public static GoogleSTTManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<string> Transcribe(
        byte[] wavData)
    {
        string url =
            $"https://speech.googleapis.com/v1/speech:recognize?key={APIKeys.GoogleSTTKey}";

        string audioBase64 =
            System.Convert.ToBase64String(
                wavData);

        GoogleSTTRequest requestBody =
            new GoogleSTTRequest();

        requestBody.config =
            new STTConfig();

        requestBody.audio =
            new STTAudio();

        requestBody.config.languageCode =
            "en-US";

        requestBody.config.encoding =
            "LINEAR16";

        requestBody.config.sampleRateHertz =
            16000;


        requestBody.audio.content =
            audioBase64;

        string json =
            JsonConvert.SerializeObject(
                requestBody);

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

        var operation =
            request.SendWebRequest();

        while (!operation.isDone)
        {
            await Task.Yield();
        }

        if(request.result !=
            UnityWebRequest.Result.Success)
        {
            Debug.LogError(
                request.error);

            Debug.LogError(
                request.downloadHandler.text);

            return null;
        }

        string response =
            request.downloadHandler.text;

        Debug.Log(
            response);

        GoogleSTTResponse stt =
            JsonConvert.DeserializeObject
            <GoogleSTTResponse>(
                response);

        if(
            stt == null ||
            stt.results == null ||
            stt.results.Length == 0)
        {
            return null;
        }

        return stt
            .results[0]
            .alternatives[0]
            .transcript;
    }
}