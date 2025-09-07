using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Motorcycle_Rental_API.Utils
{

    public static class EndpointUtils
    {
        /// <summary>
        /// Executa um use case com retorno Result<T> e permite mensagens customizadas de sucesso e falha.
        /// </summary>
        public static async Task<IActionResult> CallUseCase<T>(
            Func<Task<Result<T>>> func,
            ILogger logger = null,
            Func<Result<T>, IActionResult> onFailure = null,
            Func<Result<T>, IActionResult> onSuccess = null)
        {
            try
            {
                var result = await func();

                if (result.IsSuccess)
                {
                    if (onSuccess != null)
                        return onSuccess(result.Value);

                    // Padrão: Created se tiver valor, NoContent se não
                    return result.Value != null
                        ? new CreatedResult(string.Empty, result.Value)
                        : new NoContentResult();
                }

                if (onFailure != null)
                    return onFailure(result);

                return new ObjectResult(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Erro inesperado ao executar use case.");
                return new ObjectResult(new { error = "Ocorreu um erro inesperado." })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        /// <summary>
        /// Use case sem retorno (Result) com mensagens customizadas
        /// </summary>
        public static async Task<IActionResult> CallUseCase(
            Func<Task<Result>> func,
            ILogger logger = null,
            Func<Result, IActionResult> onFailure = null,
            Func<Result, IActionResult> onSuccess = null)
        {
            try
            {
                var result = await func();

                if (result.IsSuccess)
                {
                    if (onSuccess != null)
                        return onSuccess(result);

                    return new NoContentResult();
                }

                if (onFailure != null)
                    return onFailure(result);

                return new BadRequestObjectResult(new { errors = result.Errors });
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Erro inesperado ao executar use case.");
                return new ObjectResult(new { error = "Ocorreu um erro inesperado." })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }


}

