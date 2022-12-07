namespace Services;

public class ServiceResult
{
	/// <summary>
	/// Creates a successful <see cref="ServiceResult"/>
	/// </summary>
	/// <returns>a <see cref="ServiceResult"/> that indicates a service call was successful</returns>
	public static ServiceResult Ok() => new () { ServiceError = ServiceError.None };

	/// <summary>
	/// Creates a successful <see cref="ServiceResult"/> that includes a result payload
	/// </summary>
	/// <param name="result">The result payload to return from the service call</param>
	/// <returns>a <see cref="ServiceResult"/> that indicates a service callw as successful, along with the result payload</returns>
	public static ServiceResult Ok(object result) => new()
	{
		ServiceError = ServiceError.None,
		Result = result
	};

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> with generic error information
	/// </summary>
	/// <returns>a <see cref="ServiceResult"/> that indicates a service call was unsuccessful</returns>
	public static ServiceResult Failure() => new()
	{
		ServiceError = ServiceError.Unknown,
		ErrorMessage = "An unknown error has occurred."
	};

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> with the specified error message
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult Failure(string errorMessage) => new()
	{
		ServiceError = ServiceError.Unknown,
		ErrorMessage = errorMessage
	};

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> with the specified error information
	/// </summary>
	/// <param name="errorType">The <see cref="ServiceError"/> representing the result of the service</param>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult Failure(ServiceError errorType, string? errorMessage) => new()
	{
		ServiceError = errorType,
		ErrorMessage = errorMessage
	};

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> indicating the specified resource couldn't be found
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult NotFound(string? errorMessage = null) => Failure(ServiceError.NotFound, errorMessage);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> indicating the current user is not authenticated
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult Unauthorized(string? errorMessage = null) => Failure(ServiceError.Unauthorized, errorMessage);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> indicating the current user is not authorized to perform an action
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult Forbidden(string? errorMessage = null) => Failure(ServiceError.Forbidden, errorMessage);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> indicating the request could not be processed with the given data
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult Unprocessable(string? errorMessage = null) => Failure(ServiceError.Unprocessable, errorMessage);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> indicating there was conflicting data that prevented the request from processing
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult Conflict(string? errorMessage = null) => Failure(ServiceError.DataConflict, errorMessage);

	/// <summary>
	/// Creates a failed <see cref="ServiceResult"/> indicating the resource being updated was modified by another user
	/// </summary>
	/// <param name="errorMessage">A message with more specific information about why a service call failed</param>
	/// <returns></returns>
	public static ServiceResult ConcurrencyConflict(string? errorMessage = null) => Failure(ServiceError.DatabaseConcurrency, errorMessage);

	public bool WasSuccessful => ServiceError == ServiceError.None;

	public ServiceError ServiceError { get; private init; }

	public object? Result { get; private init; }

	public string? ErrorMessage { get; set; }
}