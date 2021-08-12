#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data
{
    class InternalIdentificationGeneratorFactory : IIdentificationGeneratorFactory
    {
        private IReadOnlyList<IObjectIdentificationGenerator> _idGenerators;


        public InternalIdentificationGeneratorFactory(DataExtensionBuilder builder)
        {
            _idGenerators = builder.Options.IdGenerators;
        }


        public virtual IIdentificationGenerator<TId> GetIdGenerator<TId>()
            => (IIdentificationGenerator<TId>)GetIdGenerator(typeof(TId));

        public virtual IObjectIdentificationGenerator GetIdGenerator(Type idType)
            => _idGenerators.First(p => p.IdType == idType);

    }
}
