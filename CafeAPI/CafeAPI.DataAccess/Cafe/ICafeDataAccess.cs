using CafeAPI.Core.Cafe;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CafeAPI.DataAccess.Cafe
{
    public interface ICafeDataAccess
    {
        public List<CafeDetails> GetAllCafeDetails(int locationId);
        public List<LocationDetails> GetLocations();
        public int InsertCafeDetails(CafeDetails CafeDetails, IFormFile logo);
        public bool UpdateCafeDetails(CafeDetails CafeDetails, IFormFile logo);
        public bool DeleteCafeDetails(int cafeId);
        public List<CafeDetails> GetAllCafeDropdown(int locationId);

    }
}
