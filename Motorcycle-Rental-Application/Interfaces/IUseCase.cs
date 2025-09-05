using FluentResults;

namespace Motorcycle_Rental_Application.Interfaces
{
    public interface IUseCase<TRequest, TResponse>
    {
     public Task<Result<TResponse>> ExecuteAsync(TRequest request);
    }

    public interface IUseCase<TRequest>
    {
        public Task<Result> ExecuteAsync(TRequest request);
        
    }


}

