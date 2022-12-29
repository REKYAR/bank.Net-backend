namespace Bank.NET___backend.Authorization
{
    public interface IBankEmployeeService
    {
        Task<bool> IsBankEmployee(string userMail);
    }
}
