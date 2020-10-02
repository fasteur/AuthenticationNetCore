using System;
using System.Collections.Generic;

namespace AuthenticationNetCore.Api.Data
{
    public class Classe
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Teacher Teacher { get; set; }
        public List<StudentClasse> Students { get; set; }
    }
}