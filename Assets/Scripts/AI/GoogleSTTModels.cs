using System;

[Serializable]
public class GoogleSTTRequest
{
    public STTConfig config;
    public STTAudio audio;
}

[Serializable]
public class STTConfig
{
    public string encoding;
    public int sampleRateHertz;
    public string languageCode;

    
}

[Serializable]
public class STTAudio
{
    public string content;
}

[Serializable]
public class GoogleSTTResponse
{
    public STTResult[] results;
}

[Serializable]
public class STTResult
{
    public STTAlternative[]
        alternatives;
}

[Serializable]
public class STTAlternative
{
    public string transcript;
}
