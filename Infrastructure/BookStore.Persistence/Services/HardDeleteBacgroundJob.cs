using BookStore.Application.Interfaces.IManagers;
using BookStore.Domain.Entities.Users;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStore.Persistence.Services;
public class HardDeleteBacgroundJob: BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HardDeleteBacgroundJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                try
                {
                    var userRepository = scope.ServiceProvider.GetRequiredService<IBaseManager<User>>();

                    var usersToDelete = await userRepository.GetAllAsync(u => u.IsDeleted && u.DeletedAt.HasValue
                                                       && u.DeletedAt.Value.AddDays(30) <= DateTime.UtcNow);

                    foreach (var user in usersToDelete)
                    {
                        userRepository.HardDelete(user);
                    }

                    await userRepository.Commit();
                }
                catch (Exception ex)
                {
                }
            }

            // Restarts the Job after 1 day
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}

