using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Graph;

namespace Bank.NET___backend.Authorization
{
    public class BankEmplyeeAuthorizationHandler : AuthorizationHandler<GroupAuthorizationRequirement>
    {

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirement requirement)
        {
            var claims = context.User.Claims.ToList();
            var EmailClaim = Helpers.GetClaim(claims, "emails");
            if (EmailClaim != null)
            {

                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
