using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modulo.Contratos.Application.Common;

public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
