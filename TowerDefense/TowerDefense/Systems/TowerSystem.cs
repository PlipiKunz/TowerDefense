using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Systems
{

    /// <summary>
    /// This system knows how to accept keyboard input and use that
    /// to move an entity, based on the entities 'KeyboardControlled'
    /// component settings.
    /// </summary>
    class TowerSystem : System
    {

        public TowerSystem()
            : base(typeof(Components.TowerComponent))
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var movable = entity.GetComponent<Components.PathMovable>();
                var input = entity.GetComponent<Components.KeyboardControlled>();

                
            }
        }
    }
}
