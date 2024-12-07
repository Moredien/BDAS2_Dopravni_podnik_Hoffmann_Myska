using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class LinkaDTO
{
    [IdProperty]
    [ColumnName("LINKA")]
    public int Linka { get; set; }

    [ColumnName("START_STOP")]
    public string StartStop { get; set; }

    [ColumnName("END_STOP")]
    public string EndStop { get; set; }

    [ColumnName("START_TIME")]
    public DateTime StartTime { get; set; }

    [ColumnName("END_TIME")]
    public DateTime EndTime { get; set; }

    [ColumnName("TRASA")]
    public int Trasa { get; set; }

    [ColumnName("DIRECTION")]
    public int Direction { get; set; }
}