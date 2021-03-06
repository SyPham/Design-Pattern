using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMR_API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DMR_API._Repositories.Interface;
using DMR_API._Services.Interface;
using DMR_API.DTO;
using DMR_API.Models;
using Microsoft.EntityFrameworkCore;
using EC_API.Data;
using DMR_API.Data;

namespace DMR_API._Services.Services
{
    public class RoleService : IRoleService
    {

        private readonly IRoleRepository _repoRole;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public RoleService(IRoleRepository repoRole, IMapper mapper, MapperConfiguration configMapper)
        {
            _configMapper = configMapper;
            _mapper = mapper;
            _repoRole = repoRole;

        }
        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public RoleService(IRoleRepository repoRole)
        {
            _repoRole = repoRole;

        }
        public async Task<bool> Add(RoleDto model)
        {
            var artRole = _mapper.Map<Role>(model);
            _repoRole.Add(artRole);
            return await _repoRole.SaveAll();
        }


        public async Task<bool> Delete(object id)
        {
            var ArtRole = _repoRole.FindById(id);
            _repoRole.Remove(ArtRole);
            return await _repoRole.SaveAll();
        }

        public async Task<bool> Update(RoleDto model)
        {
            var artRole = _mapper.Map<Role>(model);
            _repoRole.Update(artRole);
            return await _repoRole.SaveAll();
        }
        public async Task<List<RoleDto>> GetAllAsync()
        {
            return await _repoRole.FindAll().ProjectTo<RoleDto>(_configMapper).OrderBy(x => x.ID).ToListAsync();
        }


        public Task<PagedList<RoleDto>> GetWithPaginations(PaginationParams param)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<RoleDto>> Search(PaginationParams param, object text)
        {
            throw new NotImplementedException();
        }

        public RoleDto GetById(object id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Role>> GetAllDto()
        {
            var model =  await _unitOfWork.GetRepository<Role>().FindAll().OrderBy(x => x.ID).ToListAsync();
            return model;
        }
    }
}