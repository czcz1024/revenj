﻿// This software is part of the Autofac IoC container
// Copyright © 2011 Autofac Contributors
// http://autofac.org
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using Revenj.Extensibility.Autofac.Builder;
using Revenj.Extensibility.Autofac.Core;
using Revenj.Extensibility.Autofac.Core.Activators.Reflection;

namespace Revenj.Extensibility.Autofac.Features.OpenGenerics
{
	/// <summary>
	/// Generates activators for open generic types.
	/// </summary>
	class OpenGenericRegistrationSource : IRegistrationSource
	{
		readonly RegistrationData _registrationData;
		readonly ReflectionActivatorData _activatorData;

		public OpenGenericRegistrationSource(
			RegistrationData registrationData,
			ReflectionActivatorData activatorData)
		{
			if (registrationData == null) throw new ArgumentNullException("registrationData");
			if (activatorData == null) throw new ArgumentNullException("activatorData");

			OpenGenericServiceBinder.EnforceBindable(activatorData.ImplementationType, registrationData.Services);

			_registrationData = registrationData;
			_activatorData = activatorData;
		}

		public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
		{
			if (service == null) throw new ArgumentNullException("service");
			if (registrationAccessor == null) throw new ArgumentNullException("registrationAccessor");

			Type constructedImplementationType;
			IEnumerable<Service> services;
			if (OpenGenericServiceBinder.TryBindServiceType(service, _registrationData.Services, _activatorData.ImplementationType, out constructedImplementationType, out services))
			{
				yield return RegistrationBuilder.CreateRegistration(
					Guid.NewGuid(),
					_registrationData,
					new ReflectionActivator(
						constructedImplementationType,
						_activatorData.ConstructorFinder,
						_activatorData.ConstructorSelector,
						_activatorData.ConfiguredParameters,
						_activatorData.ConfiguredProperties),
					services);
			}
		}

		public bool IsAdapterForIndividualComponents
		{
			get { return false; }
		}

		public override string ToString()
		{
			return string.Format("{0} providing {1}",
				_activatorData.ImplementationType.FullName,
				string.Join(", ", _registrationData.Services.Select(s => s.Description).ToArray()));
		}
	}
}
