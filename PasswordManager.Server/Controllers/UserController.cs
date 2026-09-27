using ApplicationStatusCode;
using DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.Text.Json;
using WebCM.Utilities;
using DomainModel;

namespace WebCM.Controllers
{
	public class UserController	: ControllerBase
	{
		private readonly SvcUser _service;
		private readonly IServiceProvider _serviceProvider;


		public UserController (SvcUser service, IServiceProvider serviceProvider)
		{
			_service = service;
			_serviceProvider = serviceProvider;
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
