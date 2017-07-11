namespace FastClone.Tests
{
    internal class TypeManifest
    {
        public const string NamespaceMask = @"AssemblyToProcess.{0}";

        public const string StaticCloneMethod = @"CloneMethod";

        public const string FastCloneMethod = @"FastClone";

        public const string BuildTestEntityMethod = @"BuildTestEntity";

        public const string PartialWeave = @"PartialWeave";
        public const string LacksParameterlessCtor = @"LacksParameterlessCtor";

        public const string MissingInterface = @"MissingInterface";

        public const string BasicTest = @"BasicTest";

        public const string PrivateCtor = @"PrivateCtor";

        public const string InternalCtor = @"InternalCtor";

        public const string InternalProperties = @"InternalProperties";

        public const string PrivateField = @"PrivateField";

        public const string ConstAndReadOnly = @"ConstAndReadonly";
    }
}