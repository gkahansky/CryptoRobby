using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crypto.Infra;

namespace Crypto.Importer.Base
{
    public abstract class Importer
    {
        private readonly ILogger _logger;
        private readonly IDbHandler _dbHandler;

        public Importer(ILogger logger, IDbHandler dbHandler)
        {
            _logger = logger;
            _dbHandler = dbHandler;
            Parser parser = new Parser(logger);
        }

    }
}
