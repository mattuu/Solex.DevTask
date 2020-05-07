using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Security.Principal;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc;

namespace Solex.DevTest.TestUtils
{
    [ExcludeFromCodeCoverage]
    // http://stackoverflow.com/questions/19908385/automocking-web-api-2-controller
    public class WebApiAutoMoqCustomization : CompositeCustomization
    {
        public WebApiAutoMoqCustomization()
            : base(new HttpSchemeCustomization(),
                   new HttpRequestMessageCustomization(),
                   new HttpActionContextCustomization(),
                   new ApiControllerCustomization(),
                   new AutoConfiguredMoqCustomization())
        {
        }

        private class HttpSchemeCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Inject(new UriScheme("http"));
            }
        }

        private class HttpRequestMessageCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<HttpRequestMessage>(c => c
                                                          .Without(x => x.Content)
                                                         );
            }
        }

        private class HttpActionContextCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                //fixture.Register(() => HttpContextUtil.CreateActionContext());
            }
        }

        private class ApiControllerCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customizations.Add(
                    new FilteringSpecimenBuilder(
                        new Postprocessor(
                            new MethodInvoker(
                                new ModestConstructorQuery()),
                            new ApiControllerFiller()),
                        new ApiControllerSpecification()));
            }

            private class ApiControllerFiller : ISpecimenCommand
            {
                public void Execute(object specimen, ISpecimenContext context)
                {
                    if (specimen == null)
                        throw new ArgumentNullException(nameof(specimen));
                    if (context == null)
                        throw new ArgumentNullException(nameof(context));

                    var target = specimen as ControllerBase;
                    if (target == null)
                        throw new ArgumentException(
                            "The specimen must be an instance of ApiController.",
                            nameof(specimen));

                    //target.Request =
                    //    (HttpRequestMessage) context.Resolve(
                    //        typeof(HttpRequestMessage));

                    //target.User = (IPrincipal) context.Resolve(typeof(IPrincipal));
                }
            }

            private class ApiControllerSpecification : IRequestSpecification
            {
                public bool IsSatisfiedBy(object request)
                {
                    var requestType = request as Type;
                    if (requestType == null)
                        return false;
                    return typeof(ControllerBase).IsAssignableFrom(requestType);
                }
            }
        }
    }
}