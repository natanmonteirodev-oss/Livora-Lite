using AutoMapper;
using Livora_Lite.Application.DTO;
using Livora_Lite.Domain.Entities;

namespace Livora_Lite.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Tenant Mappings
            CreateMap<Tenant, TenantDTO>().ReverseMap();
            CreateMap<CreateTenantRequestDTO, Tenant>();
            CreateMap<UpdateTenantRequestDTO, Tenant>();

            // Property Mappings
            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<CreatePropertyRequestDTO, Property>();
            CreateMap<UpdatePropertyRequestDTO, Property>();

            // Address Mappings
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<CreateAddressRequestDTO, Address>();
            CreateMap<UpdateAddressRequestDTO, Address>();

            // PropertyType Mappings
            CreateMap<PropertyType, PropertyTypeDTO>().ReverseMap();

            // PropertyStatus Mappings
            CreateMap<PropertyStatus, PropertyStatusDTO>().ReverseMap();

            // TenantStatus Mappings
            CreateMap<TenantStatus, TenantStatusDTO>().ReverseMap();

            // Contract Mappings
            CreateMap<Contract, ContractDTO>().ReverseMap();
            CreateMap<CreateContractRequestDTO, Contract>();
            CreateMap<UpdateContractRequestDTO, Contract>();

            // ContractStatus Mappings
            CreateMap<ContractStatus, ContractStatusDTO>().ReverseMap();

            // Billing Mappings
            CreateMap<Billing, BillingDTO>().ReverseMap();
            CreateMap<CreateBillingRequestDTO, Billing>();
            CreateMap<UpdateBillingRequestDTO, Billing>();

            // BillingStatus Mappings
            CreateMap<BillingStatus, BillingStatusDTO>().ReverseMap();

            // Payment Mappings
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<CreatePaymentRequestDTO, Payment>();
            CreateMap<UpdatePaymentRequestDTO, Payment>();

            // PaymentMethod Mappings
            CreateMap<PaymentMethod, PaymentMethodDTO>().ReverseMap();

            // MaintenanceRequest Mappings
            CreateMap<MaintenanceRequest, MaintenanceRequestDTO>().ReverseMap();
            CreateMap<CreateMaintenanceRequestDTO, MaintenanceRequest>();

            // AuditLog Mappings
            CreateMap<AuditLog, AuditLogDTO>().ReverseMap();
            CreateMap<CreateAuditLogDTO, AuditLog>();

            // User Mappings
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
