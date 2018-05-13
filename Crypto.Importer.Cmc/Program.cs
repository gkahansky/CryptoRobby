using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.Importer.Cmc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ImporterCmc()

            };
            ServiceBase.Run(ServicesToRun);
        }


    }
}
