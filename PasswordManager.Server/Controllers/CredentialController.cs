using ApplicationStatusCode;
using DataTransferObjects.Credential;
using DomainModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using PasswordManager.Server.StaticObjects;

namespace WebCM.Controllers
{
    [ApiController]
    [ResponseCache(NoStore = true)]
    public class CredentialController : ControllerBase
    {
        private readonly SvcCredential _service;


        public CredentialController(SvcCredential service)
        {
            _service = service;
        }


        [HttpGet("[controller]")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public IEnumerable<CredentialResponse> GetAll([FromQuery] Guid clientid)
        {
            return _service.GetAllCredentials(clientid);
        }

        [HttpPut("[controller]/{id:guid}")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public IActionResult Update([FromBody] CredentialRequest updateCredential, Guid id)
        {
            AppStatusCode<Credential> output = _service.Update(updateCredential, id);
            return StatusCode(output.ToHTTPCode(), output);
        }

        [HttpPost("[controller]")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public IActionResult Create([FromBody] CredentialRequest newCredential)
        {
            AppStatusCode<Credential> output = _service.Create(newCredential);
            return StatusCode(output.ToHTTPCode(), output);
        }

        [HttpGet("[controller]/password")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public IActionResult GetPassword([FromQuery] Guid teamid, [FromQuery] Guid id)
        {
            string password = _service.GetPassword(teamid, id);
            if (string.IsNullOrEmpty(password))
            {
                return NotFound();
            }
            return Ok(password);
        }

        [HttpGet("[controller]/key")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public FileResult Download([FromQuery] Guid teamid, [FromQuery] Guid id)
        {
            byte[] keyfile = _service.GetKey(teamid, id);
            Console.WriteLine(keyfile.Length);
            return File(keyfile, "text/plain; charset=utf-8", "keyfile.pem");
        }

        [HttpPost("[controller]/give")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public IActionResult GiveCredential([FromBody] GiveCredentialRequest newCredential)
        {
            return Ok();
        }

        [HttpDelete("[controller]")]
        [Authorize(Roles = AdfsRoles.userrole)]
        public IActionResult Delete([FromQuery] Guid teamid, [FromQuery] Guid id)
        {
            AppStatusCode<Credential> output = _service.Delete(teamid, id);
            return StatusCode(output.ToHTTPCode(), output);
        }
    }
}
