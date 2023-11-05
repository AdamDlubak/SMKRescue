using CsvHelper.Configuration;

namespace SMKRescue.Models;

public class ExcelModel
{
    public string DataZnieczulenia { get; set; }
    public string Plec { get; set; }
    public string Wiek { get; set; }
    public string Inicjaly { get; set; }
    public string ASA { get; set; }
    public string KodZabiegu { get; set; }
    public string Nadzor { get; set; }
    public string NazwaZabiegu { get; set; }
}

public sealed class ExcelModelMap : ClassMap<ExcelModel>
{
    public ExcelModelMap()
    {
        Map(m => m.DataZnieczulenia).Name("Data znieczulenia");
        Map(m => m.Plec).Name("Płeć");
        Map(m => m.Wiek).Name("Wiek");
        Map(m => m.Inicjaly).Name("Inicjały pacjenta");
        Map(m => m.ASA).Name("ASA");
        Map(m => m.KodZabiegu).Name("Kod zabiegu");
        Map(m => m.Nadzor).Name("Nadzór");
        Map(m => m.NazwaZabiegu).Name("Nazwa zabiegu");
    }
}