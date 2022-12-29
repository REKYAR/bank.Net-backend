using Azure.Core;
using Bank.NET___backend.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Bank.NET___backend.Authorization
{
    public class GroupAuthorizationHandler : AuthorizationHandler<GroupAuthorizationRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GroupAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAuthorizationRequirement requirement)
        {
            var claims = context.User.Claims.ToList();
            var EmailClaim = Helpers.GetClaim(claims, "emails");
            if (EmailClaim == null)
            {
                return;                
            }

            using IServiceScope scope = _serviceScopeFactory.CreateScope();

            IBankEmployeeService bankEmployeeService = scope.ServiceProvider.GetRequiredService<IBankEmployeeService>();

            bool result = await bankEmployeeService.IsBankEmployee(EmailClaim.Value);
            if (result)
            {
                context.Succeed(requirement);
            }

            return;
        }
    }
}
