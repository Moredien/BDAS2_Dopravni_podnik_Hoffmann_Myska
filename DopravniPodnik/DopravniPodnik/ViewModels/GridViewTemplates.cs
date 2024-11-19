﻿using System.Collections.ObjectModel;
using DopravniPodnik.Data.DTO;
using DopravniPodnik.Data.Models;
using DopravniPodnik.ViewModels.Forms;

namespace DopravniPodnik.ViewModels;

public class DataGridColumnInfo
{
    public string Header { get; set; }
    public string BindingPath { get; set; }
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
                        new DataGridColumnInfo { Header = "Datum Narození", BindingPath = "datum_narozeni" },
                        new DataGridColumnInfo { Header = "Datum Založení", BindingPath = "datum_zalozeni" },
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
            default: return null;
        }
    }
}