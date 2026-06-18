using System.Collections.Generic;

[System.Serializable]
public class SearchCandidate
{
    public string uid;

    public string name;

    public int likes;

    public int views;

    public string description;

    public List<string> tags;
}