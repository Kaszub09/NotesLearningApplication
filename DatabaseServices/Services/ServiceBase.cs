using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.Services {
    public class ServiceBase {
        protected NotesDbContext _dbContext;
        protected IMapper _mapper;

        public ServiceBase(NotesDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}


