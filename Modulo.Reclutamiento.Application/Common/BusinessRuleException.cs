using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ==========================
// Modulo.Reclutamiento.Application/Common/BusinessRuleException.cs
// ==========================
namespace Modulo.Reclutamiento.Application.Common;

public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
