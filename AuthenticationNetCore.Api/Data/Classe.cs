using System;
using System.Collections.Generic;

namespace AuthenticationNetCore.Api.Data
{
    public class Classe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public User Teacher { get; set; }
        public List<UserClasse> Students { get; set; }
    }
}