using Microsoft.AspNetCore.Authorization;

namespace Undervisningsopgaver.Authorization;

// Requirement definerer HVAD vi kræver (ikke HOW)
public class MinimumAgeRequirement : IAuthorizationRequirement
{
    public int MinimumAge { get; }

    public MinimumAgeRequirement(int minimumAge)
    {
        MinimumAge = minimumAge;
    }
}