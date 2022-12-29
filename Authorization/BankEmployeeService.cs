using Bank.NET___backend.Data;
using Microsoft.EntityFrameworkCore;

namespace Bank.NET___backend.Authorization
{
    public class BankEmployeeService : IBankEmployeeService
    {
        private readonly SqlContext _sqlContext;

        public BankEmployeeService(SqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> IsBankEmployee(string userMail)
        {
            int count = await _sqlContext.Admins.Where(e => e.Email== userMail).CountAsync();

            if (count > 0)
                return true;

            return false;
        }
    }
}
