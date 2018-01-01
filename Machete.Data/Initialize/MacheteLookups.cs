#region COPYRIGHT
// File:     MacheteLookups.cs
// Author:   Savage Learning, LLC.
// Created:  2012/06/17 
// License:  GPL v3
// Project:  Machete.Data
// Contact:  savagelearning
// 
// Copyright 2011 Savage Learning, LLC., all rights reserved.
// 
// This source file is free software, under either the GPL v3 license or a
// BSD style license, as supplied with this software.
// 
// This source file is distributed in the hope that it will be useful, but 
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
// or FITNESS FOR A PARTICULAR PURPOSE. See the license files for details.
//  
// For details please refer to: 
// http://www.savagelearning.com/ 
//    or
// http://www.github.com/jcii/machete/
// 
#endregion
using Machete.Domain;
using System;
using System.Collections.Generic;

namespace Machete.Data
{
    // static -- [ class-modifier ]
    //              cannot be instantiated, cannot be used as a type, can only contain
    //              static members
    public static class MacheteLookup    {
        /// <summary>
        /// There have been multiple renditions of the Lookups table and a form of cache, as I learned
        /// C#, .Net, and MVC. This list of objects is used to seed the database and to provide 
        /// real'ish values in unit & integration tests. It should not be used in the application
        /// functionality because the customer may change teh values to suit their purposes.
        /// 
        /// For actual Machete functionality, The Service Layer's LookupCache object should be used.
        /// It gets values from the database, which may be different from these static values.
        /// 
        /// Also, please note that if you edit this list for any reason (such as to increase flexibility
        /// for center implementations, or to make corrections) make sure those changes are reflected in
        /// Records.cs, otherwise you will break the tests. (Updated 4/1/14 Chaim)
        /// </summary>
        public static List<Lookup> cache {get {return _cache;} }
        private static List<Lookup> _cache = new List<Lookup>
            {
                //new Lookup { ID=0, category = "null", text_EN = "null",   text_ES="null",    selected = false },
                new Lookup { ID=1, category = LCategory.race, text_EN = "Afroamerican",   text_ES="Afroamericano",    selected = false },
                new Lookup { ID=2, category = LCategory.race, text_EN = "Asian",          text_ES="Asiano",           selected = false},
                new Lookup { ID=3, category = LCategory.race, text_EN = "Caucasian",      text_ES="Caucásico",        selected = false },
                new Lookup { ID=4, category = LCategory.race, text_EN = "Hawaiian",       text_ES="Hawaiano",         selected = false },
                new Lookup { ID=5, category = LCategory.race, text_EN = "Latino",         text_ES="Latino",           selected = true },
                new Lookup { ID=6, category = LCategory.race, text_EN = "Native American",text_ES="Nativo Americanos",selected = false },
                new Lookup { ID=7, category = LCategory.race, text_EN = "Other",          text_ES="Otro",             selected = false },
                new Lookup { ID=8, category = LCategory.language,     text_EN = "fluent (3)",          text_ES = "habla con fluidez (3)", level=3, selected=false },
                new Lookup { ID=9, category = LCategory.language,     text_EN = "conversational (2)",  text_ES = "conversacional (2)",    level=2, selected=false },
                new Lookup { ID=10, category = LCategory.language,    text_EN = "limited (1)",         text_ES = "muy poco (1)",          level=1, selected=true },
                new Lookup { ID=11, category = LCategory.language,    text_EN = "none (0)",            text_ES = "nada (1)",              level=0, selected=true },
                new Lookup { ID=12, category = LCategory.neighborhood,     text_EN = "Primary City",             text_ES = "Ciudad Primaria",            selected=true },
                new Lookup { ID=13, category = LCategory.income,     text_EN = "Poor (Less than $15,000)",     text_ES = "Menos que $15.000",          selected=true },
                new Lookup { ID=14, category = LCategory.income,     text_EN = "Between $15,000 and $25,000",  text_ES = "Entre $ 15.000 y $ 25.000",  selected=false },
                new Lookup { ID=15, category = LCategory.income,     text_EN = "Between $25,000 and $37,000",  text_ES = "Between $25,000 and $37,000",     selected=false },
                new Lookup { ID=16, category = LCategory.income,     text_EN = "Above $37,000",                text_ES = "Por encima de $ 37.000",     selected=false },
                new Lookup { ID=17, category = LCategory.income,     text_EN = "Unknown",                      text_ES = "Desconocido",     selected=false },
                new Lookup { ID=18, category = LCategory.worktype, key=LWorkType.DWC,     text_EN = "Day Worker Center",        text_ES = "Trabajadores (Jornaleros)", ltrCode="DWC", selected=true },
                new Lookup { ID=19, category = LCategory.worktype, key=LWorkType.HHH,     text_EN = "Domestic Workers Program", text_ES = "Trabajadores domésticos", ltrCode="HHH", selected=false },
                new Lookup { ID=20, category = LCategory.worktype, key=LWorkType.EVT,     text_EN = "Special event",            text_ES = "Evento especial", ltrCode="EVT", selected=false },
                new Lookup { ID=21, category = LCategory.emplrreference,    text_EN = "Flier",        text_ES = "Volante", selected=true },
                new Lookup { ID=22, category = LCategory.emplrreference,    text_EN = "Friend",       text_ES = "Un amigo", selected=false },
                new Lookup { ID=23, category = LCategory.emplrreference,    text_EN = "Website",      text_ES = "Sitio red", selected=false },
                new Lookup { ID=24, category = LCategory.emplrreference,    text_EN = "Facebook",     text_ES = "Facebook", selected=false },
                new Lookup { ID=25, category = LCategory.emplrreference,    text_EN = "Other",        text_ES = "Otro", selected=false},
                new Lookup { ID=26, category = LCategory.transportmethod,   key="transport_bus",     text_EN = "Worker buses",       text_ES = "Trabajador en bus",    sortorder=2,  selected=true},
                new Lookup { ID=27, category = LCategory.transportmethod,   key="transport_car",   text_EN = "Worker drives",      text_ES = "Trabajador en carro",  sortorder=5,  selected=false },
                new Lookup { ID=28, category = LCategory.transportmethod,   key="transport_pickup",   text_EN = "Employer Picks up",  text_ES = "Patron lleva",         sortorder=5,  selected=false },
                new Lookup { ID=32, category = LCategory.transportmethod,   key="transport_van",   text_EN = "Van service",    text_ES = "Servicio de van",   sortorder=1,  selected=false },
                new Lookup { ID=30, category = LCategory.transportmethod,   key="transport_walks",   text_EN = "Worker walks",       text_ES = "Trabajador camina",    sortorder=5,  selected=false },
                new Lookup { ID=31, category = LCategory.maritalstatus,     text_EN = "Single", text_ES = "Individual", selected=true },
                new Lookup { ID=29, category = LCategory.maritalstatus,     text_EN = "Married", text_ES = "Casado", selected=false },
                new Lookup { ID=33, category = LCategory.maritalstatus,     text_EN = "Separated", text_ES = "Separado", selected=false },
                new Lookup { ID=34, category = LCategory.maritalstatus,     text_EN = "Widow/Widower", text_ES = "Viuda/Viudo", selected=false },
                new Lookup { ID=35, category = LCategory.maritalstatus,     text_EN = "Divorced", text_ES = "Divorciado", selected=false },
                new Lookup { ID=36, category = LCategory.gender,     text_EN = "Male", text_ES = "Masculino", selected=true },
                new Lookup { ID=37, category = LCategory.gender,     text_EN = "Female", text_ES = "Femenino", selected=false },
                new Lookup { ID=38, category = LCategory.gender,     text_EN = "Transgender", text_ES = "Transgénero", selected=false },
                new Lookup { ID=39, category = LCategory.gender,     text_EN = "Other", text_ES = "Otro", selected=false },
                new Lookup { ID=40, category = LCategory.orderstatus,  key=LOrderStatus.Active,   text_EN = "Active", text_ES = "Activo", selected=false},
                new Lookup { ID=41, category = LCategory.orderstatus,  key=LOrderStatus.Pending,   text_EN = "Pending", text_ES = "Pendientes", selected=true},
                new Lookup { ID=42, category = LCategory.orderstatus,  key=LOrderStatus.Completed,   text_EN = "Completed", text_ES = "Completado", selected=false},
                new Lookup { ID=43, category = LCategory.orderstatus,  key=LOrderStatus.Cancelled,   text_EN = "Cancelled", text_ES = "Cancelado", selected=false},
                new Lookup { ID=44, category = LCategory.orderstatus,  key=LOrderStatus.Expired,   text_EN = "Expired", text_ES = "Expirado", selected=false},
                new Lookup { ID=45, category = LCategory.countryoforigin,     text_EN = "USA", text_ES = "EE.UU", selected=false},
                new Lookup { ID=46, category = LCategory.countryoforigin,     text_EN = "Mexico", text_ES = "México", selected=true},
                new Lookup { ID=47, category = LCategory.countryoforigin,     text_EN = "Guatemala", text_ES = "Guatemala", selected=false},
                new Lookup { ID=48, category = LCategory.countryoforigin,     text_EN = "El Salvador", text_ES = "El Salvador", selected=false},
                new Lookup { ID=49, category = LCategory.countryoforigin,     text_EN = "Honduras", text_ES = "Honduras", selected=false},
                new Lookup { ID=50, category = LCategory.countryoforigin,     text_EN = "Belize", text_ES = "Belice", selected=false},
                new Lookup { ID=51, category = LCategory.countryoforigin,     text_EN = "Nicaragua", text_ES = "Nicaragua", selected=false},
                new Lookup { ID=52, category = LCategory.countryoforigin,     text_EN = "Costa Rica", text_ES = "Costa Rica", selected=false},
                new Lookup { ID=53, category = LCategory.countryoforigin,     text_EN = "Panama", text_ES = "Panamá", selected=false},
                new Lookup { ID=54, category = LCategory.countryoforigin,     text_EN = "Columbia", text_ES = "Colombia", selected=false},
                new Lookup { ID=55, category = LCategory.countryoforigin,     text_EN = "Peru", text_ES = "Perú", selected=false},
                new Lookup { ID=56, category = LCategory.countryoforigin,     text_EN = "Venezuela", text_ES = "Venezuala", selected=false},
                new Lookup { ID=57, category = LCategory.countryoforigin,     text_EN = "Ecuador", text_ES = "Ecuador", selected=false},
                new Lookup { ID=58, category = LCategory.countryoforigin,     text_EN = "Bolivia", text_ES = "Bolivia", selected=false},
                new Lookup { ID=59, category = LCategory.countryoforigin,     text_EN = "Chile", text_ES = "Chile", selected=false},
                new Lookup { ID=60, category = LCategory.countryoforigin,     text_EN = "Argentina", text_ES = "Argentina", selected=false},
                new Lookup { ID=61, category = LCategory.countryoforigin,     text_EN = "Cuba", text_ES = "Cuba", selected=false},
                new Lookup { ID=62, category = LCategory.countryoforigin,     text_EN = "Dominican Republic", text_ES = "República Dominicana", selected=false},
                new Lookup { ID=63, category = LCategory.skill, key="skill_general_labor",          typeOfWorkID=20, speciality=false,  ltrCode="", minHour=1, wage=15,    sortorder=2,  text_EN = "general labor",             text_ES = "trabajo general",                      selected=true },
                new Lookup { ID=64, category = LCategory.skill, key="skill_deep_cleaning",          typeOfWorkID=21, speciality=false,  ltrCode="", minHour=1, wage=15,    sortorder=2,  text_EN = "housecleaning",             text_ES = "trabajo domÃ©stico", selected=false },
                new Lookup { ID=65, category = LCategory.skill, key="skill_painting_rollerbrush",   typeOfWorkID=20, speciality=true,   ltrCode="P", minHour=1, wage=15,    sortorder=5,  text_EN = "painter (rollerbrush)",    text_ES = "pintor (cepillo)",   selected=false, subcategory="paint", level=1 },
                new Lookup { ID=66, category = LCategory.skill, key="skill_painting_spray",         typeOfWorkID=20, speciality=true,   ltrCode="P", minHour=1, wage=15,    sortorder=5,  text_EN = "painter (spray)",          text_ES = "pintor (mÃ¡quina)",   selected=false, subcategory="paint", level=2 },
                new Lookup { ID=67, category = LCategory.skill, key="skill_drywall",                typeOfWorkID=20, speciality=true,   ltrCode="B", minHour=1, wage=15,    sortorder=9,  text_EN = "drywall (patch/tape)",     text_ES = "yeso (arreglar o poner cinta)",           selected=false, subcategory="build", level=1 },
                new Lookup { ID=68, category = LCategory.skill, key="skill_drywall_adv",            typeOfWorkID=20, speciality=true,   ltrCode="B", minHour=1, wage=15,    sortorder=9,  text_EN = "drywall (hang)",           text_ES = "yeso (colgar)",                                 selected=false, subcategory="build", level=2 },
                new Lookup { ID=69, category = LCategory.skill, key="skill_insulation",             typeOfWorkID=20, speciality=true,   ltrCode="B", minHour=1, wage=15,    sortorder=9,  text_EN = "insulation",               text_ES = "aislamiento",                                       selected=false, subcategory="build", level=3 },
                new Lookup { ID=70, category = LCategory.skill, key="skill_carpentry",              typeOfWorkID=20, speciality=true,   ltrCode="C", minHour=1, wage=15,    sortorder=9,  text_EN = "carpentry (siding / framing)", text_ES = "carpinterÃ­a (revestimiento / encuadre)",   selected=false, subcategory="carpentry", level=1 },
                new Lookup { ID=71, category = LCategory.skill, key="skill_masonry",                typeOfWorkID=20, speciality=true,   ltrCode="C", minHour=1, wage=15,    sortorder=9,  text_EN = "masonry",                  text_ES = "ladrillo y ajuste de baldosas",     selected=false, subcategory="carpentry", level=2 },
                new Lookup { ID=72, category = LCategory.skill, key="skill_moving",                 typeOfWorkID=20, speciality=false,  ltrCode="", minHour=1, wage=15,    sortorder=5,  text_EN = "Moving",                    text_ES = "Mudanza",                       selected=false},
                new Lookup { ID=73, category = LCategory.skill, key="skill_adv_gardening",          typeOfWorkID=20, speciality=false,  ltrCode="G", minHour=1, wage=15,    sortorder=4,  text_EN = "Gardening",                text_ES = "JardinerÃ­a avanzada",                               selected=false,subcategory="garden", level=2},
                new Lookup { ID=74, category = LCategory.skill, key="skill_yardwork",               typeOfWorkID=20, speciality=false,  ltrCode="G", minHour=1, wage=15,    sortorder=1,  text_EN = "Yardwork",                 text_ES = "trabajar en el jardÃ­n",  subcategory="garden", level=2, selected=false},
                new Lookup { ID=75, category = LCategory.skill, key="skill_landscaping",            typeOfWorkID=20, speciality=true,   ltrCode="G", minHour=1, wage=15, sortorder=9, text_EN = "Landscaping", text_ES = "Paisajismo", fixedJob=false, selected=false, subcategory="garden", level=3},
                new Lookup { ID=76, category = LCategory.skill, key="skill_roofing",                typeOfWorkID=20, speciality=true,   ltrCode="R", minHour=1, wage=15, sortorder=9, text_EN = "Roofing", text_ES = "Techado", fixedJob=false, selected=false, subcategory="roof", level=1},
                new Lookup { ID=77, category = LCategory.eventtype, text_EN="Recommendation", text_ES="Recomendación", selected = true },
                new Lookup { ID=78, category = LCategory.eventtype, text_EN="Certificate", text_ES="Certificado",    selected = false },
                new Lookup { ID=79, category = LCategory.eventtype, text_EN="Complaint", text_ES="Queja",            selected = false },
                new Lookup { ID=80, category = LCategory.eventtype, text_EN="Sanction", text_ES="Sanción",           selected = false },
                new Lookup { ID=81,category = LCategory.eventtype,    text_EN="Explusion",         text_ES="Expulsión",          selected = false },
                new Lookup { ID=82, category = LCategory.memberstatus, key = LMemberStatus.Active, text_EN=LMemberStatus.Active,        text_ES="Activo",           selected = true },
                new Lookup { ID=83, category = LCategory.memberstatus, key = LMemberStatus.Inactive, text_EN=LMemberStatus.Inactive,      text_ES="Inactivo",         selected = false },
                new Lookup { ID=84, category = LCategory.memberstatus, key = LMemberStatus.Sanctioned, text_EN=LMemberStatus.Sanctioned,    text_ES="Sancionado",       selected = false },
                new Lookup { ID=85, category = LCategory.memberstatus, key = LMemberStatus.Expired, text_EN=LMemberStatus.Expired,       text_ES="Expirado",         selected = false },
                new Lookup { ID=86, category = LCategory.memberstatus, key = LMemberStatus.Expelled, text_EN=LMemberStatus.Expelled,      text_ES="Expulsado",       selected = false },
                new Lookup { ID=87,category = LCategory.activityType,  key = LActType.Class,    text_EN=LActType.Class,     text_ES="Clase",          selected = true },
                new Lookup { ID=88,category = LCategory.activityType,  key = LActType.Workshop,    text_EN=LActType.Workshop,     text_ES="Taller",          selected = false },
                new Lookup { ID=89,category = LCategory.activityType,  key = LActType.Assembly, text_EN=LActType.Assembly,  text_ES="Asamblea",          selected = false },
                new Lookup { ID=90, category = LCategory.activityType, key = LActType.OrgMtg,   text_EN=LActType.OrgMtg,    text_ES="Reunión de Organizadores", selected = false },
                new Lookup { ID=91, category = LCategory.activityName, text_EN="Basic English",      text_ES="Inglés basico",       selected = true },
                new Lookup { ID=92, category = LCategory.activityName, text_EN="Intermediate English",      text_ES="Inglés intermedio",       selected = false },
                new Lookup { ID=93,category = LCategory.activityName, text_EN="Volunteering",         text_ES="Voluntariado",          selected = false },
                new Lookup { ID=94,category = LCategory.activityName, text_EN="OSHA Training", text_ES="Capacitación", selected = false},
                new Lookup { ID=95,category = LCategory.activityName, text_EN="Health & Safety",         text_ES="Salud y seguridad",          selected = false },
                new Lookup { ID=96,category = LCategory.activityName, text_EN="Leadership Development", text_ES="Liderazgo", selected = false},
                new Lookup { ID=97,category = LCategory.activityName, text_EN="Basic Gardening", text_ES="Jardinería Básica", selected = false},
                new Lookup { ID=98,category = LCategory.activityName, text_EN="Advanced Gardening", text_ES="Jardinería Avanzada", selected = false},
                new Lookup { ID=99,category = LCategory.activityName, text_EN="Financial Education", text_ES="Educación Fiscal", selected = false},
                new Lookup { ID=100,category = LCategory.activityName, key = LActName.Assembly, text_EN=LActName.Assembly, text_ES="Asamblea", selected = false },
                new Lookup { ID=101,category = LCategory.activityName, key = LActName.OrgMtg,   text_EN=LActName.OrgMtg,   text_ES="Reunión de Organizadores", selected = false },
                new Lookup { ID=102,category = LCategory.emailstatus, key= LEmailStatus.Sent, text_EN="Sent",         text_ES="Enviado",          selected = false },
                new Lookup { ID=103,category = LCategory.emailstatus, key= LEmailStatus.TransmitError, text_EN="Transmit error",         text_ES="Error en la transmisión",          selected = false },
                new Lookup { ID=104,category = LCategory.emailstatus, key= LEmailStatus.Sending, text_EN="Sending",         text_ES="Mandando...",          selected = false },
                new Lookup { ID=105,category = LCategory.emailstatus, key= LEmailStatus.Pending, text_EN="Pending",         text_ES="Pendiente",          selected = false },
                new Lookup { ID=106,category = LCategory.emailTemplate, key= LKey.Default, text_EN="default template (English)", text_ES="plantilla predeterminada (Ingles)", selected=true, emailTemplate="Thanks for using Machete! You can change this in the Config section."},
                new Lookup { ID=107,category = LCategory.emailstatus, key= LEmailStatus.ReadyToSend, text_EN="Ready to send",         text_ES="Listo para enviar",          selected = false },
                new Lookup { ID=255,category = LCategory.transportTransactType, text_EN="Cash",         text_ES="Cash",          selected = true },
                new Lookup { ID=256,category = LCategory.transportTransactType, text_EN="Paypal",         text_ES="Paypal",          selected = false },
                new Lookup { ID=257,category = LCategory.transportTransactType, text_EN="Check",         text_ES="Check",          selected = false },
                new Lookup { ID=258,category = LCategory.transportTransactType, text_EN="Credit Card",         text_ES="Credit Card",          selected = false },

                // OnlineOrdering related lookups:

                new Lookup { ID=263,category = LCategory.workerRating, text_EN="1-Unacceptable experience", text_ES="1-Unacceptable experience", selected=false },
                new Lookup { ID=264,category = LCategory.workerRating, text_EN="2-Needs improvement", text_ES="2-Needs improvement", selected=false },
                new Lookup { ID=265,category = LCategory.workerRating, text_EN="3-Okay Experience", text_ES="3-Okay Experience", selected=false },
                new Lookup { ID=266,category = LCategory.workerRating, text_EN="4-Would Recommend", text_ES="4-Would Recommend", selected=false },
                new Lookup { ID=267,category = LCategory.workerRating, text_EN="5-Excellent Experience", text_ES="5-Excellent Experience", selected=true },

                // Demographic related lookups:

                new Lookup { ID=269,category = LCategory.housingType, text_EN="Rent", text_ES="Rent", selected=true },
                new Lookup { ID=270,category = LCategory.housingType, text_EN="Own", text_ES="Own", selected=false },
                new Lookup { ID=271,category = LCategory.vehicleTypeID, text_EN="Truck", text_ES="Truck", selected=true },
                new Lookup { ID=272,category = LCategory.vehicleTypeID, text_EN="Car", text_ES="Car", selected=false },
                new Lookup { ID=273,category = LCategory.incomeSourceID, text_EN="No Income", text_ES="No Income", selected=true },
                new Lookup { ID=274,category = LCategory.incomeSourceID, text_EN="Employment", text_ES="Employment", selected=false },
                new Lookup { ID=275,category = LCategory.incomeSourceID, text_EN="Federal or State Benefits", text_ES="Federal or State Benefits", selected=false },
                new Lookup { ID=276,category = LCategory.usBornChildren, text_EN="No", text_ES="No", selected=true },
                new Lookup { ID=277,category = LCategory.usBornChildren, text_EN="Yes", text_ES="Yes", selected=false },
                new Lookup { ID=278,category = LCategory.usBornChildren, text_EN="Refuse to Answer", text_ES="Refuse to Answer", selected=false },
                new Lookup { ID=279,category = LCategory.educationLevel, text_EN="Less Than High School", text_ES="Less Than High School", selected=true },
                new Lookup { ID=280,category = LCategory.educationLevel, text_EN="High School", text_ES="High School", selected=false },
                new Lookup { ID=281,category = LCategory.educationLevel, text_EN="Some College", text_ES="Some College", selected=false },
                new Lookup { ID=282,category = LCategory.educationLevel, text_EN="Bachelors", text_ES="Bachelors", selected=false },
                new Lookup { ID=283,category = LCategory.educationLevel, text_EN="Masters and Above", text_ES="Masters and Above", selected=false },
                new Lookup { ID=284,category = LCategory.farmLabor, text_EN="Farmer", text_ES="Farmer", selected=true },
                new Lookup { ID=285,category = LCategory.farmLabor, text_EN="Migrant Farm Worker", text_ES="Migrant Farm Worker", selected=false },
                new Lookup { ID=286,category = LCategory.farmLabor, text_EN="Seasonal Farm Worker", text_ES="Seasonal Farm Worker", selected=false },
                new Lookup { ID=287,category = LCategory.training, text_EN="OSHA", text_ES="OSHA", selected=true },
            };
        //
        //
        public static void Initialize(MacheteContext context) {
            _cache.ForEach(u => {
                u.datecreated = DateTime.Now;
                u.dateupdated = DateTime.Now;
                u.createdby = "Init T. Script";
                u.updatedby = "Init T. Script";
                context.Lookups.Add(u); 
            });
            context.Commit();
        }
    }    
}