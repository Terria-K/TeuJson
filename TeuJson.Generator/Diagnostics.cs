using Microsoft.CodeAnalysis;

namespace TeuJson.Generator;

internal static class TeuDiagnostic
{
    internal static readonly DiagnosticDescriptor ThreeArgumentsRule 
        = new("TE001", "Three type arguments are not supported.", 
            "Three type arguments are not supported", "Usage", DiagnosticSeverity.Error, true, "Use one type argument to fix this error.");

    internal static readonly DiagnosticDescriptor KeyTypeRule
        = new("TE002", "Key types are not supported.", 
            "Key types other than string are not supported", "Usage", DiagnosticSeverity.Error, true, "Use string as a key type instead.");

    internal static readonly DiagnosticDescriptor WillNotWork
        = new("TE003", "Two serialization options are disabled.", 
            "Two serialization options in this class are disabled", "Usage", DiagnosticSeverity.Warning, true, 
            "Enable one of the serialization option explicitly to make it work.");

    internal static readonly DiagnosticDescriptor RecordRule
        = new("TE004", "Record Types are not yet supported with deserialization.", 
            "Record Types are not yet supported with deserialization, but it will come in the future.", 
            "Usage", DiagnosticSeverity.Warning, true, "Mark it as class or struct.");

    internal static readonly DiagnosticDescriptor StructIfNull
        = new("TE005", "Struct type does not need to have [IfNull] Atrribute", 
            "Structs are not nullable, it doesn't makes sense to have this attribute.", 
            "Usage", DiagnosticSeverity.Warning, true, "Remove the [IfNull] Attribute");
}