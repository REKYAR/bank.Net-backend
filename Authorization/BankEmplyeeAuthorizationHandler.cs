using Azure.Core;
using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Authorization;

namespace Bank.NET___backend.Authorization
{
    public class BankEmplyeeAuthorizationHandler : AuthorizationHandler<GroupAuthorizationRequirement>
    {
        /*private readonly SqlContext _sqlContext;

        public BankEmplyeeAuthorizationHandler(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }*/

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirement requirement)
        {
            var claims = context.User.Claims.ToList();
            var EmailClaim = Helpers.GetClaim(claims, "emails");
            if (EmailClaim != null)
            {
                //var res = _sqlContext.Admins.Where( a => a.Email == EmailClaim.Value).First();
                /*var res = 5;
                if (res != null)
                {
                    
                }*/

                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
