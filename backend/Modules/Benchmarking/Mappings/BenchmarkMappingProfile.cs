using AutoMapper;
using backend.Modules.Benchmarking.DTOs;
using backend.Modules.Benchmarking.Entities;

namespace backend.Modules.Benchmarking.Mappings;

public class BenchmarkMappingProfile : Profile
{
    public BenchmarkMappingProfile()
    {
        CreateMap<BenchmarkMetric, BenchmarkMetricDto>();
    }
}
