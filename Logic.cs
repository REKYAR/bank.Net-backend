using Bank.NET___backend.Data;

namespace Bank.NET___backend
{
    public static class Logic
    {
        //returns null for request refused, height of installment for accepted
        public static decimal generateOffer(Request request)
        {
            Random random = new Random();
            decimal cost = (decimal)(3 *(random.NextDouble() + 1)/100);
            //request.Amount;
            //request.IncomeLevel;
            //request.NumberOfInstallments;
            //if (300*request.IncomeLevel < request.Amount * (1 + cost))
            //{
            //    return null;
            //}
            return Math.Round((request.Amount * (1 + cost))/request.NumberOfInstallments, 2);
        }
    }
}
