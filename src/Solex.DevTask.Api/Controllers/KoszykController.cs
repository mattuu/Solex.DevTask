using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Interfaces;

namespace Solex.DevTask.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KoszykController : ControllerBase
    {
        private readonly IKoszykService _koszykService;

        private readonly ILogger<KoszykController> _logger;

        public KoszykController(ILogger<KoszykController> logger, IKoszykService koszykService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _koszykService = koszykService ?? throw new ArgumentNullException(nameof(koszykService));
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProduktModel>> Get()
        {
            var data = _koszykService.PobierzKoszyk();
            if (data?.Any() ?? false)
            {
                return Ok(data);
            }

            return NotFound();
        }
    }
}