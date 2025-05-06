using AutoMapper;
using equilog_backend.Common;
using equilog_backend.Data;
using equilog_backend.DTOs.StableJoinRequestDTOs;
using equilog_backend.Interfaces;

namespace equilog_backend.Services;

public class StableJoinRequestService(EquilogDbContext context, IMapper mapper) : IStableJoinRequestService
{
    public Task<Unit> CreateStableJoinRequest(StableJoinRequestDto stableJoinRequestDto)
    {
        throw new NotImplementedException();
    }
}