using ApplicationStatusCode;
using DataTransferObjects.Membership;
using DataTransferObjects.User;
using DomainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using PasswordManager.Server.StaticObjects;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using DataTransferObjects.Adfs;

namespace PasswordManager.Server.Controllers
{
	[ApiController]
	public class UserController	: ControllerBase
	{
        private readonly SvcUser _service;
		private readonly IServiceProvider _serviceProvider;


		public UserController (SvcUser service, IServiceProvider serviceProvider)
		{
			_service = service;
			_serviceProvider = serviceProvider;
        }

        [HttpGet("/[controller]/me")]
        [Authorize(Roles = $"{AdfsRoles.userrole}, {AdfsRoles.adminrole}")]
        public IActionResult Me([FromServices] IMemoryCache _cache)
        {
            if (Guid.TryParse(this.Request.Cookies["_id"], out Guid tokenId))
            {
                UserSession _userSession = _cache.Get<UserProfileDetails>(tokenId).userSession;
                UserProfile profile = new UserProfile()
                {
                    display_name = $"{_userSession.givenname} {_userSession.surname}",
                    email = _userSession.email,
                    surname = _userSession.surname,
                    role = _userSession.role,
                    given_name = _userSession.givenname

                };
                return Ok(profile);
            }
            return NotFound();
        }


        [HttpPost("[controller]")]
        [Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult ADUserSync()
		{
			SvcLdapSync ldapSync = _serviceProvider.GetRequiredService<SvcLdapSync>();
			AppStatusCode<User> response = ldapSync.ADUserSync();

            return StatusCode(response.ToHTTPCode(), response);

        }

		[HttpGet("[controller]")]
        [Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult GetAllUsers()
		{
			IEnumerable<UserResponse> output = _service.GetAllUsers();
			if(output == null)
			{
				return StatusCode(AppStatusCode<User>.ServiceUnavailable.ToHTTPCode(), AppStatusCode<User>.ServiceUnavailable);
			}
			return StatusCode(200, output);
		}
		[HttpGet("[controller]/{id:guid}/members")]
        [Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult GetTeamMembershipByUserId([FromRoute] Guid id)
		{
			MembershipResponse output =  _service.GetMembershipData(id);
			if (output == null)
			{ 
				return StatusCode(AppStatusCode<User>.ServiceUnavailable.ToHTTPCode(), AppStatusCode<User>.ServiceUnavailable);
			}
			return StatusCode(200,JsonSerializer.Serialize(output));
		}



        [HttpPost("[controller]/{id:guid}/members")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult SetTeamMembershipByUserId([FromRoute] Guid id, [FromBody] MembershipRequest membership)
        {
			AppStatusCode<User> output = _service.SetMembershipData(membership);
            if (output == null)
            {
                return StatusCode(AppStatusCode<User>.ServiceUnavailable.ToHTTPCode(), AppStatusCode<User>.ServiceUnavailable);
            }
            return StatusCode(output.ToHTTPCode(),output);
        }
    }
}
