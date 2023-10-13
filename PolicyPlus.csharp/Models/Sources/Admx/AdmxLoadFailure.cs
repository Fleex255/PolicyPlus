namespace PolicyPlus.csharp.Models.Sources.Admx
{
    public class AdmxLoadFailure
    {
        public AdmxLoadFailType FailType;
        public string AdmxPath;
        public string Info;

        public AdmxLoadFailure(AdmxLoadFailType failType, string admxPath, string info)
        {
            FailType = failType;
            AdmxPath = admxPath;
            Info = info;
        }

        public AdmxLoadFailure(AdmxLoadFailType failType, string admxPath) : this(failType, admxPath, string.Empty)
        {
        }

        public override string ToString()
        {
            var failMsg = "Couldn't load " + AdmxPath + ": " + GetFailMessage(FailType, Info);
            if (!failMsg.EndsWith("."))
            {
                failMsg += ".";
            }

            return failMsg;
        }

        private static string GetFailMessage(AdmxLoadFailType failType, string info)
        {
            switch (failType)
            {
                case AdmxLoadFailType.BadAdmxParse:
                    {
                        return "The ADMX XML couldn't be parsed: " + info;
                    }
                case AdmxLoadFailType.BadAdmx:
                    {
                        return "The ADMX is invalid: " + info;
                    }
                case AdmxLoadFailType.NoAdml:
                    {
                        return "The corresponding ADML is missing";
                    }
                case AdmxLoadFailType.BadAdmlParse:
                    {
                        return "The ADML XML couldn't be parsed: " + info;
                    }
                case AdmxLoadFailType.BadAdml:
                    {
                        return "The ADML is invalid: " + info;
                    }
                case AdmxLoadFailType.DuplicateNamespace:
                    {
                        return "The " + info + " namespace is already owned by a different ADMX file";
                    }
            }
            return string.IsNullOrEmpty(info) ? "An unknown error occurred" : info;
        }
    }
}