using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Interfaces;

namespace Solex.DevTask.Services
{
    public class KoszykService : IKoszykService
    {
        private readonly ILogger<KoszykService> _logger;

        public KoszykService(ILogger<KoszykService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void DodajProdukt(int id, decimal ilosc)
        {
            throw new NotImplementedException();
        }

        public void UsunProdukt(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProduktModel> PobierzKoszyk()
        {
            throw new NotImplementedException();
        }

        public decimal PobierzKoszykWartosc()
        {
            throw new NotImplementedException();
        }
    }
}
