using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Guide_Project.Core.Services;
using Guide_Project.Core.DTOs;
using Guide_Project.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Guide_Project.Service.Publishers;

namespace Guide_Project.API.Controllers;

[Authorize]
[Route("api/[controller]s/[action]")]
[ApiController]
public class CustomerController : CustomController
{
    private readonly IGenericService<Customer, CustomerDto> _customerService;
    private readonly WaterMarkMqPublisher<CustomerDto> _watermarker;
    public CustomerController(IGenericService<Customer, CustomerDto> customerService,
        WaterMarkMqPublisher<CustomerDto> watermarker)
    {
        _customerService = customerService;
        _watermarker = watermarker;
    }

    [HttpGet]
    public async Task<IActionResult> GetCustumers()
    {
        return ActionResultInstance(await _customerService.GetAllAsync());
    }
    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CustomerDto customerDto)
    {
        _watermarker.Publish(customerDto);
        return ActionResultInstance(await _customerService.AddAsync(customerDto));
    }
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer(CustomerDto customerDto)
    {
        return ActionResultInstance(await _customerService.Update(customerDto, customerDto.Id));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        return ActionResultInstance(await _customerService.RemoveById(id));
    }

    [Authorize(Policy = "RequireAdministratorRole")]
    [HttpDelete]
    public async Task<IActionResult> DeleteAllCustomers()
    {
        return ActionResultInstance(await _customerService.RemoveAll());
    }
}