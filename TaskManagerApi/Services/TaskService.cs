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

        public TaskService(
            AppDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedTaskResponseDTO> GetAllTasks(
            int pageNumber,
            int pageSize,
            string? search,
            bool? isCompleted)
        {
            pageNumber = pageNumber < 1
                ? 1
                : pageNumber;

            pageSize =
                pageSize is < 1 or > 100
                    ? 10
                    : pageSize;

            IQueryable<TaskItem> query =
                _context.Tasks.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(
                    x => x.Title.Contains(search));
            }

            if (isCompleted.HasValue)
            {
                query = query.Where(
                    x => x.IsCompleted ==
                         isCompleted.Value);
            }

            var totalCount =
                await query.CountAsync();

            var tasks =
                await query
                    .OrderBy(x => x.Id)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new PagedTaskResponseDTO
            {
                Items =
                    _mapper.Map<List<TaskResponseDTO>>(
                        tasks),

                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = totalCount,

                TotalPages =
                    (int)Math.Ceiling(
                        totalCount /
                        (double)pageSize)
            };
        }

        public async Task<TaskResponseDTO?> GetTaskById(
            int id)
        {
            var task =
                await _context.Tasks
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        x => x.Id == id);

            if (task == null)
                return null;

            return _mapper.Map<TaskResponseDTO>(
                task);
        }

        public async Task<TaskResponseDTO> CreateTask(
            TaskCreateDTO dto)
        {
            var task =
                _mapper.Map<TaskItem>(dto);

            task.IsCompleted = false;

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDTO>(
                task);
        }

        public async Task<TaskResponseDTO?> UpdateTask(
            int id,
            TaskUpdateDTO dto)
        {
            var task =
                await _context.Tasks
                    .FirstOrDefaultAsync(
                        x => x.Id == id);

            if (task == null)
                return null;

            _mapper.Map(dto, task);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskResponseDTO>(
                task);
        }

        public async Task<bool> DeleteTask(int id)
        {
            var task =
                await _context.Tasks
                    .FirstOrDefaultAsync(
                        x => x.Id == id);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}