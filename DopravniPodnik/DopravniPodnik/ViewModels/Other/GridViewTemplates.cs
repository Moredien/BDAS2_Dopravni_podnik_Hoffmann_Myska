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
    public bool? IsMaskable { get; set; }
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
                        new DataGridColumnInfo { Header = "Datum Narození", BindingPath = "datum_narozeni" , Format = "dd.MM.yyyy", IsMaskable = true},
                        new DataGridColumnInfo { Header = "Datum Založení", BindingPath = "cas_zalozeni" ,Format = "dd.MM.yyyy" },
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
                        new DataGridColumnInfo { Header = "Platnost od", BindingPath = "PlatnostOd" , Format = "dd.MM.yyyy"},
                        new DataGridColumnInfo { Header = "Platnost do", BindingPath = "PlatnostDo" , Format = "dd.MM.yyyy"},
                    });
            case "TypyVozidel":
                return new DataGridDataContext(
                    modelType,
                    typeof(TypyVozidelFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Název", BindingPath = "Nazev" }
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
            case "Linky":
                return new DataGridDataContext(
                    modelType,
                    typeof(LinkyFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Číslo", BindingPath = "CisloLinky" },
                        new DataGridColumnInfo { Header = "Jméno", BindingPath = "Jmeno" },
                    });
            case "JizdyViewDTO":
                return new DataGridDataContext(
                    modelType,
                    typeof(JizdyFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Linka", BindingPath = "CisloLinky" },
                        new DataGridColumnInfo { Header = "Jméno linky", BindingPath = "JmenoLinky" },
                        new DataGridColumnInfo { Header = "Začátek", BindingPath = "Zacatek" , Format = "dd.MM.yyyy HH:mm"},
                        new DataGridColumnInfo { Header = "Konec", BindingPath = "Konec" , Format = "dd.MM.yyyy HH:mm"},
                        new DataGridColumnInfo { Header = "Typ Vozidla", BindingPath = "TypVozidla" },
                        new DataGridColumnInfo { Header = "Značka vozidla", BindingPath = "ZnackaVozidla" },
                        new DataGridColumnInfo { Header = "Jméno řidiče", BindingPath = "JmenoRidice" },
                        new DataGridColumnInfo { Header = "Příjmení řidiče", BindingPath = "PrijmeniRidice" },
                    });
            case "ZastavkyLinkyViewDTO":
                return new DataGridDataContext(
                    modelType,
                    typeof(ZastavkyLinkyViewFormViewModel),
                    new ObservableCollection<DataGridColumnInfo>{
                        new DataGridColumnInfo { Header = "Číslo linky", BindingPath = "CisloLinky" },
                        new DataGridColumnInfo { Header = "Jméno zastávky", BindingPath = "JmenoZastavky" },
                        new DataGridColumnInfo { Header = "Odjezd", BindingPath = "Odjezd" , Format = "dd.MM.yyyy HH:mm"},
                    });
            default: return null;
        }
    }
}