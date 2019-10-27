using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using OriginService.Data.Repositories.Interfaces;
using OriginService.Model.Models;
using zipkin4net;
using OperationStatus = OriginService.Model.Models.OperationStatus;

namespace OriginService.Api.Controllers {
    [Route ("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase {
        private readonly ICustomerRepository _customerRepository;


        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        private static List<Customer> MEM_DB = new List<Customer>();

        // GET api/customers/5
        [HttpGet ("{id}")]
        public async Task<IActionResult> GetById (int id)
        {
            var trace = Trace.Current;
            
            //trace.Record(Annotations.ServerRecv());
//            trace.Record(Annotations.ServiceName(nameof(CustomersController)));
            //trace.Record(Annotations.Rpc("GET"));
            //trace.Record(Annotations.ServerSend());
            trace.Record(Annotations.Tag("Actions", "Customers.GetById"));
            trace.Record(Annotations.Event("GetById.BeforeDB"), DateTime.Now);
            
            var customerEntity = await _customerRepository
                .GetCustomerById(id)
                .ConfigureAwait(false);

            trace.Record(Annotations.Event("GetById.AfterDB"), DateTime.Now);
            
            if (customerEntity == null)
            {
                return NotFound(new OperationStatus($"Customer {id} not found"));
            }

            trace.Record(Annotations.Event("GetById.BeforeMapping"), DateTime.Now);
            
            var customer = new Customer
            {
                Id = customerEntity.Id,
                FirstName = customerEntity.FirstName,
                LastName = customerEntity.LastName,
                Email = customerEntity.Email
            };
            
            trace.Record(Annotations.Event("GetById.AfterMapping"), DateTime.Now);

            return Ok(customer);
        }

        // POST api/customers
        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest(new OperationStatus("Customer is not valid"));
            }
            
            var customerEntity = new Data.Entities.Customer
            {
                FirstName =  customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email
            };

            var customerId = await _customerRepository.CreateCustomer(customerEntity).ConfigureAwait(false);;

            customer.Id = customerId;
            
            return CreatedAtAction(nameof(GetById), new { id = customerId}, customer);
        }

        // PUT api/customers/5
        [HttpPut ("{id}")]
        public async Task<IActionResult> Put (int id, [FromBody] Customer customer) {
            if (customer == null)
            {
                return BadRequest(new OperationStatus("Customer is not valid"));
            }

            var customerEntity = await _customerRepository.GetCustomerById(id).ConfigureAwait(false);; 

            if (customerEntity == null)
            {
                return NotFound(new OperationStatus("Customer not found"));
            }
            
            customerEntity.FirstName = customer.FirstName;
            customerEntity.LastName = customer.LastName;
            customerEntity.Email = customer.Email;

            await _customerRepository.UpdateCustomer(id, customerEntity).ConfigureAwait(false);;

            return Ok();
        }

        // DELETE api/customers/5
        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var customer = await _customerRepository.GetCustomerById(id).ConfigureAwait(false);;

            if (customer == null)
            {
                return NotFound(new OperationStatus("Customer not found"));
            }

            await _customerRepository.DeleteCustomer(id).ConfigureAwait(false); 

            return Ok();
        }
    }
}