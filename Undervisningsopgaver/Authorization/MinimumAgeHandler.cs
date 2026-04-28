using Microsoft.AspNetCore.Authorization;

namespace Undervisningsopgaver.Authorization;
// Handler definerer HOW vi tjekker kravet
public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        MinimumAgeRequirement requirement)
    {
        // Find BirthDate-claim i tokenet
        var birthDateClaim = context.User.FindFirst("BirthDate");

        if (birthDateClaim is null)
            return Task.CompletedTask; // Ingen claim = krav ikke opfyldt

        // Beregn alder
        if (DateTime.TryParse(birthDateClaim.Value, out var birthDate))
        {
            var age = DateTime.Today.Year - birthDate.Year;
            // Korriger hvis fødselsdagen ikke er nået endnu i år
            if (birthDate.Date > DateTime.Today.AddYears(-age))
                age--;

            // Godkend hvis alder opfylder kravet
            if (age >= requirement.MinimumAge)
                context.Succeed(requirement); // ✅ Krav opfyldt!
        }

        return Task.CompletedTask;
    }
}