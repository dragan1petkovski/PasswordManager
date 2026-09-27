using Microsoft.AspNetCore.Authorization;
using ApplicationStatusCode;
using Services.Audit;
using Microsoft.AspNetCore.Mvc;
using DataTransferObjects.Client;
using Services;
using DomainModel;
using PasswordManager.Server.StaticObjects;

namespace WebCM.Controllers
{
	[ApiController]
	[ResponseCache(NoStore = true)]
	public class ClientController : ControllerBase
	{
		private readonly SvcClient _service;
		private readonly UserSession _userSession;



		public ClientController(SvcClient service, UserSession userSession)
		{
			_service = service;
			_userSession = userSession;
		}

		[HttpGet("[controller]/details")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IEnumerable<ClientDetailResponse> GetDetails()
		{
			return _service.GetClientDetailsResponse();

		}

		[HttpGet("[controller]")]
		[Authorize(Roles = $"{AdfsRoles.adminrole},{AdfsRoles.userrole}")]
		public IEnumerable<ClientResponse> Get()
		{
			Console.WriteLine($"userrole: {_userSession.email}");
			switch (_userSession.role)
			{
				case AdfsRoles.adminrole:
					return _service.GetClientResponse();

				case AdfsRoles.userrole:
					return _service.GetClientResponse(_userSession.upn);

				default:
					return null;
			}

		}

        [HttpPut("[controller]/{id:guid}")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IActionResult Update([FromBody] ClientRequest item, Guid id)
		{
			AppStatusCode<Client> output = _service.Update(item, id);
			return StatusCode(output.ToHTTPCode(), output);
		}

		[HttpDelete("[controller]/{id:guid}")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IActionResult Delete(Guid id)
		{
			AppStatusCode<Client> output = _service.Delete(id);
			return StatusCode(output.ToHTTPCode(), output);
		}

		[HttpPost("[controller]")]
		[Authorize(Roles = $"{AdfsRoles.adminrole}")]
		public IActionResult Create([FromBody] ClientRequest item)
		{
			AppStatusCode<Client> output = _service.Create(item);
			return StatusCode(output.ToHTTPCode(), output);
		}

    }
}
