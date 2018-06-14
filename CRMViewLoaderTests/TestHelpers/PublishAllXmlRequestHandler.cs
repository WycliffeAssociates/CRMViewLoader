using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeXrmEasy;
using FakeXrmEasy.FakeMessageExecutors;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;

namespace CRMViewLoaderTests.TestHelpers
{
    public class PublishAllXmlRequestHandler : IFakeMessageExecutor
    {
        public bool published = false;
        public bool CanExecute(OrganizationRequest request)
        {
            return request is PublishAllXmlRequest;
        }

        public OrganizationResponse Execute(OrganizationRequest request, XrmFakedContext ctx)
        {
            published = true;
            return new PublishAllXmlResponse();
        }

        public Type GetResponsibleRequestType()
        {
            return typeof(PublishAllXmlRequest);
        }
    }
}
