using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace AuthenticationNetCore.Test.Data
{
    public class RegexUtilitiesTestDataSourceAttribute : DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return new object[] { "blabla@gmail.com", true};
            yield return new object[] { "blabl$a@gmail.com", true};
            yield return new object[] { "blablagmail.com", false};
            yield return new object[] { "blabla@gmailcom", false};
            yield return new object[] { "blabl@a@gmail.com", false};
            yield return new object[] { "blabl\a@gmail.com", true};
        }
    }
}