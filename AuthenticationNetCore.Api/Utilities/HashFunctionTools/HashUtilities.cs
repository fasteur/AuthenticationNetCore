namespace AuthenticationNetCore.Api.Utilities.HashFunctionTools
{
    public static class HashUtilities
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                {
                    for (int i = 0; i < ComputeHash.Length; i++)
                    {
                        if (ComputeHash[i] != passwordHash[i])
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
        }
    }
}