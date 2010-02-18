using System;
using System.Web;
using System.Web.Routing;
using Cogs.Common;
using Cogs.Server.Handlers;
using Moq;
using Xunit;

namespace Cogs.Server.Tests
{
	public class CogRouteHandlerContext
	{
		public Mock<IComponentContainer> containerMock;
		public CogRouteHandler<FakeHandler> routeHandler;

		public CogRouteHandlerContext()
		{
			containerMock = new Mock<IComponentContainer>();
			routeHandler = new CogRouteHandler<FakeHandler>(containerMock.Object);
		}
	}

	public class WhenGetHttpHandlerIsCalled : CogRouteHandlerContext
	{
		[Fact]
		public void ShouldRequestHttpHandlerFromContainer()
		{
			var context = new RequestContext(new FakeHttpContext(), new RouteData());
			var expected = new FakeHandler();

			containerMock.Expect(x => x.Get<FakeHandler>()).Returns(expected).AtMostOnce();
			var handler = routeHandler.GetHttpHandler(context);

			Assert.NotNull(handler);
			Assert.Same(expected, handler);
		}

		[Fact]
		public void ShouldInjectRouteDataIntoHttpHandler()
		{
			var routeData = new RouteData();
			var context = new RequestContext(new FakeHttpContext(), routeData);

			containerMock.Expect(x => x.Get<FakeHandler>()).Returns(new FakeHandler()).AtMostOnce();
			var handler = routeHandler.GetHttpHandler(context) as FakeHandler;

			Assert.NotNull(handler);
			Assert.NotNull(handler.RouteData);
			Assert.Same(routeData, handler.RouteData);
		}
	}

	public class FakeHandler : CogHandlerBase
	{
		public override void ProcessRequest(HttpContext context)
		{
		}
	}
}