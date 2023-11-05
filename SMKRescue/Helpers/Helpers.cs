namespace SMKRescue.Helpers;

public static class Helpers
{
    public static string PrepareProcedureGroup(string asa, string procedureName)
    {
        return !string.IsNullOrWhiteSpace(procedureName) ? $"ASA {asa}, {procedureName}" : $"ASA {asa}";
    }

    public static string ConvertDate(string date)
    {
        var dateParts = date.Split("-");
        return $"{dateParts[2]}-{dateParts[1]}-{dateParts[0]}";
    }

    public static string MapSex(string sex)
    {
        return sex switch
        {
            "Mężczyzna" => "M",
            "Kobieta" => "K",
            _ => string.Empty
        };
    }
}