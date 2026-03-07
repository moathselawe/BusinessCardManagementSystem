namespace HireMind.Application.Services;
public class FileParserService : IFileParserService
{
    public async Task<List<BusinessCardPreviewDto>> ParseFileAsync(IFormFile file)
    {
        if (file.FileName.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
            return await ParseCsv(file);
        else if (file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            return await ParseXml(file);

        throw new InvalidOperationException("Unsupported file type");
    }

    private async Task<List<BusinessCardPreviewDto>> ParseCsv(IFormFile file)
    {
        var result = new List<BusinessCardPreviewDto>();
        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);

        if (!reader.EndOfStream)
            await reader.ReadLineAsync();

        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync();
            var values = line.Split(',');

            result.Add(new BusinessCardPreviewDto(
                ArabicName: values[0],
                EnglishName: values[1],
                DateOfBirth: DateTime.TryParse(values[2], out var dob) ? dob : DateTime.MinValue,
                Email: values[3],
                Phone: values[4],
                Logo: string.IsNullOrWhiteSpace(values[5]) ? null : values[5],
                Address: string.IsNullOrWhiteSpace(values[6]) ? null : values[6]
                ));
        }
        return result;
    }


    private async Task<List<BusinessCardPreviewDto>> ParseXml(IFormFile file)
    {
        var result = new List<BusinessCardPreviewDto>();
        using var stream = file.OpenReadStream();
        var doc = new System.Xml.XmlDocument();
        doc.Load(stream);
        foreach (System.Xml.XmlNode node in doc.SelectNodes("//BusinessCard")!)
        {
            result.Add(new BusinessCardPreviewDto(
                ArabicName: node["ArabicName"]?.InnerText ?? "",
                EnglishName: node["EnglishName"]?.InnerText ?? "",
                DateOfBirth: DateTime.MinValue, 
                Email: node["Email"]?.InnerText ?? "",
                Phone: node["Phone"]?.InnerText ?? "",
                Logo: null,
                Address: null
            ));
        }
        return result;
    }

}
