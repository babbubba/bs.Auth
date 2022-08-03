using bs.Auth.Api.Models;
using bs.Auth.Api.Repositories;
using bs.Data.Helpers;
using bs.Data.Interfaces;
using bs.Datatable.Dtos;
using bs.Datatable.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bs.Auth.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Customers : ControllerBase
    {
        private readonly PaginatorService paginatorService;
        private readonly CustomersRepository customersRepository;
        private readonly IUnitOfWork unitOfWork;

        public Customers(PaginatorService paginatorService, CustomersRepository customersRepository, IUnitOfWork unitOfWork)
        {
            this.paginatorService = paginatorService;
            this.customersRepository = customersRepository;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("GetCustomers")]
        public IActionResult GetCustomers(PageRequest pageRequest)
        {
            var response = unitOfWork.RunInTransaction(() =>
            {
                return paginatorService.GetPage(pageRequest, customersRepository.GetCustomers());
            });
            return Ok(response);
        }

        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer(CustomerModel model)
        {
            var customerId = unitOfWork.RunInTransaction(() =>
            {
                return customersRepository.CreateCustomer(model);
            });
            return Ok(new Response<Guid>(customerId));
        }
    }
}
