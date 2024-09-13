using CafeAPI.Core.Cafe;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.Service.Cafe
{
    public interface ICafeService
    {
        public List<CafeDetails> GetAllCafeDetails(int locationId);

        public List<LocationDetails> GetLocations();

        public string InsertCafeDetails(CafeDetails CafeDetails, IFormFile logo);


        public string UpdateCafeDetails(CafeDetails CafeDetails, IFormFile logo);

        public string DeleteCafeDetails(int cafeId);

         public List<CafeDetails> GetAllCafeDropdown(int locationId);

    }
}
