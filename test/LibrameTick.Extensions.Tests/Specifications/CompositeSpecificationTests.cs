using System;
using System.Collections.Generic;
using Xunit;

namespace Librame.Extensions.Specifications
{
    public class TestClass
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DateTime CreatedTime { get; set; }


        public static IEnumerable<TestClass> GetTestClasses()
        {
            var name = "Test_";

            for (var i = 0; i < 10; i++)
            {
                name += i;

                yield return new TestClass
                {
                    Id = i + 1,
                    Name = name,
                    CreatedTime = DateTime.Now,
                };
            }
        }
    }


    public class CompositeSpecificationTests
    {
        [Fact]
        public void AllTest()
        {
            var idSpecification = new BaseSpecification<TestClass>(t => t.Id > 3);
            var nameSpecification = new BaseSpecification<TestClass>(t => t.Name!.Length > 9);

            var specification = new CompositeSpecification<TestClass>(idSpecification, nameSpecification);
            var result = specification.Evaluate(TestClass.GetTestClasses());
            Assert.NotEmpty(result);
        }

    }
}
