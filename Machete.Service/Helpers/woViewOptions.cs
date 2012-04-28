using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace Machete.Service.Helpers
{
    public class woViewOptions
    {
        public CultureInfo CI     { get; set; }
        public string search { get; set; }
        public int? EmployerID { get; set; }
        public int? status { get; set; }
        public bool orderDescending { get; set; }
        public int displayStart { get; set; }
        public int displayLength { get; set; }
        public string sortColName { get; set; }
        public bool showOrdersPending { get; set; }
        public bool showOrdersWorkers { get; set; }
        public bool showInactiveWorker { get; set; }
        public bool showSanctionedWorker { get; set; }
        public bool showExpiredWorker { get; set; }
        public bool showExpelledWorker { get; set; }

        public woViewOptions() { }

        public woViewOptions(object o)
        {
            Type objectType = o.GetType();

            ConstructorInfo[] info = objectType.GetConstructors();
            MethodInfo[] methods = objectType.GetMethods();

            // get all the constructors
            Console.WriteLine("Constructors:");
            foreach (ConstructorInfo cf in info)
            {
                Console.WriteLine(cf);
            }

            Console.WriteLine();
            // get all the methods
            Console.WriteLine("Methods:");
            foreach (MethodInfo mf in methods)
            {
                Console.WriteLine(mf);
            }
        }
    }
}
