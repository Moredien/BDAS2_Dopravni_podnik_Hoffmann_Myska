using DopravniPodnik.Utils;

namespace DopravniPodnik.Data.DTO;

public class EmployeeHierarchyDTO
{
    [ColumnName("HLADINA")]
    public int Level { get; set; }

    [ColumnName("ID_ZAMESTNANCE")]
    public int EmployeeId { get; set; }

    [ColumnName("JMENO")]
    public string Name { get; set; }

    [ColumnName("PLAT")]
    public decimal Salary { get; set; }

    [ColumnName("PLATNOST_UVAZKU_DO")]
    public DateTime? ContractEndDate { get; set; }

    [ColumnName("HIERARCHIE")]
    public string Hierarchy { get; set; }
}