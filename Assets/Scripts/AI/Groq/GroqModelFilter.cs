using System.Threading.Tasks;
using UnityEngine;

public class GroqModelFilter : MonoBehaviour
{
    public static GroqModelFilter Instance;

    private void Awake()
    {
        Instance = this;
    }

    public async Task<bool> IsValidCandidate(
        string targetObject,
        SearchCandidate candidate)
    {
        string prompt =
$@"User wants to spawn:

{targetObject}

Candidate Model:

Name:
{candidate.name}

Description:
{candidate.description}

Tags:
{string.Join(", ", candidate.tags)}

Can this model directly represent the requested object?

Examples:

Target: spaceship
Candidate: Spaceship Corridor
Answer: NO

Target: spaceship
Candidate: Light Fighter Spaceship
Answer: YES

Target: dragon
Candidate: Dragon Head
Answer: NO

Target: dragon
Candidate: Black Dragon
Answer: YES

Return ONLY:

YES

or

NO";

        string result =
            await GroqManager.Instance
                .Ask(prompt);

        Debug.Log(
            $"Candidate: {candidate.name}");

        Debug.Log(
            $"Groq Decision: {result}");

        return result
            .Trim()
            .ToUpper()
            .Contains("YES");
    }
}