using BCMS.Domain.Dtos;
using MediatR;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace BCMS.Application.Commands.BusinessCard;

public record GeneratePdfCommand(Guid Id) : IRequest<GeneratePdfResult>;
public record GeneratePdfResult(byte[] FileBytes, string FileName);
public class GeneratePdfHandlerValidator : AbstractValidator<GeneratePdfCommand>
{
    public GeneratePdfHandlerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
public class GeneratePdfHandler : IRequestHandler<GeneratePdfCommand, GeneratePdfResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;

    public GeneratePdfHandler(IBusinessCardRepository businessCardRepository)
    {
        _businessCardRepository = businessCardRepository;
    }

    public async Task<GeneratePdfResult> Handle(GeneratePdfCommand request, CancellationToken cancellationToken)
    {
        var card = await _businessCardRepository.GetByIdAsync(request.Id, cancellationToken);

        string logoSrc = !string.IsNullOrWhiteSpace(card.Logo) && card.Logo.StartsWith("data:image/")
                 ? card.Logo
                 : "data:image/svg+xml;base64,PHN2ZyBmaWxsPSIjY2NjIiBoZWlnaHQ9IjQ4IiB3aWR0aD0iNDgiIHZpZXdCb3g9IjAgMCA0OCA0OCIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIj48Y2lyY2xlIGN4PSIyNCIgY3k9IjE0IiByPSIxMCIvPjxwYXRoIGQ9Ik0yNCAyNmMtOC44IDAtMTYgNy4yLTE2IDE2aDMyaC0wLjAwMUM0MCAzMy4yIDMyLjggMjYgMjQgMjZ6Ii8+PC9zdmc+"; // default icon

        string html = $@"
<html>
<head>
<style>
.business-card-preview {{
  width: 400px;
  height: 160px;
  border-radius: 15px;
  display: flex;
  align-items: center;
  padding: 15px;
  box-shadow: 0 5px 15px rgba(0,0,0,0.2);
  position: relative;
  overflow: hidden;
  font-family: Arial, sans-serif;
  background: #ffffff;
  color: #000000;
}}
.business-card-preview::before {{
  content: '';
  position: absolute;
  bottom: -50px;
  left: -50px;
  width: 200%;
  height: 200%;
  background: rgba(0, 122, 217, 0.1);
  border-radius: 50%;
  transform: rotate(-30deg);
}}
.logo img {{
  width: 80px;
  height: 80px;
  border-radius: 50%;
  object-fit: cover;
  margin-right: 15px;
}}
.info h2 {{
  margin: 0 0 6px 0;
  font-size: 1.3rem;
}}
.info h4 {{
  margin: 0 0 10px 0;
  font-weight: 500;
  font-size: 1rem;
}}
.contact p {{
  margin: 3px 0;
  font-size: 0.8rem;
  display: flex;
  align-items: center;
}}
.contact p span.icon {{
  margin-right: 6px;
}}
</style>
</head>
<body>
  <div class='business-card-preview'>
    <div class='logo'>
      <img src='{logoSrc}' />
    </div>
    <div class='info'>
      <h2>{card.EnglishName}</h2>
      <h4>Business Card</h4>
      <div class='contact'>
        <p><span class='icon'>📍</span>{card.Address}</p>
        <p><span class='icon'>📞</span>{card.Phone}</p>
        <p><span class='icon'>✉️</span>{card.Email}</p>
      </div>
    </div>
  </div>
</body>
</html>
";

        await new BrowserFetcher().DownloadAsync();

        await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true,
            Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" } 
        });

        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(html);

        byte[] pdfBytes = await page.PdfDataAsync(new PdfOptions
        {
            PrintBackground = true,
            Landscape = false, 
            PreferCSSPageSize = false,
            Width = "118mm",
            Height = "54mm",
            MarginOptions = new MarginOptions
            {
                Top = "0mm",
                Bottom = "0mm",
                Left = "0mm",
                Right = "0mm"
            }
        });


        return new GeneratePdfResult(pdfBytes, $"{card.EnglishName}-BusinessCard.pdf");
    }
}
