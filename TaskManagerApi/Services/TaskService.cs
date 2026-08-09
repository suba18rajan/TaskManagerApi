using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TaskService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<TaskResponseDTO>> GetAllTasks()
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<TaskResponseDTO>>(tasks);
        }

        public async Task<TaskResponseDTO?> GetTaskById(int id)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                return null;

            return _mapper.Map<TaskResponseDTO>(task);
        }

        public async Task<TaskResponseDTO> CreateTask(TaskCreateDTO dto)
        {
            var task = _mapper.Map<TaskItem>(dto);

            task.IsCompleted = false;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDTO>(task);
        }

        public async Task<TaskResponseDTO?> UpdateTask(int id, TaskUpdateDTO dto)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                return null;

            _mapper.Map(dto, task);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDTO>(task);
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}