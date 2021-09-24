﻿using Librame.Extensions.Core;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.Accessing
{
    class InternalTestAccessorSeeder : AbstractAccessorSeeder
    {
        public InternalTestAccessorSeeder(CoreExtensionOptions options, IIdentificationGeneratorFactory idGeneratorFactory)
            : base(options.Clock, idGeneratorFactory)
        {
        }


        public User[] GetUsers()
        {
            return Seed(nameof(GetUsers), key =>
            {
                var users = new User[10];

                for (var i = 0; i < users.Length; i++)
                {
                    var user = new User
                    {
                        Name = $"Seed Name {i + 1}",
                        Passwd = "123456"
                    };

                    user.Id = IdGeneratorFactory.GetNewId<long>();
                    user.PopulateCreation(0, DateTimeOffset.UtcNow);

                    users[i] = user;
                }

                return users;
            });
        }

        public Task<User[]> GetUsersAsync(CancellationToken cancellationToken = default)
            => cancellationToken.RunTask(GetUsers);

    }
}