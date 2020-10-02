using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationNetCore.Api.Repositories.ClasseRepo
{
    public class ClasseRepository : GenericRepository<Classe>, IClasseRepository
    {
        public ClasseRepository(DataContext context) : base(context)
        {}
        // public override async Task<Classe> AddAsync(Classe entity)
        // {
        //     var data = await context.Classes.AddAsync(entity);
        //     return data.Entity;
        // }
    }
}