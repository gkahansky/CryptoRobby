using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoRobert.Infra;
using RabbitMQ.Client;

namespace CryptoRobert.Importer.Base
{
    public abstract class Importer
    {
        private readonly ILogger _logger;
        private readonly IDbHandler _dbHandler;
        private readonly IFileHandler _fileHandler;

        public Importer(ILogger logger, IDbHandler dbHandler)
        {
            _logger = logger;
            _dbHandler = dbHandler;
            Parser parser = new Parser(logger);
            if (Config.RecordTicksToFile)
            {
                _fileHandler = new FileHandler(_logger);
            }
                
        }

        
    }
}
