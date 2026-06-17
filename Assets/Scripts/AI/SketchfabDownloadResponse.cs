using System;

[Serializable]
public class SketchfabDownloadResponse
{
    public DownloadSource source;
}

[Serializable]
public class DownloadSource
{
    public string url;
    public long size;
}