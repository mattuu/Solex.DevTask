using System.Collections.Generic;
using Solex.DevTask.Api;
using Solex.DevTask.Api.Models;

namespace Solex.DevTask.Interfaces
{
    public interface IKoszykService
    {
        void DodajProdukt(int id, decimal ilosc);

        void UsunProdukt(int id);

        IEnumerable<ProduktModel> PobierzKoszyk();

        decimal PobierzKoszykWartosc();
    }
}