using System.Text.Json;
using ApplicationStatusCode;
using DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WebCM.Utilities;
using SvcAudit;
using DomainModel;
namespace WebCM.Controllers
{
	[ApiController]
	public class TeamController : ControllerBase
	{
		private readonly SvcTeam _service;
		private readonly UserSession _userSession;

		public TeamController(SvcTeam teamService, UserSession userSession)
		{
			_service = teamService;
			_userSession = userSession;
		}

		[HttpGet("[controller]/details")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IEnumerable<TeamResponseDetails> GetDetails()
		{
            return _service.GetAllDetails();
        }


		[HttpGet("[controller]")]
		[Authorize(Roles = $"{AdfsRoles.userrole}")]
		public IEnumerable<ItemId> GetTeamsByClient([FromQuery] Guid clientid)
		{
            return _service.GetTeamsByClientId(clientid, _userSession.upn);
        }
		
		[HttpPost("[controller]")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IActionResult Create([FromBody] TeamRequest item)
		{
			AppStatusCode<Team> output = _service.Create(item);
			return StatusCode(output.ToHTTPCode(), output);

		}

		[HttpDelete("[controller]/{id:guid}")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IActionResult Delete(Guid id)
		{
			AppStatusCode<Team> output = _service.Delete(id);
			return StatusCode(output.ToHTTPCode(), output);
		}

		[HttpPut("[controller]/{id:guid}")]
        [Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult Update(Guid id, [FromBody] TeamRequest item)
		{
			AppStatusCode<Team> output = _service.Update(item, id);
			return StatusCode(output.ToHTTPCode(), output);
		}

		[HttpGet("[controller]/{id:guid}/members")]
        [Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult GetUserMembershipByTeamId([FromRoute] Guid id)
        {
            MembershipResponse output = _service.GetMembershipData(id);
            if (output == null)
            {

                return StatusCode(AppStatusCode<User>.ServiceUnavailable.ToHTTPCode(), AppStatusCode<User>.ServiceUnavailable);
            }
            return Ok(output);
        }

        [HttpPost("[controller]/{id:guid}/members")]
        [Authorize(Roles = $"{AdfsRoles.adminrole}")]
        public IActionResult SetUserMembershipByTeamId([FromRoute] Guid id, [FromBody] MembershipRequest membership)
        {
			AppStatusCode<Team> output = _service.SetMembershipData(membership);
            if (output == null)
            {
                return StatusCode(AppStatusCode<User>.ServiceUnavailable.ToHTTPCode(), AppStatusCode<User>.ServiceUnavailable);
            }
            return StatusCode(output.ToHTTPCode(),output);
        }
    }
}
