using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Models.Warrants;
using AviaSalesApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AviaSalesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarrantsController : ControllerBase
    {
        private readonly IWarrantsService _warrantsService;

        public WarrantsController(IWarrantsService warrantsService) => _warrantsService = warrantsService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarrantModel>>> WarrantsGetByIban([FromQuery] string iban)
        {
            var warrants = await _warrantsService.GetWarrantsByIbanAsync(iban);
            return Ok(warrants);
        }

        [HttpGet("{iban}/{warrantId}")]
        public async Task<ActionResult<WarrantModel>> WarrantGetById([FromRoute] string iban,
            [FromRoute] Guid warrantId)
        {
            var warrant = await _warrantsService.GetWarrantByIbanAndIdAsync(iban, warrantId);
            return Ok(warrant);
        }

        [HttpPost]
        public async Task<ActionResult<WarrantModel>> WarrantCreate([FromBody] WarrantCreateUpdateModel model)
        {
            var warrant = await _warrantsService.CreateWarrantAsync(model);
            return Ok(warrant);
        }

        [HttpPut("{iban}/{warrantId}")]
        public async Task<IActionResult> WarrantUpdate([FromRoute] string iban, [FromRoute] Guid warrantId, 
            [FromBody] WarrantCreateUpdateModel model)
        {
            await _warrantsService.UpdateWarrantAsync(iban, warrantId, model);
            return NoContent();
        }

        [HttpDelete("{iban}/{warrantId}")]
        public async Task<IActionResult> WarrantDelete([FromRoute] string iban, [FromRoute] Guid warrantId)
        {
            await _warrantsService.DeleteWarrantAsync(iban, warrantId);
            return NoContent();
        }
    }
}