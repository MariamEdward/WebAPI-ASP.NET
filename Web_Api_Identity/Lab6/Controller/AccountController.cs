using Lab6.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Cors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lab6.Controller
{
    //allow anynonymous
  
    public class AccountController : ApiController
    {

        public async Task<IHttpActionResult> postUser(UserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //Usermanager
            AppDbContext dbcontext = new AppDbContext();
            UserStore<IdentityUser> userstore = new UserStore<IdentityUser>(dbcontext);
            UserManager<IdentityUser> manager = new UserManager<IdentityUser>(userstore);
            
            //Map from UserModel to IdentityUser
            IdentityUser user = new IdentityUser();
            user.UserName = userModel.Name;
            //user.PasswordHash = userModel.Password;

            //create 
            IdentityResult result = await manager.CreateAsync(user, userModel.Password);
            //ok
            if (result.Succeeded)
                return Created("", "User Added");
            //badrequ
            return BadRequest(result.Errors.ToList()[0]);
            
        }

    }
}
