using System;

namespace AuthenticationNetCore.Api.Data
{
    public class StudentClasse
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; }
        public Guid ClasseId { get; set; }
        public Classe Classe { get; set; }
    }
}