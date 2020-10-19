using Domain.Enums;
using Domain.Model;
using Domain.Sql;
using Microsoft.EntityFrameworkCore;
using Repositories.Sql;
using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using System.Collections.Generic;

namespace Services.Login
{
    public class LoginService : ILoginService
    {
        private readonly IDatabaseRepository<Person> personsRepository;
        private readonly IDatabaseRepository<LoginHistory> loginHistoryRepository;
        private readonly IDatabaseRepository<Group> groupRepository;

        public LoginService(IDatabaseRepository<Person> personsRepository, IDatabaseRepository<LoginHistory> loginHistoryRepository, IDatabaseRepository<Group> groupRepository)
        {
            this.personsRepository = personsRepository;
            this.loginHistoryRepository = loginHistoryRepository;
            this.groupRepository = groupRepository;
        }

        public async Task<(LoginResult, PersonModel)> Login(LoginRequestModel loginRequestModel)
        {
            try
            {
                var passwordSha = Security.CreateSha512Hash(loginRequestModel.Password);
                var person = await personsRepository.Find()
                    .Include(x => x.Group)
                    .Where(x =>
                        x.GroupId == loginRequestModel.GroupId &&
                        x.FirstName == loginRequestModel.FirstName &&
                        x.LastName == loginRequestModel.LastName &&
                        x.Password == passwordSha)
                    .Select(x => new PersonModel(x))
                    .FirstOrDefaultAsync();

                if (person == null)
                    return (LoginResult.PersonNotFound, null);
                else
                {
                    var loginHistory = new LoginHistory() 
                    { 
                        PersonId = person.Id, 
                        Host = loginRequestModel.Host, 
                        DateLogin = DateTime.UtcNow 
                    };
                    loginHistoryRepository.Add(loginHistory);
                    await loginHistoryRepository.SaveAsync();

                    return (LoginResult.OK, person);
                }
            }
            catch (Exception exp)
            {
                return (LoginResult.OtherError, null);
            }
        }

        public async Task<(LoginResult, PersonModel)> GetPersonAccessById(int personsId)
        {
            try
            {
                var result = await personsRepository.Find()
                    .Include(x => x.Group)
                    .Where(x => x.Id == personsId)
                    .Select(x => new PersonModel(x))
                    .FirstOrDefaultAsync();

                if (result != null)
                    return (LoginResult.OK, result);
                else
                    return (LoginResult.PersonNotFound, null);
            }
            catch (Exception exp)
            {
                return (LoginResult.OtherError, null);
            }
        }

        public Task<List<GroupModel>> GetPersonGroups()
        {
            return groupRepository.Find().OrderBy(x => x.Name).Select(x => new GroupModel(x)).ToListAsync();
        }

    }
}