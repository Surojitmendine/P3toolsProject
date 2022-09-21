using API.Entity;
using API.Helper.ReqObject;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helper
{
    public class RespExample
    {
        public class SampleExamples : IExamplesProvider<IEnumerable<SampleEntity>>
        {
            public IEnumerable<SampleEntity> GetExamples()
            {
                return new List<SampleEntity>
            {
                new SampleEntity { PK_AutoDemoSample = 1, IsActive = true, Auto50_TextBoxVarchar=""/*,EmailIDTextbox="test@example.com",TextBoxVarchr150Auto="",DateTimePicker=DateTime.Now*/ },
                new SampleEntity { PK_AutoDemoSample = 3, IsActive = false, Auto50_TextBoxVarchar=""/*,EmailIDTextbox="test1@example.com",TextBoxVarchr150Auto="",DateTimePicker=DateTime.Now*/ },
            };
            }
        }
    }
}
