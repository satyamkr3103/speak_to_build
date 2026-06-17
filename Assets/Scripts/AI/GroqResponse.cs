using System;
using System.Collections.Generic;

[Serializable]
public class GroqResponse
{
    public List<Choice> choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

[Serializable]
public class Message
{
    public string content;
}