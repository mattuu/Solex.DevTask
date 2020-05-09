using System;
using System.Collections.Generic;
using System.Text;
using Solex.DevTask.Domain;

namespace Solex.DevTask.Interfaces
{
    public interface IKoszykRepository
    {
        IEnumerable<Produkt> PobierzKoszyk();

        void DodajProduktDoKoszyka(int id, decimal ilosc);

        void UsunZKoszyka(int id);
    }
}
