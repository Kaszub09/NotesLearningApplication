using AutoMapper;
using DatabaseServices.Entities;
using NotesLearningApplication.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices {
    public class NotesMappingProfile : Profile {

        public NotesMappingProfile() {
            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();
            CreateMap<Note, NoteDTO>().ForMember(m => m.Category, c => c.MapFrom(s => s.Category));
            CreateMap<NoteDTO, Note>().ForMember(m => m.Category, c => c.MapFrom(s => s.Category));
            CreateMap<UserDTO, User>();
            CreateMap<User, UserDTO>();

        }
    }
}
