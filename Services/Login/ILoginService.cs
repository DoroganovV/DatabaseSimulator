using Domain.Enums;
using Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Login
{
    public interface ILoginService 
    {
        Task<(LoginResult, PersonModel)> Login(LoginRequestModel loginRequestModel);
        Task<(LoginResult, PersonModel)> GetPersonAccessById(int personsId);
        Task<List<GroupModel>> GetPersonGroups();
    }
}