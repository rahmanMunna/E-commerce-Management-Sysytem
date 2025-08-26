using AutoMapper;
using EcommerceWeb.Controllers;
using EcommerceWeb.DTO;
using EcommerceWeb.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommerceWeb.Helpers
{
    public static class MapperHelper
    {
        public static Mapper GetMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Customer, CustomerDTO>().ReverseMap();
                cfg.CreateMap<User, UserDTO>().ReverseMap();
                cfg.CreateMap<Product, ProductDTO>().ReverseMap();
                cfg.CreateMap<Order, OrderDTO>().ReverseMap();
                cfg.CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
                cfg.CreateMap<Status, StatusDTO>().ReverseMap();
                cfg.CreateMap<OrderTarcker, OrderTrackerDTO>().ReverseMap();
                cfg.CreateMap<Category, CategoryDTO>().ReverseMap();
                cfg.CreateMap<DeliveryMan, DeliveryManDTO>().ReverseMap();
                cfg.CreateMap<ReturnsTracker, ReturnsTrackerDTO>().ReverseMap();
                cfg.CreateMap<Transaction, TransactionDTO>().ReverseMap();
            });

            var mapper = new Mapper(config);
            return mapper;
        }
    }
}