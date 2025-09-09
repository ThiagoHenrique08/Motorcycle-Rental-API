using FluentAssertions;
using Motorcycle_Rental_API;
using Motorcycle_Rental_Tests.Builder;
using Motorcycle_Rental_Tests.Integration;
using System.Net;
using System.Net.Http.Json;

namespace Motorcycle_Rental_Tests.Integration.DeliveryManIntegrationTest;
public sealed class CreateDeliveryManIntegrationTests : BaseIntegrationTests, IDisposable
{

    private HttpResponseMessage? _result;
    public CreateDeliveryManIntegrationTests(CustomWebApplicationFactory<Program> factory)
         : base(factory)
    {
    }

    [Fact]
    public async Task Test()
    {
        // Arrange
        var dto = new DeliveryManBuilder().Build();


        // Act
        _result = await GetHttpClient().PostAsJsonAsync("/DeliveryMan", dto);
        _result?.StatusCode.Should().Be(HttpStatusCode.Created);

        // Assert: verificar se a entidade realmente existe no banco
        var createdDeliveryMan = _dbContext.DeliveryMans.FirstOrDefault(c =>
            c.Identifier == dto.Identifier);

        createdDeliveryMan.Should().NotBeNull();
        createdDeliveryMan!.Name.Should().Be(dto.Name);
        createdDeliveryMan!.CNPJ.Should().Be(dto.CNPJ);
        createdDeliveryMan!.BirthDate.Should().BeCloseTo(dto.BirthDate, TimeSpan.FromMilliseconds(1));
        createdDeliveryMan!.CNHNumber.Should().Be(dto.CNHNumber);
        createdDeliveryMan!.CNHType.Should().Be(dto.CNHType);
        createdDeliveryMan!.CNHImage.Should().Be(dto.CNHImage);


        // Cleanup: remove tudo do banco
        _dbContext.DeliveryMans.Remove(createdDeliveryMan);

        await _dbContext.SaveChangesAsync();
    }

    public new void Dispose()
    {
        _dbContext.Database.EnsureDeleted(); // limpa o banco de teste
        _dbContext.Dispose();
    }

}
