using System.Collections.ObjectModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public class DataGridColumnInfo
{
    public string Header { get; set; }
    public string BindingPath { get; set; }
    public string Format { get; set; }
}

public class DataGridDataContext
{
    public Type ModelType { get; set; }
    public Type EditFormType { get; set; }
    public ObservableCollection<DataGridColumnInfo> Columns { get; }
    public DataGridDataContext(Type modelType,Type editFormType, ObservableCollection<DataGridColumnInfo> columns)
    {
        ModelType = modelType;
        EditFormType = editFormType;
        Columns = columns;
    }
}

public static class GridViewTemplates
{
    public static DataGridDataContext Get(Type modelType)
    {
        switch (modelType.Name)
        {
            case "UzivatelDTO":

                return new DataGridDataContext(
                    modelType,
                    typeof(UzivatelFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>
                    {
                        new DataGridColumnInfo { Header = "Username", BindingPath = "uzivatelske_jmeno" },
                        new DataGridColumnInfo { Header = "Jméno", BindingPath = "jmeno" },
                        new DataGridColumnInfo { Header = "Příjmení", BindingPath = "prijmeni" },
                        new DataGridColumnInfo { Header = "Datum Narození", BindingPath = "datum_narozeni" , Format = "dd.MM.yyyy"},
                        new DataGridColumnInfo { Header = "Datum Založení", BindingPath = "datum_zalozeni" ,Format = "dd.MM.yyyy" },
                        new DataGridColumnInfo { Header = "Typ Uživatele", BindingPath = "nazev_typ_uzivatele" }
                    }
                );
            case "TypyUzivatele":
                return new DataGridDataContext(
                    modelType,
                    typeof(TypyUzivateleFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>
                    {
                        new DataGridColumnInfo { Header = "Název", BindingPath = "Nazev" },
                    });
            case "Adresy":
                return new DataGridDataContext(
                    modelType,
                    typeof(AdresyFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Město", BindingPath = "Mesto" },
                        new DataGridColumnInfo { Header = "Ulice", BindingPath = "Ulice" },
                        new DataGridColumnInfo { Header = "Číslo popisné", BindingPath = "CisloPopisne" }
                        });
            case "Zamestnanci":
                return new DataGridDataContext(
                    modelType,
                    typeof(ZamestnanciFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Plat", BindingPath = "Plat" },
                        new DataGridColumnInfo { Header = "Platnost úvazku do", BindingPath = "PlatnostUvazkuDo" , Format = "dd.MM.yyyy"},
                        new DataGridColumnInfo { Header = "id nadřízeného", BindingPath = "IdNadrizeneho" }
                    });
            case "Foto":
                return new DataGridDataContext(
                    modelType,
                    null,
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Soubor", BindingPath = "JmenoSouboru" },
                        new DataGridColumnInfo { Header = "Datum přidání", BindingPath = "DatumPridani" , Format = "dd.MM.yyyy"},
                    });
            case "Jizdy":
                return new DataGridDataContext(
                    modelType,
                    null,
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Začátek", BindingPath = "Zacatek" },
                        new DataGridColumnInfo { Header = "Konec", BindingPath = "Konec" },
                    });
            case "KartyMhd":
                return new DataGridDataContext(
                    modelType,
                    null,
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Zůstatek", BindingPath = "Zustatek" },
                        new DataGridColumnInfo { Header = "Platnost od", BindingPath = "PlatnostOd" },
                        new DataGridColumnInfo { Header = "Platnost do", BindingPath = "PlatnostDo" },
                    });
            case "TypyVozidel":
                return new DataGridDataContext(
                    modelType,
                    typeof(TypyVozidelFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Název", BindingPath = "Nazev" },
                        new DataGridColumnInfo { Header = "Značka", BindingPath = "Znacka" },
                    });
            case "Ridici":
                return new DataGridDataContext(
                    modelType,
                    typeof(RidiciFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Jméno", BindingPath = "Jmeno" },
                        new DataGridColumnInfo { Header = "Příjmení", BindingPath = "Prijmeni" },
                    });
            case "Zastavky":
                return new DataGridDataContext(
                    modelType,
                    typeof(ZastavkyFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Jméno", BindingPath = "Jmeno" },
                    });
            case "TypyPredplatneho":
                return new DataGridDataContext(
                    modelType,
                    typeof(TypyPredplatnehoFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Jméno", BindingPath = "Jmeno" },
                        new DataGridColumnInfo { Header = "Cena", BindingPath = "Cena" },
                    });
            case "Logy":
                return new DataGridDataContext(
                    modelType,
                    typeof(LogyFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Čas", BindingPath = "Cas" },
                        new DataGridColumnInfo { Header = "Tabulka", BindingPath = "Tabulka" },
                        new DataGridColumnInfo { Header = "Operace", BindingPath = "Operace" },
                    });
            default: return null;
        }
    }
}