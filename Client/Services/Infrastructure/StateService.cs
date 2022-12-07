using System.Net.Http;
using Autoinjector;
using Microsoft.Extensions.Logging;
using Core.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Client.Services.Infrastructure;

namespace Client.Services.Infrastructure;

[Service(ServiceLifetime.Scoped, typeof(IStateService))]
public class StateService : CrudService<StateDto, StateDto, StateDto, StateService>,
                            IStateService
{
	public StateService(HttpClient client,
	                    ILogger<StateService> logger,
	                    ISnackbarService snackbar)
		: base("states", client, logger, snackbar)
	{
		AddSuccessMessage = "State created successfully!";
		EditSuccessMessage = "State updated successfully!";
		DeleteSuccessMessage = "State deleted successfully!";
	}
}

public interface IStateService : ICrudService<StateDto, StateDto, StateDto>
{
}