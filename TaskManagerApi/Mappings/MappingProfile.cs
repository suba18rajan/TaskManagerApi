using AutoMapper;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TaskCreateDTO, TaskItem>();

            CreateMap<TaskUpdateDTO, TaskItem>();

            CreateMap<TaskItem, TaskResponseDTO>();
        }
    }
}