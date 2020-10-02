using System.Threading.Tasks;
using AuthenticationNetCore.Api.Data;

namespace AuthenticationNetCore.Api.Repositories.ClasseRepo
{
    public interface IClasseRepository : IRepository<Classe>
    {
        // new Task<Classe> AddAsync(Classe entity);
    }
}