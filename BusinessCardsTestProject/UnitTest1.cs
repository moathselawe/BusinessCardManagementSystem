using BCMS.Api.Controllers;
using BCMS.Application.Commands.BusinessCard;
using BCMS.Application.Queries.BusinessCard;
using BCMS.Domain.Dtos;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace BusinessCardsTestProject;

public class BusinessCardsControllerTests
{
    private readonly Mock<ISender> _mockSender;
    private readonly BusinessCardsController _controller;

    public BusinessCardsControllerTests()
    {
        _mockSender = new Mock<ISender>();
        _controller = new BusinessCardsController(_mockSender.Object);
    }

    [Fact]
    public async Task CreateCard_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var dto = new CreateBusinessCardDto(
            "عربي", "English", DateTime.UtcNow.AddYears(-25),
            "test@example.com", "+123456789", null, "Address 123"
        );

        var commandResult = new CreateBusinessCardResult(Guid.NewGuid());
        _mockSender.Setup(s => s.Send(It.IsAny<CreateBusinessCardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(commandResult);

        // Act
        var result = await _controller.CreateCard(dto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(commandResult.Id, result.Value);
    }

    [Fact]
    public async Task DeleteCard_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockSender.Setup(s => s.Send(It.IsAny<DeleteBusinessCardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeleteBusinessCardResult(true));

        // Act
        var result = await _controller.DeleteCard(id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetCardById_ReturnsResult()
    {
        // Arrange
        var id = Guid.NewGuid();
        var dto = new BusinessCardDto(id, "عربي", "English", DateTime.UtcNow.AddYears(-20), "test@test.com", "+12345678", null, "Address");
        _mockSender.Setup(s => s.Send(It.IsAny<GetBusinessCardByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetBusinessCardByIdResult(dto));

        // Act
        var result = await _controller.GetCardById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Id, result.response.Id);
    }

    [Fact]
    public async Task UpdateCard_ReturnsOk_WhenSuccessful()
    {
        // Arrange
        var dto = new UpdateBusinessCardDto(Guid.NewGuid(), "عربي", "English", DateTime.UtcNow.AddYears(-25), "test@example.com", "+123456789", null, "Address 123");
        _mockSender.Setup(s => s.Send(It.IsAny<UpdateBusinessCardCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new UpdateBusinessCardResult(true));

        // Act
        var result = await _controller.UpdateCard(dto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Id, result.Value);
    }

    [Fact]
    public async Task GetAllCards_ReturnsList()
    {
        // Arrange
        var cards = new List<BusinessCardDto>
            {
                new(Guid.NewGuid(), "عربي", "English", DateTime.UtcNow.AddYears(-20), "a@test.com", "+12345", null, "Addr1"),
                new(Guid.NewGuid(), "عربي2", "English2", DateTime.UtcNow.AddYears(-22), "b@test.com", "+67890", null, "Addr2")
            };

        _mockSender.Setup(s => s.Send(It.IsAny<GetAllBusinessCardsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllBusinessCardsResult(cards));

        // Act
        var result = await _controller.GetAllCards();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.response.Count);
    }

    [Fact]
    public async Task ExportFile_ReturnsFile()
    {
        // Arrange
        var request = new ExportRequestDto { FileType = "csv" };
        var fileContent = System.Text.Encoding.UTF8.GetBytes("test");
        _mockSender.Setup(s => s.Send(It.IsAny<ExportBusinessCardsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ExportBusinessCardsResult(fileContent, "text/csv", "file.csv"));

        // Act
        var result = await _controller.ExportFile(request) as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("file.csv", result.FileDownloadName);
    }

    [Fact]
    public async Task PreviewFile_ReturnsCards()
    {
        // Arrange
        var fileMock = new Mock<IFormFile>();
        var cards = new List<BusinessCardPreviewDto>
            {
                new("عربي", "English", DateTime.UtcNow.AddYears(-20), "test@test.com", "+12345", null, "Address")
            };

        _mockSender.Setup(s => s.Send(It.IsAny<PreviewBusinessCardsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PreviewBusinessCardsResult(cards));

        // Act
        var result = await _controller.PreviewFile(fileMock.Object) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
    }


    [Fact]
    public async Task Search_ReturnsOk_WithResults()
    {
        // Arrange
        var filters = new SearchFiltersRqDto("Test");
        var resultDto = new SearchFiltersRsDto<BusinessCardDto>(new List<BusinessCardDto>
            {
                new(Guid.NewGuid(), "عربي", "English", DateTime.UtcNow.AddYears(-20), "a@test.com", "+12345", null, "Address1")
            }, 1);

        _mockSender.Setup(s => s.Send(It.IsAny<SearchQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(resultDto);

        // Act
        var result = await _controller.Search(filters) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(resultDto, result.Value);
    }

    [Fact]
    public async Task CreateMany_ReturnsOk_WithCount()
    {
        // Arrange
        var cards = new List<CreateBusinessCardDto>
            {
                new("عربي", "English", DateTime.UtcNow.AddYears(-25), "a@test.com", "+1234567", null, "Address 1"),
                new("عربي2", "English2", DateTime.UtcNow.AddYears(-30), "b@test.com", "+1234568", null, "Address 2")
            };

        _mockSender.Setup(s => s.Send(It.IsAny<CreateManyBusinessCardsCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CreateManyBusinessCardsResult(cards.Count));

        // Act
        var result = await _controller.CreateMany(cards) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cards.Count, ((CreateManyBusinessCardsResult)result.Value).Count);
    }

    [Fact]
    public async Task GeneratePdf_ReturnsFile()
    {
        // Arrange
        var command = new GeneratePdfCommand(Guid.NewGuid());
        var pdfBytes = new byte[] { 1, 2, 3, 4 };
        var fileName = "BusinessCard.pdf";

        _mockSender.Setup(s => s.Send(It.IsAny<GeneratePdfCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GeneratePdfResult(pdfBytes, fileName));

        // Act
        var result = await _controller.GeneratePdf(command) as FileContentResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("application/pdf", result.ContentType);
        Assert.Equal(fileName, result.FileDownloadName);
        Assert.Equal(pdfBytes, result.FileContents);
    }
}

