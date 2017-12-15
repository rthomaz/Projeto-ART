﻿namespace ART.Domotica.Worker.AutoMapper
{
    using ART.Domotica.Model;
    using ART.Domotica.Repository.Entities;

    using global::AutoMapper;
    using System.Linq;

    public class DeviceSensorsProfile : Profile
    {
        #region Constructors

        public DeviceSensorsProfile()
        {
            CreateMap<DeviceSensors, DeviceSensorsGetModel>()
                .ForMember(vm => vm.DeviceSensorsId, m => m.MapFrom(x => x.Id))
                .ForMember(vm => vm.PublishIntervalInSeconds, m => m.MapFrom(x => x.PublishIntervalInSeconds))
                .ForMember(vm => vm.SensorInDevice, m => m.MapFrom(x => x.SensorInDevice.OrderBy(y => y.Ordination)));

            CreateMap<DeviceSensors, DeviceSensorsSetPublishIntervalInSecondsModel>()
                .ForMember(vm => vm.DeviceSensorsId, m => m.MapFrom(x => x.Id))
                .ForMember(vm => vm.PublishIntervalInSeconds, m => m.MapFrom(x => x.PublishIntervalInSeconds));
        }

        #endregion Constructors
    }
}