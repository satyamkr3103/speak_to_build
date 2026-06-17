using System;
using System.Collections.Generic;

[Serializable]
public class SketchfabSearchResponse
{
    public List<SketchfabModel> results;
}

[Serializable]
public class SketchfabModel
{
    public string uid;

    public string name;

    public bool isDownloadable;

    public int likeCount;

    public int viewCount;

    public string description;

    public List<Tag> tags;
}

[Serializable]
public class Tag
{
    public string name;
}