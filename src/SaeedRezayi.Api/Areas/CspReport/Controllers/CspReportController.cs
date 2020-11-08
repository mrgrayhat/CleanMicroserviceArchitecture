using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DNTCommon.Web.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SaeedRezayi.Api.Areas.CspReport.Controllers
{
    //[Route("api/[controller]")]
    //public class CspReportController : ControllerBase
    //{
    //    [HttpPost("[action]")]
    //    [IgnoreAntiforgeryToken]
    //    public async Task<IActionResult> Log()
    //    {
    //        CspPost cspPost;
    //        using (var bodyReader = new StreamReader(this.HttpContext.Request.Body))
    //        {
    //            var body = await bodyReader.ReadToEndAsync().ConfigureAwait(false);
    //            this.HttpContext.Request.Body = new MemoryStream(Encoding
    //            .UTF8.GetBytes(body));
    //            cspPost = JsonConvert.DeserializeObject<CspPost>(body);
    //        }

    //        //TODO: log cspPost

    //        return Ok();
    //    }
    //}
}
