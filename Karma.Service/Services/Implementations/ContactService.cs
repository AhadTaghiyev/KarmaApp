using System;
using System.Collections.Generic;
using AutoMapper;
using Karma.Core.DTOS;
using Karma.Core.Entities;
using Karma.Core.Repositories;
using Karma.Data.Repositories;
using Karma.Service.Exceptions;
using Karma.Service.Extentions;
using Karma.Service.Responses;
using Karma.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace Karma.Service.Services.Implementations
{
    public class ContactService : IContactService
    {
        readonly IContactRepository _ContactRepository;
        readonly IPositionRepository _positionRepository;
        readonly IWebHostEnvironment _env;
        readonly IMapper _mapper;

        public ContactService(IContactRepository ContactRepository, IPositionRepository positionRepository, IWebHostEnvironment env, IMapper mapper)
        {
            _ContactRepository = ContactRepository;
            _positionRepository = positionRepository;
            _env = env;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(ContactPostDto dto)
        {
            CommonResponse commonResponse = new CommonResponse
            {
                StatusCode = 200
            };
            Contact contact = _mapper.Map<Contact>(dto);
            await _ContactRepository.AddAsync(contact);
            await _ContactRepository.SaveChangesAsync();
            return commonResponse;
        }
        public async Task<IEnumerable<ContactGetDto>> GetAllAsync(int page=1)
        {
            var query = _ContactRepository.GetQuery(x=>x.IsDeleted==false);
            IEnumerable<ContactGetDto> contacts = _mapper.Map<IEnumerable<ContactGetDto>>(query);
            return contacts;
        }

        public async Task RemoveAsync(int id)
        {
            Contact? Contact = await _ContactRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Contact == null)
            {
                throw new ItemNotFoundExcpetion("Contact Not Found");
            }
            Contact.IsDeleted = true;
            await _ContactRepository.UpdateAsync(Contact);
            await _ContactRepository.SaveChangesAsync();
        }
    }
}

