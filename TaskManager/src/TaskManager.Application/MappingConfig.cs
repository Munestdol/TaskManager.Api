using Mapster;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application
{
    public static class MappingConfig
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            TypeAdapterConfig<TaskItem, TaskDto>.NewConfig();
            TypeAdapterConfig<CreateTaskDto, TaskItem>.NewConfig();
            TypeAdapterConfig<UpdateTaskDto, TaskItem>.NewConfig();
        }
    }
}
