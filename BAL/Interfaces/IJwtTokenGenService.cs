public interface IJwtTokenGenService{
            string GenerateJwtToken(string userName, string role);
}