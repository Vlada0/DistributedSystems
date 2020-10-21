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

        public WarrantsController(IWarrantsService warrantsService)
        {
            _warrantsService = warrantsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WarrantModel>>> WarrantsGetByIban([FromQuery] string iban)
        {
            var warrants = await _warrantsService.GetWarrantsByIban(iban);
            return Ok(warrants);
        }

        [HttpGet("{iban}/{warrantId}")]
        public async Task<ActionResult<WarrantModel>> WarrantGetById([FromRoute] string iban,
            [FromRoute] Guid warrantId)
        {
            var warrant = await _warrantsService.GetWarrantByIbanAndId(iban, warrantId);
            return Ok(warrant);
        }

        public async Task<ActionResult<WarrantModel>> WarrantCreate([FromBody] WarrantCreateUpdateModel model)
        {
            var warrant = await _warrantsService.CreateWarrant(model);
            return CreatedAtRoute(nameof(WarrantGetById), new {iban = warrant.PassengerIban, warrantId = warrant.Id},
                warrant);
        }
    }
}