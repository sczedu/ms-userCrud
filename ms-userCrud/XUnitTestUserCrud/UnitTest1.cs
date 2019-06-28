using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ms_userCrud;
using ms_userCrud.Business;
using ms_userCrud.Controllers;
using ms_userCrud.Data;
using ms_userCrud.Models;
using ms_userCrud.Security;
using ms_userCrud.Security.SecurityClasses;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTestUserCrud
{
    public class UnitTest1
    {
        MysqlDBContext _mysqlDBContext;
        SigningConfigurations _signingConfigurations;
        TokenConfigurations _tokenConfigurations;

        [Fact]
        public async Task adasddas()
        {
            var services = new ServiceCollection();
            services.AddTransient<UserController, UserController>();
            services.AddTransient<PasswordService, PasswordService>();

            var serviceProvider = services.BuildServiceProvider();
            var userController = new UserController(_mysqlDBContext);
            var passwordService = new PasswordService(_signingConfigurations, _tokenConfigurations);

            var response = userController.Authentication(new User(), passwordService);

        }
    }
}
