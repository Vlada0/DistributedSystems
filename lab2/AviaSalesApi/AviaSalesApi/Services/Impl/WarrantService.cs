using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AviaSalesApi.Data.Entities;
using AviaSalesApi.Data.Repository.Interfaces;
using AviaSalesApi.Infrastructure.Exceptions;
using AviaSalesApi.Models.Warrants;
using AviaSalesApi.Services.Interfaces;

namespace AviaSalesApi.Services.Impl
{
    public class WarrantService : IWarrantsService
    {
        private readonly IMongoRepository<Warrant> _mongoRepository;
        private readonly IMapper _mapper;

        public WarrantService(IMongoRepository<Warrant> mongoRepository, IMapper mapper)
        {
            _mongoRepository = mongoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WarrantModel>> GetWarrantsByIbanAsync(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                throw new BadRequestException("Iban should not be empty.");
            }
            
            var warrants = await _mongoRepository.FilterByAsync(w => w.PassengerIban == iban);
            var models = _mapper.Map<IEnumerable<WarrantModel>>(warrants);

            return models;
        }

        public async Task<WarrantModel> GetWarrantByIdAsync(Guid id)
        {
            var warrant = await _mongoRepository.FindByIdAsync(id);

            return _mapper.Map<WarrantModel>(warrant);
        }

        public async Task<WarrantModel> CreateWarrantAsync(WarrantCreateUpdateModel model)
        {
            var warrant = _mapper.Map<Warrant>(model);

            var id = await _mongoRepository.InsertOneAsync(warrant);
            warrant.Id = id;

            return _mapper.Map<WarrantModel>(warrant);
        }

        public async Task UpdateWarrantAsync(Guid warrantId, WarrantCreateUpdateModel model)
        {
            var warrant = _mapper.Map<Warrant>(model);
            warrant.Id = warrantId;

            await _mongoRepository.ReplaceOneAsync(warrant);
        }

        public async Task DeleteWarrantAsync(Guid warrantId)
        {
            await _mongoRepository.DeleteByIdAsync(warrantId);
        }
    }
}