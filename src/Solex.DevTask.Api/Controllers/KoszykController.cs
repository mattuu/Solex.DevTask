using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Domain.Exceptions;
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

        [HttpPost]
        public IActionResult DodajProduktDoKoszyka(DodajProduktModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _koszykService.DodajProdukt(model.Id, model.Ilosc);

            return new StatusCodeResult(201);
        }

        [HttpDelete]
        public IActionResult UsunZKoszyka(int id)
        {
            try
            {
                _koszykService.UsunProdukt(id);
                return NoContent();
            }
            catch (ItemNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("total")]
        public IActionResult PobierzKoszykWartosc()
        {
            return Ok(_koszykService.PobierzKoszykWartosc());
        }
    }
}