using System;
using System.Linq;
using AutoMapper;
using PaymentProcessor.Dtos;
using PaymentProcessor.Models;

namespace LifeLongApi.Codes
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }
    }
}