using System;

namespace Cami.Core
{
    public interface IUpdatable
    {
        void Update(float timeStep);

        bool Enabled { get; }
    }
}
