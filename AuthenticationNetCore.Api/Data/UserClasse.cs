using System;

namespace AuthenticationNetCore.Api.Data
{
    public class UserClasse
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ClasseId { get; set; }
        public Classe Classe { get; set; }
    }
}