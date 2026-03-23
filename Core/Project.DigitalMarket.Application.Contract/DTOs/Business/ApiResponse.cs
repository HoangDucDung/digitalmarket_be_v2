using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DigitalMarket.Application.Contract.DTOs.Business
{
    public class ApiResponse<T>
    {
        public string Error { get; set; } = null;
        public T Data { get; set; }
    }
}
