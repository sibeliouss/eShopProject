using Application.Features.Customers.Dtos;
using Application.Services.Repositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services.Customers;

public class CustomerService: ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
       
    }

    public async Task CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        var customer = new Customer 
        {
            FirstName = createCustomerDto.FirstName,
            LastName = createCustomerDto.LastName,
            Email = createCustomerDto.Email,
            EmailConfirmed = true,
            UserName = createCustomerDto.UserName,
            UserId = createCustomerDto.UserId,
            PasswordHash = createCustomerDto.PasswordHash
        };
        
        await _customerRepository.AddAsync(customer);
    }
    public  async Task UpdateCustomerInformationAsync(UpdateCustomerInformationDto request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new Exception("Kayıt Bulunamadı!");
        }

        customer.FirstName = request.FirstName;
        customer.LastName = request.LastName;
        customer.UserName = request.UserName;
        customer.Email = request.Email;

        await _customerRepository.UpdateAsync(customer);
    }

    public async Task UpdateCustomerPasswordAsync(UpdateCustomerPasswordDto request)
    {
        var customer = await _customerRepository.GetByIdAsync(request.Id);
        if (customer == null)
        {
            throw new Exception("Kayıt Bulunamadı!");
        }
  
        var passwordHasher = new PasswordHasher<Customer>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(customer, customer.PasswordHash, request.CurrentPassword);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new Exception("Mevcut şifre yanlış!" );
        }

        if (request.NewPassword != request.ConfirmedPassword)
        {
            throw new Exception("Yeni şifreler uyuşmuyor!");
        }

        var hashedNewPassword = passwordHasher.HashPassword(null, request.NewPassword);
        customer.PasswordHash = hashedNewPassword;
       
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        return customer;
    }

    public async Task DeleteAccountAsync(Guid customerId, string password)
    {
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
        {
            throw new Exception("Kullanıcı bulunamadı.");
        }

        // Şifre doğrulama : Hesap silinmeden önce şifre onayı ver
        var passwordHasher = new PasswordHasher<Customer>();
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(customer, customer.PasswordHash, password);
        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new Exception("Şifre yanlış!");
        }

        // Hesabı sil
        await _customerRepository.DeleteAsync(customer); 
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }
}
