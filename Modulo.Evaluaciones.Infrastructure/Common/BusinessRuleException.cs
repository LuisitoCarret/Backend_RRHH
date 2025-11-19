// Modulo.Evaluaciones.Infrastructure/Common/BusinessRuleException.cs
namespace Modulo.Evaluaciones.Infrastructure.Common;
public sealed class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
