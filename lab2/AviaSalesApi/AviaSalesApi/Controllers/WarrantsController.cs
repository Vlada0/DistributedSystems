using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AviaSalesApi.Models.Warrants;
using AviaSalesApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AviaSalesApi.Controllers
{
    [Route("api/[controller]")]
    public class WarrantsController : BaseController
    {
        private readonly IWarrantsService _warrantsService;

        public WarrantsController(IWarrantsService warrantsService) => _warrantsService = warrantsService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarrantModel>>> WarrantsGetByIban([FromQuery] string iban)
        {
            var warrants = await _warrantsService.GetWarrantsByIbanAsync(iban);
            return Ok(warrants);
        }

        [HttpGet("{warrantId}")]
        public async Task<ActionResult<WarrantModel>> WarrantGetById([FromRoute] Guid warrantId)
        {
            var warrant = await _warrantsService.GetWarrantByIdAsync(warrantId);
            return Ok(warrant);
        }

        [HttpPost]
        public async Task<ActionResult<WarrantModel>> WarrantCreate([FromBody] WarrantCreateUpdateModel model)
        {
            var warrant = await _warrantsService.CreateWarrantAsync(model);
            
            return CreatedAtAction(nameof(WarrantGetById), new {warrantId = warrant.Id}, warrant);
        }

        [HttpPut("{warrantId}")]
        public async Task<IActionResult> WarrantUpdate([FromRoute] Guid warrantId, 
            [FromBody] WarrantCreateUpdateModel model)
        {
            await _warrantsService.UpdateWarrantAsync(warrantId, model);
            return NoContent();
        }

        [HttpDelete("{warrantId}")]
        public async Task<IActionResult> WarrantDelete([FromRoute] Guid warrantId)
        {
            await _warrantsService.DeleteWarrantAsync(warrantId);
            return NoContent();
        }
    }
}