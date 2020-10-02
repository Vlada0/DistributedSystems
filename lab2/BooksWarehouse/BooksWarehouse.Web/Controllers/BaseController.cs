using BooksWarehouse.Infrastructure;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable InconsistentNaming

namespace BooksWarehouse.Web.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
    }
}