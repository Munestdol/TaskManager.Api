using Mapster;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Tasks.DTOs;
using TaskManager.Domain.Entities;

namespace TaskManager.Application
{
    public static class MappingConfig
    {
        public static void RegisterMappings(this TypeAdapterConfig config)
        {
            config.NewConfig<TaskItem, TaskDto>();

            config.NewConfig<CreateTaskDto, TaskItem>()
                  .ConstructUsing(src => new TaskItem(src.Title, src.Description, src.DueDateUtc));

            config.NewConfig<UpdateTaskDto, TaskItem>();
        }
    }
}
