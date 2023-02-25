namespace Asp_Net_API_Authenticatoin_With_Bearer_JWT.Models
{
    public class User : UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
