using AutoMapper;

namespace AutoMapperPostBuilder_Sample
{
    static class MappingClass
    {
        static MappingClass()
        {            
            // Below initialization code works fine. If you want to trigger a AutoMapperPostBuild exception
            // just comment out any of the lines in which a member is mapped.
            // for instance, line 24
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<SampleClassA, SampleClassB>()
                    .ForMember(dest => dest.Prop1, opt => opt.MapFrom(src => src.Prop1));
                cfg.CreateMap<SampleClassB, SampleClassA>()
                    .ForMember(dest => dest.Prop4, opt => opt.Ignore())
                    .ForMember(dest => dest.Prop6, opt => opt.Ignore());

                cfg.CreateMap<SampleClassA_1, SampleClassB_1>()
                    .ForMember(dest => dest.InnerProp10, opt => opt.MapFrom(src => src.InnerProp1))
                    .ForMember(dest => dest.InnerProp20, opt => opt.MapFrom(src => src.InnerProp2));
                cfg.CreateMap<SampleClassB_1, SampleClassA_1>()                                        
                    .ForMember(dest => dest.InnerProp1, opt => opt.MapFrom(src => src.InnerProp10))
                    .ForMember(dest => dest.InnerProp2, opt => opt.MapFrom(src => src.InnerProp20));
            });
            
            Mapper.AssertConfigurationIsValid();
        }

        public static void Invoke()
        {
        }
    }
}
