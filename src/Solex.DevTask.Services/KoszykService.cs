﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Solex.DevTask.Api.Models;
using Solex.DevTask.Interfaces;

namespace Solex.DevTask.Services
{
    public class KoszykService : IKoszykService
    {
        private readonly ILogger<KoszykService> _logger;
        private readonly IKoszykRepository _koszykRepository;
        private readonly IMapper _mapper;

        public KoszykService(ILogger<KoszykService> logger, IKoszykRepository koszykRepository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _koszykRepository = koszykRepository ?? throw new ArgumentNullException(nameof(koszykRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public void DodajProdukt(int id, decimal ilosc)
        {
            _koszykRepository.DodajProduktDoKoszyka(id, ilosc);
        }

        public void UsunProdukt(int id)
        {
            _koszykRepository.UsunZKoszyka(id);
        }

        public IEnumerable<ProduktModel> PobierzKoszyk()
        {
            var produkty = _koszykRepository.PobierzKoszyk();
            return _mapper.Map<IEnumerable<ProduktModel>>(produkty);
        }

        public decimal PobierzKoszykWartosc()
        {
            var produkty = _koszykRepository.PobierzKoszyk();

            var total = 0m;
            foreach (var produkt in produkty)
            {
                total += produkt.Ilosc * (produkt.Ilosc > 10 ? 10 : 5);
            }

            return total;
        }
    }
}
