﻿namespace ART.Domotica.Worker.AutoMapper
{
    using ART.Domotica.Model;
    using ART.Domotica.Repository.Entities;

    using global::AutoMapper;

    public class DeviceDatasheetProfile : Profile
    {
        #region Constructors

        public DeviceDatasheetProfile()
        {
            CreateMap<DeviceDatasheet, DeviceDatasheetGetModel>()
                .ForMember(vm => vm.DeviceTypeId, m => m.MapFrom(x => x.DeviceTypeId))
                .ForMember(vm => vm.DeviceDatasheetId, m => m.MapFrom(x => x.Id))
                .ForMember(vm => vm.Name, m => m.MapFrom(x => x.Name))
                .ForMember(vm => vm.HasDeviceSerial, m => m.MapFrom(x => x.HasDeviceSerial))
                .ForMember(vm => vm.HasDeviceSensor, m => m.MapFrom(x => x.HasDeviceSensor));
        }

        #endregion Constructors
    }
}