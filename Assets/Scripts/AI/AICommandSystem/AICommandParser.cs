using System.Threading.Tasks;
using UnityEngine;

public class AICommandParser : MonoBehaviour
{
    public static AICommandParser Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<AICommand>


        Parse(string userPrompt)
    {

        string prompt =
$@"
Convert user request into JSON.

Valid actions:
Spawn
Delete
Move
Rotate
Scale

User:
{userPrompt}

Return ONLY JSON.

Example:

spawn dragon

{{
""action"":""Spawn"",
""targetObject"":""dragon""
}}

delete the dragon
{{
""action"":""Delete"",
""targetObject"":""dragon"",
""referenceObject"":"""",
""value"":0
}}

move the dragon 5 units forward
{{
""action"":""Move"",
""targetObject"":""dragon"",
""value"":5
}}

rotate the dragon 90 degrees
{{
""action"":""Rotate"",
""targetObject"":""dragon"",
""value"":90
}}

make dragon twice as big
{{
""action"":""Scale"",
""targetObject"":""dragon"",
""value"":2
}}

";
        string result =
            await GroqManager
            .Instance
            .Ask(prompt);

        result =
            result.Replace(
                "```json",
                "");

        result =
            result.Replace(
                "```",
                "");

        result =
            result.Trim();


        return JsonUtility
            .FromJson<AICommand>(
                result);
    }
}