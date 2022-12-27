namespace Bank.NET___backend.Models
{
    public enum APIConnectorActions
    {
        Continue,
        ShowBlockPage,

    }
    
    public abstract class BaseAPIConnectorResponse
    {
        public string version { get; set; }
        public string action { get; set; }

        public BaseAPIConnectorResponse(APIConnectorActions act)
        {
            version = "1.0.0";
            action = act.ToString();
        }
    }


    public class APIConnectorContinue: BaseAPIConnectorResponse
    {
        public APIConnectorContinue(): base(APIConnectorActions.Continue) { }
    }


    public class APIConnectorBlock: BaseAPIConnectorResponse
    {
        public string userMessage { get; set; }
        
        public APIConnectorBlock(string userMessage): base(APIConnectorActions.ShowBlockPage)
        {
            this.userMessage = userMessage;
        }
    }
}
