using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class GroqSearchRanker : MonoBehaviour
{
    public static GroqSearchRanker Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<int> PickBestResult(
        string userObject,
        List<SearchCandidate> candidates)
    {
        StringBuilder prompt =
            new StringBuilder();

        prompt.AppendLine(
$@"The user wants to spawn:

{userObject}

Choose the BEST COMPLETE object.

Do NOT choose:
corridors
bridges
cockpits
engines
interiors
accessories
parts

Return ONLY the index number.");

        for (int i = 0; i < candidates.Count; i++)
        {
            prompt.AppendLine(
$@"
{i}

Name:
{candidates[i].name}

Likes:
{candidates[i].likes}

Views:
{candidates[i].views}

Tags:
{string.Join(", ",
    candidates[i].tags)}
");
        }

        string result =
            await GroqManager.Instance
                .Ask(
                    prompt.ToString());

        Debug.Log(
            "Groq Raw Response: " +
            result);

        Match match =
            Regex.Match(
                result,
                @"\d+");

        if(match.Success)
        {
            return int.Parse(
                match.Value);
        }

        return 0;
    }
}