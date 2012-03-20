using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machete.Domain;
using System.Data.Entity;

namespace Machete.Data
{
    // static -- [ class-modifier ]
    //              cannot be instantiated, cannot be used as a type, can only contain
    //              static members
    public static class MacheteLookup    {
        public static List<Lookup> cache {get {return _cache;} }
        private static List<Lookup> _cache = new List<Lookup>
            {
                new Lookup { ID=1, category = "race", text_EN = "Afroamerican",   text_ES="Afroamericano",    selected = false },
                new Lookup { ID=2, category = "race", text_EN = "Asian",          text_ES="Asiano",           selected = false},
                new Lookup { ID=3, category = "race", text_EN = "Caucasian",      text_ES="Caucásico",        selected = false },
                new Lookup { ID=4, category = "race", text_EN = "Hawaiian",       text_ES="Hawaiano",         selected = false },
                new Lookup { ID=5, category = "race", text_EN = "Latino",         text_ES="Latino",           selected = true },
                new Lookup { ID=6, category = "race", text_EN = "Native American",text_ES="Nativo Americanos",selected = false},
                new Lookup { ID=7, category = "race", text_EN = "Other",          text_ES="Otro",             selected = false},
                new Lookup { ID=8, category = "language",     text_EN = "conversational (3)", text_ES = "conversacional (3)", level=3, selected=false},
                new Lookup { ID=9, category = "language",     text_EN = "limited(2)",        text_ES = "limitado (2)",       level=2, selected=false},
                new Lookup { ID=10, category = "language",     text_EN = "none (1)",           text_ES = "nada (1)",           level=1, selected=true},
                new Lookup { ID=11, category = "neighborhood",     text_EN = "Seattle",            text_ES = "Seattle",            selected=false},
                new Lookup { ID=12, category = "neighborhood",     text_EN = "Capitol Hill",       text_ES = "Capitol Hill",       selected=false},
                new Lookup { ID=13, category = "neighborhood",     text_EN = "Central District",   text_ES = "Central District",   selected=false},
                new Lookup { ID=14, category = "neighborhood",     text_EN = "South Park",         text_ES = "South Park",         selected=false},
                new Lookup { ID=15, category = "neighborhood",     text_EN = "Kent",               text_ES = "Kent",               selected=false},
                new Lookup { ID=16, category = "neighborhood",     text_EN = "Auburn",             text_ES = "Auburn",             selected=false},
                new Lookup { ID=78, category = "neighborhood",     text_EN = "Shoreline",          text_ES = "Shoreline",          selected=false},
                new Lookup { ID=17, category = "income",     text_EN = "Less than $15,000",            text_ES = "Menos que $15.000",          selected=true},
                new Lookup { ID=18, category = "income",     text_EN = "Between $15,000 and $25,000",  text_ES = "Entre $ 15.000 y $ 25.000",  selected=false},
                new Lookup { ID=19, category = "income",     text_EN = "Between $25,000 and $37,000",  text_ES = "Between $25,000 and $37,000",     selected=false},
                new Lookup { ID=84, category = "income",     text_EN = "Above $37,000",                text_ES = "Por encima de $ 37.000",     selected=false},
                new Lookup { ID=85, category = "income",     text_EN = "unknown",                      text_ES = "desconocido",     selected=false},
                new Lookup { ID=20, category = "worktype",     text_EN = "(DWC) Day Worker Center",    text_ES = "(DWC) Centro del Trabajadores", selected=true},
                new Lookup { ID=21, category = "worktype",     text_EN = "(HHH) Household Helpers",    text_ES = "(HHH) Trabajadoras de la casa", selected=false},
                new Lookup { ID=22, category = "worktype",     text_EN = "Special event",        text_ES = "Acto especial", selected=false},                
                new Lookup { ID=23, category = "emplrreference",    text_EN = "A friend",       text_ES = "Un amigo", selected=false},
                new Lookup { ID=24, category = "emplrreference",    text_EN = "Flyer",        text_ES = "Volante", selected=false},
                new Lookup { ID=25, category = "emplrreference",    text_EN = "Yelp",         text_ES = "Yelp", selected=false},
                new Lookup { ID=26, category = "emplrreference",    text_EN = "Angie's List", text_ES = "Lista de Angie", selected=false},
                new Lookup { ID=27, category = "emplrreference",    text_EN = "Other",        text_ES = "Otro", selected=false},
                new Lookup { ID=28, category = "emplrreference",    text_EN = "Facebook",     text_ES = "Facebook", selected=false},
                new Lookup { ID=29, category = "transportmethod",   text_EN = "Worker buses",       text_ES = "Trabajador en bus",    sortorder=2,  selected=true},
                new Lookup { ID=30, category = "transportmethod",   text_EN = "Worker drives",      text_ES = "Trabajador en carro",  sortorder=5,  selected=false},
                new Lookup { ID=31, category = "transportmethod",   text_EN = "Employer Picks up",  text_ES = "Patron lleva",         sortorder=5,  selected=false},
                new Lookup { ID=32, category = "transportmethod",   text_EN = "Casa Latina van",    text_ES = "Van de Casa Latina",   sortorder=1,  selected=false},
                new Lookup { ID=86, category = "transportmethod",   text_EN = "Worker walks",       text_ES = "Trabajador camina",    sortorder=5,  selected=false},
                new Lookup { ID=33, category = "maritalstatus",     text_EN = "Single", text_ES = "Individual", selected=true},
                new Lookup { ID=34, category = "maritalstatus",     text_EN = "Married", text_ES = "Casado", selected=false},
                new Lookup { ID=35, category = "maritalstatus",     text_EN = "Separated", text_ES = "Separado", selected=false},
                new Lookup { ID=36, category = "maritalstatus",     text_EN = "Widow/Widower", text_ES = "Viuda/Viudo", selected=false},
                new Lookup { ID=37, category = "maritalstatus",     text_EN = "Divorced", text_ES = "Divorciado", selected=false},
                new Lookup { ID=38, category = "gender",     text_EN = "Male", text_ES = "Masculino", selected=true},
                new Lookup { ID=39, category = "gender",     text_EN = "Female", text_ES = "Femenino", selected=false},
                new Lookup { ID=40, category = "gender",     text_EN = "Transgender", text_ES = "Transgénero", selected=false},
                new Lookup { ID=41, category = "gender",     text_EN = "Other", text_ES = "Otro", selected=false},
                new Lookup { ID=42, category = "orderstatus",     text_EN = "Active", text_ES = "Activo", selected=false},
                new Lookup { ID=43, category = "orderstatus",     text_EN = "Pending", text_ES = "Pendientes", selected=true},
                new Lookup { ID=44, category = "orderstatus",     text_EN = "Completed", text_ES = "Completado", selected=false},
                new Lookup { ID=45, category = "orderstatus",     text_EN = "Cancelled", text_ES = "Cancelado", selected=false},
                new Lookup { ID=46, category = "orderstatus",     text_EN = "Expired", text_ES = "Expirado", selected=false},
                new Lookup { ID=47, category = "countryoforigin",     text_EN = "USA", text_ES = "EE.UU", selected=false},
                new Lookup { ID=48, category = "countryoforigin",     text_EN = "Mexico", text_ES = "México", selected=true},
                new Lookup { ID=49, category = "countryoforigin",     text_EN = "Guatemala", text_ES = "Guatemala", selected=false},
                new Lookup { ID=50, category = "countryoforigin",     text_EN = "Honduras", text_ES = "Honduras", selected=false},
                new Lookup { ID=51, category = "countryoforigin",     text_EN = "Nicaragua", text_ES = "Nicaragua", selected=false},
                new Lookup { ID=52, category = "countryoforigin",     text_EN = "Costa Rica", text_ES = "Costa Rica", selected=false},
                new Lookup { ID=53, category = "countryoforigin",     text_EN = "Panama", text_ES = "Panamá", selected=false},
                new Lookup { ID=54, category = "countryoforigin",     text_EN = "Columbia", text_ES = "Columbia", selected=false},
                new Lookup { ID=55, category = "countryoforigin",     text_EN = "Peru", text_ES = "Perú", selected=false},
                new Lookup { ID=56, category = "countryoforigin",     text_EN = "Venezuala", text_ES = "Venezuala", selected=false},
                new Lookup { ID=57, category = "countryoforigin",     text_EN = "Ecuador", text_ES = "Ecuador", selected=false},
                new Lookup { ID=58, category = "countryoforigin",     text_EN = "Bolivia", text_ES = "Bolivia", selected=false},
                new Lookup { ID=59, category = "countryoforigin",     text_EN = "Chile", text_ES = "Chile", selected=false},
                new Lookup { ID=75, category = "countryoforigin",     text_EN = "Cuba", text_ES = "Cuba", selected=false},
                new Lookup { ID=76, category = "countryoforigin",     text_EN = "El Salvador", text_ES = "El Salvador", selected=false},
                new Lookup { ID=79, category = "countryoforigin",     text_EN = "India", text_ES = "India", selected=false},
                new Lookup { ID=80, category = "countryoforigin",     text_EN = "Russia", text_ES = "Rusia", selected=false},
                new Lookup { ID=81, category = "countryoforigin",     text_EN = "Argentina", text_ES = "Argentina", selected=false},
                new Lookup { ID=82, category = "countryoforigin",     text_EN = "Brazil", text_ES = "Brasil", selected=false},
                new Lookup { ID=60, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=5, wage=12,    sortorder=2,  text_EN = "general labor",                               text_ES = "general del trabajo",                      selected=true},
                new Lookup { ID=61, category = "skill", typeOfWorkID=20, speciality=true,   ltrCode="P", minHour=5, wage=15,    sortorder=5,  text_EN = "painter (rollerbrush)",                       text_ES = "pintor (rollerbrush)",                     selected=false, subcategory="paint", level=1 },
                new Lookup { ID=62, category = "skill", typeOfWorkID=20, speciality=true,   ltrCode="P", minHour=5, wage=18,    sortorder=5,  text_EN = "painter (spray)",                             text_ES = "pintor (spray)",                           selected=false, subcategory="paint", level=2 },
                new Lookup { ID=63, category = "skill", typeOfWorkID=20, speciality=true,   ltrCode="B", minHour=5, wage=15,    sortorder=9,  text_EN = "insulation / sheetrock / taping / drywall",   text_ES = "aislamiento / yeso / grabación / yeso",    selected=false, subcategory="build", level=1 },
                new Lookup { ID=64, category = "skill", typeOfWorkID=20, speciality=true,   ltrCode="F", minHour=5, wage=18,    sortorder=9,  text_EN = "Build retaining wall / fence",                text_ES = "Construir muro / valla",                   selected=false, subcategory="fence", level=1 },
                new Lookup { ID=65, category = "skill", typeOfWorkID=20, speciality=true,   ltrCode="C", minHour=5, wage=18,    sortorder=9,  text_EN = "carpentry (siding / framing)",                text_ES = "carpintería (revestimiento / encuadre)",   selected=false, subcategory="carpentry", level=1 },
                new Lookup { ID=66, category = "skill", typeOfWorkID=20, speciality=true,   ltrCode="B", minHour=5, wage=18,    sortorder=9,  text_EN = "brick / masonry / tile setting",              text_ES = "ladrillo / hormigón / baldosa ajuste",     selected=false, subcategory="build", level=2 },
                new Lookup { ID=67, category = "skill", typeOfWorkID=21, speciality=false,  ltrCode="", minHour=5, wage=15,    sortorder=3,  text_EN = "HHH housework",                               text_ES = "HHH trabajo de casa",                      selected=false},
                new Lookup { ID=68, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=5, wage=15,    sortorder=5,  text_EN = "Moving",                                      text_ES = "Movimiento de casa",                       selected=false},
                new Lookup { ID=69, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="G", minHour=5, wage=15,    sortorder=4,  text_EN = "Gardening",                                   text_ES = "Jardinería",                               selected=false,subcategory="garden", level=1},
                new Lookup { ID=70, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=5, wage=13,    sortorder=1,  text_EN = "Digging/Weeding",                             text_ES = "Excavación / deshierbar",                  selected=false},
                new Lookup { ID=71, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=1, wage=30,    sortorder=9,  text_EN = "DWC Chambita 1hr",                            text_ES = "DWC Chambita 1hr",       fixedJob=true, selected=false},
                new Lookup { ID=72, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=2, wage=20,    sortorder=9,  text_EN = "DWC Chambita 2hr",                            text_ES = "DWC Chambita 2hr",       fixedJob=true, selected=false},
                new Lookup { ID=73, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=3, wage=16.66666,sortorder=9,  text_EN = "DWC Chambita 3hr",                           text_ES = "DWC Chambita 3hr",       fixedJob=true, selected=false},
                new Lookup { ID=74, category = "skill", typeOfWorkID=21, speciality=false,  ltrCode="", minHour=2, wage=25,    sortorder=9,  text_EN = "HHH Chambita 2hr",                            text_ES = "HHH Chambita 2hr",       fixedJob=true, selected=false},
                new Lookup { ID=77, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=5, wage=15,    sortorder=9,  text_EN = "Demolition",                                  text_ES = "Demolición",             fixedJob=false, selected=false},
                new Lookup { ID=83, category = "skill", typeOfWorkID=20, speciality=false,  ltrCode="", minHour=5, wage=13,    sortorder=1,  text_EN = "Yardwork",                                    text_ES = "trabajar en el jardín",  fixedJob=false, selected=false},
                new Lookup { ID=87, category = "skill", typeOfWorkID=21, speciality=false,  ltrCode="", minHour=3, wage=16.66666,    sortorder=9,  text_EN = "HHH Chambita 3hr",                            text_ES = "HHH Chambita 3hr",  fixedJob=true, selected=false},
                new Lookup { ID=88, category = "skill", typeOfWorkID=20, speciality=true,  ltrCode="G", minHour=5, wage=99999, sortorder=9, text_EN = "Landscaping", text_ES = "Paisajismo", fixedJob=false, selected=false, subcategory="garden", level=2},
                new Lookup { ID=89, category = "skill", typeOfWorkID=20, speciality=true,  ltrCode="R", minHour=5, wage=99999, sortorder=9, text_EN = "Roofing", text_ES = "Techado", fixedJob=false, selected=false, subcategory="roof", level=1},
                new Lookup { ID=90, category = "eventtype", text_EN="Recommendation",   text_ES="recomendación",    selected = true },
                new Lookup { ID=91, category = "eventtype", text_EN="Complaint",        text_ES="Queja",            selected = false },
                new Lookup { ID=92, category = "eventtype", text_EN="Sanction",         text_ES="Sanción",          selected = false },
                new Lookup { ID=93, category = "memberstatus", text_EN="Active",        text_ES="Activo",           selected = true },
                new Lookup { ID=94, category = "memberstatus", text_EN="Inactive",      text_ES="Inactivo",         selected = false },
                new Lookup { ID=95, category = "memberstatus", text_EN="Sanctioned",    text_ES="Sancionado",       selected = false },
                new Lookup { ID=96, category = "memberstatus", text_EN="Expired",       text_ES="Expirado",         selected = false },
                new Lookup { ID=97, category = "memberstatus", text_EN="Expelled",      text_ES="Expulsado",       selected = false },
                new Lookup { ID=98, category = "activityName", text_EN="Basic English",      text_ES="Ingles basico",       selected = true },
                new Lookup { ID=99, category = "activityName", text_EN="Intermediate English",      text_ES="Ingles intermedio",       selected = false },
                new Lookup { ID=100,category = "eventtype", text_EN="Explusion",         text_ES="Expulsión",          selected = false },
                new Lookup { ID=101,category = "activityType", text_EN="Class",         text_ES="Clase",          selected = true },
                new Lookup { ID=102,category = "activityType", text_EN="Assembly",         text_ES="Asamblea",          selected = false },
                new Lookup { ID=103,category = "activityType", text_EN="Volunteering",         text_ES="Voluntariado",          selected = false },
                new Lookup { ID=104,category = "activityType", text_EN="Health & Safety",         text_ES="Salud y seguridad",          selected = false }
            };
        //
        //
        public static void Initialize(MacheteContext context) {
            _cache.ForEach(u => context.Lookups.Add(u));
            context.SaveChanges();
        }
    }    
}