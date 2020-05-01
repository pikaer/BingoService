namespace Bingo.Model.Contract
{
    public class LoginRequest
    {
        public string Code { get; set; }
    }

    public class LoginResponse
    {
        public long UId { get; set; }
    }
}
