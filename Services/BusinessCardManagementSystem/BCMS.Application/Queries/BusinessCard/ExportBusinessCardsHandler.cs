using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCMS.Application.Queries.BusinessCard;

public record ExportBusinessCardsQuery(ExportRequestDto request) : IRequest<ExportBusinessCardsResult>;

public record ExportBusinessCardsResult(byte[] FileContent, string ContentType, string FileName);

public class ExportBusinessCardsHandler : IRequestHandler<ExportBusinessCardsQuery, ExportBusinessCardsResult>
{
    private readonly IBusinessCardRepository _businessCardRepository;

    public ExportBusinessCardsHandler(IBusinessCardRepository businessCardRepository)
    {
        _businessCardRepository = businessCardRepository;
    }

    public async Task<ExportBusinessCardsResult> Handle(ExportBusinessCardsQuery request, CancellationToken cancellationToken)
    {
        List<businessCardModel> cards;

        var fileType = request.request.FileType;
        var ids = request.request.Ids;

        if (ids != null && ids.Any())
        {
            cards = await _businessCardRepository.GetByIdsAsync(ids, cancellationToken);
        }
        else
        {
            cards = await _businessCardRepository.GetAllAsync(cancellationToken);
        }

        if (fileType.Equals("csv", StringComparison.OrdinalIgnoreCase))
        {
            var csv = GenerateCsv(cards);
            return new ExportBusinessCardsResult(
                FileContent: System.Text.Encoding.UTF8.GetBytes(csv),
                ContentType: "text/csv",
                FileName: $"BusinessCards_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
            );
        }
        else if (fileType.Equals("xml", StringComparison.OrdinalIgnoreCase))
        {
            var xml = GenerateXml(cards);
            return new ExportBusinessCardsResult(
                FileContent: System.Text.Encoding.UTF8.GetBytes(xml),
                ContentType: "application/xml",
                FileName: $"BusinessCards_{DateTime.UtcNow:yyyyMMddHHmmss}.xml"
            );
        }

        throw new InvalidOperationException("Unsupported file type. Use 'csv' or 'xml'.");
    }


    private string GenerateCsv(List<businessCardModel> cards)
    {
        var sb = new StringBuilder();
        sb.AppendLine("ArabicName,EnglishName,DateOfBirth,Email,Phone,Address"); 
        foreach (var c in cards)
        {
            sb.AppendLine($"{c.ArabicName},{c.EnglishName},{c.DateOfBirth:yyyy-MM-dd},{c.Email},{c.Phone},{c.Address}");
        }
        return sb.ToString();
    }

    private string GenerateXml(List<businessCardModel> cards)
    {
        var doc = new System.Xml.XmlDocument();
        var root = doc.CreateElement("BusinessCards");
        doc.AppendChild(root);

        foreach (var c in cards)
        {
            var cardNode = doc.CreateElement("BusinessCard");

            void AppendNode(string name, string? value)
            {
                var node = doc.CreateElement(name);
                node.InnerText = value ?? string.Empty;
                cardNode.AppendChild(node);
            }

            AppendNode("ArabicName", c.ArabicName);
            AppendNode("EnglishName", c.EnglishName);
            AppendNode("DateOfBirth", c.DateOfBirth.ToString("yyyy-MM-dd"));
            AppendNode("Email", c.Email);
            AppendNode("Phone", c.Phone);
            AppendNode("Address", c.Address);

            root.AppendChild(cardNode);
        }

        return doc.OuterXml;
    }
}
